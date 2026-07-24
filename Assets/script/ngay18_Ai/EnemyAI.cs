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

    [SerializeField] private Transform[] PointsA;
    [SerializeField] private Transform[] PointB;
    [SerializeField] private Transform target; // điểm mà enemy đang di chuyển đến
    private bool isGoingToA = true; // biến để xác định xem enemy đang đi đến điểm A hay điểm B
    private enum State
    {
        Patrol, Chase , Attack
    }
    private void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
        target = PointsA[Random.Range(0, PointsA.Length)];

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
        float distance = Vector3.Distance(transform.position, player.position);

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
    }

    private void ChaseState(float distance)
    {
        Debug.Log("đang truy đuổi");
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
        Debug.Log("đang tấn công");
        if (distance > attackRange + 0.5f) // nếu player ra khỏi tầm tấn công thì chuyển sang trạng thái truy đuổi
        {
            agent.isStopped = false; // tiếp tục di chuyển khi player ra khỏi tầm tấn công
            _currentState = State.Chase;
        }
    }

    private IEnumerator AttackRoutine()
    {
       isAttacking = true; // đánh dấu là đang tấn công để tiếp tục
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
        Debug.Log("đang tuần tra");
        if (distance <= detectRange)
        {
            _currentState = State.Chase;
        }
    }

    void ChooseNextPoint()
    {
        Debug.Log("Đổi điểm");

        if (PointsA.Length == 0 || PointB.Length == 0)
        {
            return;
        }
        if (isGoingToA)
        {
            target = PointB[Random.Range(0, PointB.Length)];
            isGoingToA = false;
        }
        else
        {
            target = PointsA[Random.Range(0, PointsA.Length)];
            isGoingToA = true;
        }
    }
    private void OnDisable()
    {
        isAttacking = false;
    }
}
