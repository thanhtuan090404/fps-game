using UnityEngine;
using UnityEngine.AI;

public class EnemyAI : MonoBehaviour
{
    private NavMeshAgent agent; // biến lưu trữ component NavMeshAgent của enemy
    [SerializeField] private Transform player;
    [SerializeField] private float detectRange = 5f; 
    private State _currentState = State.Patrol; // trạng thái hiện tại của enemy

    [SerializeField] private Transform[] PointsA;
    [SerializeField] private Transform[] PointB;
    [SerializeField] private Transform target; // điểm mà enemy đang di chuyển đến
    private bool isGoingToA = true; // biến để xác định xem enemy đang đi đến điểm A hay điểm B
    private enum State
    {
        Patrol, Chase
    }
    private void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
        target = PointsA[Random.Range(0, PointsA.Length)];
    }


    // Update is called once per frame
    void Update()
    {
        switch (_currentState)
        {
            case State.Patrol:
                agent.SetDestination(target.position);
                break;
            case State.Chase:
                agent.SetDestination(player.position);
                break;


        }
        float distance = Vector3.Distance(transform.position, player.position); // tính khoảng cách giữa enemy và player
        if (distance < detectRange)
        {
            _currentState = State.Chase;
        }
        else
        {
            _currentState = State.Patrol;
        }
        if (!agent.pathPending && agent.remainingDistance <= agent.stoppingDistance) // kiểm tra xem enemy đã đến điểm đích chưa
        {
            ChooseNextPoint();
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
}
