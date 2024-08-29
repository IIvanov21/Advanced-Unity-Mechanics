using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/*
 * A test state to demonstrate basic functionality for the Player.
 */
public class PlayerTestState : PlayerBaseState
{
    /*
     * Constructor that passes the PlayerStateMachine reference to the base class.
     */
    public PlayerTestState(PlayerStateMachine stateMachine) : base(stateMachine) { }

    public override void Enter()
    {
        Debug.Log("Entering the state");
    }

    public override void Exit()
    {
        Debug.Log("Exiting the state.");
    }

    /*
     * Example of handling state functionality.
     * It handles the player movememnt based on the information passed from the Input Reader.
     */
    public override void Tick(float deltaTime)
    {
        Vector3 movement = new Vector3();
        movement.x=stateMachine.InputReader.MovementValue.x;
        movement.y = 0;
        movement.z = stateMachine.InputReader.MovementValue.y;
        stateMachine.transform.Translate(movement*deltaTime);
    }
}
