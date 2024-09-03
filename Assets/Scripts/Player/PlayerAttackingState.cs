using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAttackingState : PlayerBaseState
{
    private Attack attack;
    private float previousFrameTime;
    private bool alreadyAppliedForce = false;

    public PlayerAttackingState(PlayerStateMachine stateMachine, int attackIndex) : base(stateMachine)
    {
        attack = stateMachine.Attacks[attackIndex];
    }

    public override void Enter()
    {
    }

    public override void Exit()
    {
    }

    public override void Tick(float deltaTime)
    {
    }
}
