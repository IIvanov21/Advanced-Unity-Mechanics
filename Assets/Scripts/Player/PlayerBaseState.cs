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
}
