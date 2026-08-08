using System.Collections;
using UnityEngine;
using UnityEngine.AI;

public class EnemyAI : MonoBehaviour
{
    private NavMeshAgent agent ; // biến lưu trữ component NavMeshagent của enemy
    public NavMeshAgent Agent => agent; // property để truy cập component NavMeshagent của enemy

    private Transform player;
    public Transform Player => player; // property để truy cập transform của player
    private Health playerHealth; // biến lưu trữ component Health của player

    [SerializeField] private float detectRange = 5f; 
    public float ChaseRange => detectRange; // property để truy cập khoảng cách phát hiện player

    [SerializeField]private float attackRange = 4f; // khoảng cách để tấn công player 
    public float AttackRange => attackRange; // property để truy cập khoảng cách tấn công của enemy
    private bool isAttacking = false; // biến để xác định xem enemy có đang tấn công hay không
   [SerializeField] private float attackWindup = 1f; // thời gian chờ trước khi gây sát thương
    [SerializeField]  private float attackCooldown = 1f; // thời gian chờ trước khi có thể tấn công lại
    [SerializeField] private float damage = 50f;


    [SerializeField] private Transform[] pointsA;
    [SerializeField] private Transform[] pointsB;
    [SerializeField] private Transform target; // điểm mà enemy đang di chuyển đến
    public Transform Target => target;
    private bool isGoingToA = true; // biến để xác định xem enemy đang đi đến điểm A hay điểm B

    [SerializeField] private AudioSource audioSource;
    [SerializeField] private AudioClip[] footstepClip; // mảng lưu trữ các clip âm thanh bước chân của enemy
    [SerializeField] private AudioClip attackClip; // clip âm thanh khi enemy tấn công player
    [SerializeField] private float stepInterval = 0.5f; // khoảng thời gian giữa các bước chân

    private float footstepTimer = 0f; // biến đếm thời gian giữa các bước chân
    [SerializeField] private Animator animator;
    public Animator Animator => animator; // property để truy cập component Animator của enemy
    private static readonly int speedHash = Animator.StringToHash("Speed"); // hash của tham số Speed trong Animator để tăng hiệu suất
    public static int SpeedHash => speedHash; // property để truy cập hash của tham số Speed trong Animator
    private static readonly int attackHash = Animator.StringToHash("Attack"); // hash của trigger Attack trong Animator để tăng hiệu suất

    [SerializeField] private float patrolSpeed = 2f; // tốc độ di chuyển khi đi tuần tra
    public float PatrolSpeed => patrolSpeed; // property để truy cập tốc độ di chuyển khi đi tuần tra


    [SerializeField] private float chaseSpeed = 3.5f; // tốc độ di chuyển khi truy đuổi player
    public float ChaseSpeed => chaseSpeed; // property để truy cập tốc độ di chuyển khi truy đuổi player

    private StateMachine stateMachine; // state machine của enemy
    public StateMachine StateMachine => stateMachine; // property để truy cập state machine của enemy

    public IEnemyState PatrolState { get; private set; } // trạng thái Patrol của enemy

    public IEnemyState ChaseState { get; private set; } // trạng thái Chase của enemy

    public IEnemyState AttackState { get; private set; } // trạng thái Attack của enemy

    private Vector3 spawnPosition;

    public Vector3 SpawnPosition => spawnPosition;

    [SerializeField] private float patrolRadius = 10f;

    public float PatrolRadius => patrolRadius;


    private void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
        agent.avoidancePriority = Random.Range(30, 70); // đặt độ ưu tiên tránh vật cản ngẫu nhiên để tránh tình trạng enemy bị kẹt khi di chuyển
        animator = GetComponent<Animator>();
        spawnPosition = transform.position;

        GameObject playerObj = GameObject.FindGameObjectWithTag("Player");
        if (playerObj != null)
        {
            player = playerObj.transform;
            playerHealth = playerObj.GetComponent<Health>();
        }

        // Chọn điểm Patrol ban đầu
        if (pointsA != null && pointsA.Length > 0)
        {
            target = pointsA[Random.Range(0, pointsA.Length)];
        }

        // tạo state machie
        stateMachine = new StateMachine();

        PatrolState = new PatrolState(this);
        ChaseState = new ChaseState(this);
        AttackState = new AttackState(this);

        stateMachine.Initialize(PatrolState); // khởi tạo state machine với trạng thái Patrol ban đầu
    }


    // Update is called once per frame
    void Update()
    {
        if (player == null || playerHealth == null || !playerHealth.IsAlive)
        {
            return;
        }
        // cập nhật Speed MỌI frame — khi attack, velocity ~ 0 nên Speed về 0, Animator không nhầm sang Walking
        animator.SetFloat(speedHash, agent.velocity.magnitude);

        stateMachine.Tick(); // gọi hàm Tick của state machine để cập nhật trạng thái của enemy

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
    // =========================
    // ATTACK
    // =========================

    public void StartAttack()
    {
        if (!isAttacking)
        {
            StartCoroutine(AttackRoutine());
        }
    }


    private IEnumerator AttackRoutine()
    {
       isAttacking = true; // đánh dấu là đang tấn công để tiếp tục
        animator.SetTrigger(attackHash); // kích hoạt animation tấn công

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

    public void FacePlayer()
    {
        if (player == null)
            return;

        Vector3 lookPos = player.position;
        lookPos.y = transform.position.y;

        transform.LookAt(lookPos);
    }

    // PATROL


    public void ChooseNextPoint()
    {
        if (pointsA == null ||
            pointsB == null ||
            pointsA.Length == 0 ||
            pointsB.Length == 0)
        {
            return;
        }

        if (isGoingToA)
        {
            target =
                pointsB[
                    Random.Range(
                        0,
                        pointsB.Length
                    )
                ];

            isGoingToA = false;
        }
        else
        {
            target =
                pointsA[
                    Random.Range(
                        0,
                        pointsA.Length
                    )
                ];

            isGoingToA = true;
        }
    }

    private void OnDisable()
    {
        isAttacking = false;
    }
}
