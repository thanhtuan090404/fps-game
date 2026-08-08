using UnityEngine;

public class AttackState : IEnemyState
{
    private readonly EnemyAI _ai;

    public AttackState(EnemyAI ai)
    {
        _ai = ai;
    }


    public void Enter()
    {
        _ai.Agent.isStopped = true;

        _ai.FacePlayer();

        _ai.Animator.SetFloat(
            EnemyAI.SpeedHash,
            0f
        );
    }


    public void Tick()
    {
        _ai.FacePlayer();

        float distance =
            Vector3.Distance(
                _ai.transform.position,
                _ai.Player.position
            );


        // Player chạy ra khỏi tầm đánh
        if (distance > _ai.AttackRange + 0.5f)
        {
            _ai.Agent.isStopped = false;

            _ai.StateMachine.ChangeState(
                _ai.ChaseState
            );

            return;
        }


        // Bắt đầu đánh
        _ai.StartAttack();
    }


    public void Exit()
    {
        _ai.Agent.isStopped = false;
    }
}