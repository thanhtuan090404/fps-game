using UnityEngine;
using UnityEngine.AI;

public class PatrolState : IEnemyState
{
    private readonly EnemyAI _ai;

    public PatrolState(EnemyAI ai)
    {
        _ai = ai;
    }

    public void Enter()
    {
        _ai.Agent.isStopped = false;
        _ai.Agent.speed = _ai.PatrolSpeed;

        _ai.Animator.SetFloat(
            EnemyAI.SpeedHash,
            1f
        );

        ChooseRandomPoint();
    }

    public void Tick()
    {
        float distance = Vector3.Distance(
            _ai.transform.position,
            _ai.Player.position
        );

        // Phát hiện Player
        if (distance <= _ai.ChaseRange)
        {
            _ai.StateMachine.ChangeState(
                _ai.ChaseState
            );

            return;
        }

        // Đã tới điểm → tìm điểm mới
        if (!_ai.Agent.pathPending &&
            _ai.Agent.remainingDistance <=
            _ai.Agent.stoppingDistance)
        {
            ChooseRandomPoint();
        }
    }

    private void ChooseRandomPoint()
    {
        Vector3 randomPoint =
            _ai.SpawnPosition +
            Random.insideUnitSphere * _ai.PatrolRadius;

        if (NavMesh.SamplePosition(
            randomPoint,
            out NavMeshHit hit,
            _ai.PatrolRadius,
            NavMesh.AllAreas))
        {
            _ai.Agent.SetDestination(hit.position);
        }
    }

    public void Exit()
    {
        _ai.Agent.ResetPath();
    }
}