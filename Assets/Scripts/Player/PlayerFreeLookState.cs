using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerFreeLookState : PlayerBaseState
{
    /*
     * Constructor that passes the PlayerStateMachine reference to the base class.
     */
    public PlayerFreeLookState(PlayerStateMachine stateMachine) : base(stateMachine) { }

    public override void Enter()
    {
    }

    public override void Exit()
    {
    }

    public override void Tick(float deltaTime)
    {
        stateMachine.MovementVector = CalculateMovement();

        stateMachine.Controller.Move(stateMachine.MovementVector * deltaTime);
    }

    Vector3 CalculateMovement()
    {
        Vector3 movement = new Vector3();
        movement.x = stateMachine.InputReader.MovementValue.x;
        movement.y = 0;
        movement.z = stateMachine.InputReader.MovementValue.y;
        return movement;
    }
}
