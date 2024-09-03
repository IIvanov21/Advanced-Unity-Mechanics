using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
 * Base class for all player states, inheriting from the State class..
 * Provides common functionality for player states by holding a references for the PlayerStateMachine.
 */
public abstract class PlayerBaseState : State
{
    /*
     * Reference to the Player State Machine that this state is part of.
     */
    protected PlayerStateMachine stateMachine;

    /*
     * Constructor that initializes the state with a reference to the Player State Machine.
     * The param stateMachine is a reference to the stateMachine this state belongs to.
     */
    public PlayerBaseState(PlayerStateMachine stateMachine)
    {
        this.stateMachine = stateMachine;
    }

    /*
     * Simple function which moves the player during an attack only using the given force of that attack.
     * It can also be applied in other physics, such as when the player is hit.
     */
    protected void Move(float deltaTime)
    {
        stateMachine.Controller.Move((Vector3.zero + stateMachine.ForceReceiver.Movement) * deltaTime);
    }

    /*
     * This Move function, takes in account input movement and can be used for normal scenarios, in all states.
     */
    protected void Move(Vector3 motion, float deltaTime)
    {
        stateMachine.Controller.Move((motion + stateMachine.ForceReceiver.Movement) * deltaTime);
    }

    private void FaceTarget()
    {
        //If there is no target, exit early.
        if (stateMachine.Targeter.CurrentTarget == null) return;

        //If there is a target make the player always face that target.
        //A simple version of LookAt function.
        Vector3 facingVector = stateMachine.Targeter.CurrentTarget.transform.position - stateMachine.transform.position;

        facingVector.y = 0.0f;

        stateMachine.transform.rotation = Quaternion.LookRotation(facingVector);
    }
}
