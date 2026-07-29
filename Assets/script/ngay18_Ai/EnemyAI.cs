using System.Collections;
using UnityEngine;
using UnityEngine.AI;

public class EnemyAI : MonoBehaviour
{
    private NavMeshAgent agent; // biến lưu trữ component NavMeshAgent của enemy
     private Transform player;
    private Health playerHealth; // biến lưu trữ component Health của player
    [SerializeField] private float detectRange = 5f; 
    [SerializeField]private float attackRange = 2f; // khoảng cách để tấn công player 
    private bool isAttacking = false; // biến để xác định xem enemy có đang tấn công hay không
   [SerializeField] private float attackWindup = 1f; // thời gian chờ trước khi gây sát thương
    [SerializeField]  private float attackCooldown = 1f; // thời gian chờ trước khi có thể tấn công lại
    [SerializeField] private float damage = 10f;


    private State _currentState = State.Patrol; // trạng thái hiện tại của enemy

    [SerializeField] private Transform[] pointsA;
    [SerializeField] private Transform[] pointsB;
    [SerializeField] private Transform target; // điểm mà enemy đang di chuyển đến
    private bool isGoingToA = true; // biến để xác định xem enemy đang đi đến điểm A hay điểm B

    [SerializeField] private AudioSource audioSource;
    [SerializeField] private AudioClip[] footstepClip; // mảng lưu trữ các clip âm thanh bước chân của enemy
    [SerializeField] private AudioClip attackClip; // clip âm thanh khi enemy tấn công player
    [SerializeField] private float stepInterval = 0.5f; // khoảng thời gian giữa các bước chân

    private float footstepTimer = 0f; // biến đếm thời gian giữa các bước chân
    [SerializeField] private Animator animator;

    private static readonly int speedHash = Animator.StringToHash("Speed"); // hash của tham số Speed trong Animator để tăng hiệu suất
    private static readonly int attackHash = Animator.StringToHash("Attack"); // hash của trigger Attack trong Animator để tăng hiệu suất


    private enum State
    {
        Patrol, Chase , Attack
    }
    private void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
        target = pointsA[Random.Range(0, pointsA.Length)];
        animator = GetComponent<Animator>();

        GameObject playerObj = GameObject.FindGameObjectWithTag("Player");
        if (playerObj != null)
        {
            player = playerObj.transform;
            playerHealth = playerObj.GetComponent<Health>();
        }
    }


    // Update is called once per frame
    void Update()
    {
        if (player == null || playerHealth == null || !playerHealth.IsAlive)
        {
            return;
        }
        float distance = Vector3.Distance(transform.position, player.position);
        // cập nhật Speed MỌI frame — khi attack, velocity ~ 0 nên Speed về 0, Animator không nhầm sang Walking
        animator.SetFloat(speedHash, agent.velocity.magnitude);
        switch (_currentState)
        {
            case State.Patrol:
                agent.SetDestination(target.position);
                PatrolState(distance);
                if (!agent.pathPending &&
                    agent.remainingDistance <= agent.stoppingDistance)
                {
                    ChooseNextPoint();
                }
                break;

            case State.Chase:
                agent.SetDestination(player.position);
                ChaseState(distance);
                break;

            case State.Attack:
                AttackState(distance);
                break;
        }
        if (_currentState == State.Patrol || _currentState == State.Chase)
        {

            HandleFootsteps();   
        }
    }

    private void HandleFootsteps()
    {
        if (audioSource == null || footstepClip.Length == 0)
        {
            return; // nếu không có audioSource hoặc không có clip âm thanh bước chân thì thoát
        }
        if (agent.velocity.magnitude > 0.1f && !agent.isStopped)
        {
            footstepTimer += Time.deltaTime;
            if (footstepTimer >= stepInterval)
            {
                footstepTimer = 0f;
                audioSource.pitch = Random.Range(0.8f, 1.2f); // thay đổi pitch ngẫu nhiên để âm thanh bước chân không bị lặp lại
                audioSource.PlayOneShot(footstepClip[Random.Range(0, footstepClip.Length)]); // phát âm thanh bước chân ngẫu nhiên
                audioSource.pitch = 1f; // reset pitch về 1 để không ảnh hưởng đến các âm thanh khác

            }
        }
        else
        {
            footstepTimer = 0f; // reset timer nếu enemy không di chuyển
        }
    }

    private void ChaseState(float distance)
    {
        if (distance <= attackRange)
        {
            _currentState = State.Attack;
        }
        else if (distance > detectRange) 
        {
            _currentState = State.Patrol;
        }
    }

    private void AttackState(float distance)
    {
        agent.isStopped = true; // dừng di chuyển khi tấn công
        FacePlayer(); // quay mặt về phía player

        if (!isAttacking)
        {
            StartCoroutine(AttackRoutine());
        }
        if (distance > attackRange + 0.5f) // nếu player ra khỏi tầm tấn công thì chuyển sang trạng thái truy đuổi
        {
            agent.isStopped = false; // tiếp tục di chuyển khi player ra khỏi tầm tấn công
            _currentState = State.Chase;
        }
    }

    private IEnumerator AttackRoutine()
    {
       isAttacking = true; // đánh dấu là đang tấn công để tiếp tục
        animator.SetTrigger(attackHash); // kích hoạt animation tấn công
        Debug.Log("Attack trigger fired!");   // tạm thời, xoá sau

        if (audioSource != null && attackClip != null)
        {
            audioSource.PlayOneShot(attackClip); // phát âm thanh tấn công

        }

        yield return new WaitForSeconds(attackWindup); // chờ 1 giây trước khi gây sát thương
        if (playerHealth != null && Vector3.Distance(transform.position, player.position) <= attackRange)
        {
            playerHealth.TakeDamage(damage); // gây sát thương cho player
        }
        yield return new WaitForSeconds(attackCooldown); // chờ 1 giây trước khi có thể tấn công lại
        isAttacking = false; // đánh dấu là đã tấn công xong

    }

    private void FacePlayer()
    {
        Vector3 lookPos = player.position;
        lookPos.y = transform.position.y; // giữ nguyên trục y để không bị nghiêng
        transform.LookAt(lookPos); // quay mặt về phía player
    }

    private void PatrolState(float distance)
    {
        if (distance <= detectRange)
        {
            _currentState = State.Chase;

        }
    }

    void ChooseNextPoint()
    {

        if (pointsA.Length == 0 || pointsB.Length == 0)
        {
            return;
        }
        if (isGoingToA)
        {
            target = pointsB[Random.Range(0, pointsB.Length)];
            isGoingToA = false;
        }
        else
        {
            target = pointsA[Random.Range(0, pointsA.Length)];
            isGoingToA = true;
        }
    }
    private void OnDisable()
    {
        isAttacking = false;
    }
}
