using UnityEngine;

public class ChaseState : IEnemyState
{
    private readonly EnemyAI _ai;

    public ChaseState(EnemyAI ai)
    {
        _ai = ai;
    }

    public void Enter()
    {
        _ai.Agent.isStopped = false;
        _ai.Agent.speed = _ai.ChaseSpeed;
        _ai.Animator.SetFloat(EnemyAI.SpeedHash, 1f);
    }

    public void Tick()
    {
        _ai.Agent.SetDestination(_ai.Player.position);

        float distance = Vector3.Distance(_ai.transform.position, _ai.Player.position);

        if (distance <= _ai.AttackRange)
        {
            _ai.StateMachine.ChangeState(_ai.AttackState);
        }
        else if (distance > _ai.ChaseRange)
        {
            _ai.StateMachine.ChangeState(_ai.PatrolState);
        }
    }

    public void Exit()
    {
        _ai.Agent.ResetPath();
    }
}