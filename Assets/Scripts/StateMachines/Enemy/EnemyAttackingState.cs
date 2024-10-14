using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAttackingState : EnemyBaseState
{
    private readonly int AttackHash = Animator.StringToHash("Attack");
    private readonly float AnimationDamp = 0.1f;
    private readonly float CrossFadeTime = 0.1f;


    public EnemyAttackingState(EnemyStateMachine stateMachine) : base(stateMachine)
    {
    }

    public override void Enter()
    {
        stateMachine.Weapon.SetAttack(stateMachine.AttackDamage,stateMachine.Knockback);
        stateMachine.Animator.CrossFadeInFixedTime(AttackHash, CrossFadeTime);

    }

    public override void Tick(float deltaTime)
    {
        if (GetNormalisedTime(stateMachine.Animator,"Attack") > 1.0f)
        {
            stateMachine.SwitchState(new EnemyChasingState(stateMachine));
        }

        FacePlayer();
    }

    public override void Exit()
    {
    }


    
}
