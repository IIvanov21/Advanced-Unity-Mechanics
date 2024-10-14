using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyChasingState : EnemyBaseState
{
    private readonly int SpeedHash = Animator.StringToHash("Speed");
    private readonly float AnimationDamp = 0.1f;
    private readonly int LocomotionBlendTreeHash = Animator.StringToHash("Locomotion");
    private const float CrossFadeTime = 0.1f;

    public EnemyChasingState(EnemyStateMachine stateMachine) : base(stateMachine)
    {
    }

    public override void Enter()
    {
        stateMachine.Animator.CrossFadeInFixedTime(LocomotionBlendTreeHash, CrossFadeTime);
    }

    public override void Tick(float deltaTime)
    {
        MoveToPlayer(deltaTime);
        FacePlayer();
        if(!IsInChaseRange())
        {
            stateMachine.SwitchState(new EnemyIdleState(stateMachine));
            return;
        }
        else if(IsInAttackRange()) 
        {
            stateMachine.SwitchState(new EnemyAttackingState(stateMachine));
            return;
        }

        stateMachine.Animator.SetFloat(SpeedHash, 1f, AnimationDamp, deltaTime);

    }

    public override void Exit()
    {
        stateMachine.Agent.ResetPath();
        stateMachine.Agent.velocity=Vector3.zero;
    }

    private void MoveToPlayer(float deltaTime)
    {
        if (stateMachine.Agent.isOnNavMesh)
        {
            stateMachine.Agent.destination = stateMachine.Player.transform.position;

            Move(stateMachine.Agent.desiredVelocity.normalized * stateMachine.MovementSpeed, deltaTime);
        }

        stateMachine.Agent.velocity = stateMachine.Controller.velocity;
    }

    private bool IsInAttackRange()
    {
        if(stateMachine.Player.IsDead) return false;    
        float distance = (stateMachine.Player.transform.position - stateMachine.transform.position).sqrMagnitude;
        return distance <= stateMachine.AttackRange*stateMachine.AttackRange;
    }

    
}
