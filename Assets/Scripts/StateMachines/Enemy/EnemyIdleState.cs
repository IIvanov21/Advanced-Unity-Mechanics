using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Scripting.APIUpdating;

public class EnemyIdleState : EnemyBaseState
{
    private readonly int SpeedHash = Animator.StringToHash("Speed");
    private readonly int LocomotionBlendTreeHash = Animator.StringToHash("Locomotion");
    private readonly float AnimationDamp = 0.1f;
    private readonly float CrossFadeDuration = 0.1f;

    public EnemyIdleState(EnemyStateMachine stateMachine) : base(stateMachine)
    {

    }

    public override void Enter()
    {
        

        stateMachine.Animator.CrossFadeInFixedTime(LocomotionBlendTreeHash, CrossFadeDuration);
    }

    public override void Tick(float deltaTime)
    {
        Move(deltaTime);
        FacePlayer();

        if (IsInChaseRange())
        {
            stateMachine.SwitchState(new EnemyChasingState(stateMachine));  
            return;
        }

        stateMachine.Animator.SetFloat(SpeedHash, 0f, AnimationDamp,deltaTime);

    }

    public override void Exit()
    {

    }

}
