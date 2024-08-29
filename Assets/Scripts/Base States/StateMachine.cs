using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
 * Base class for a state machine that manages transitions between different states.
 */

public abstract class StateMachine : MonoBehaviour
{
    /*
     * The current active state in the state machine.
     */
    private State currentState;

    /*
     * Using Unity's default method to execute the Tick method of the current state.
     */
    private void Update()
    {
        //If there is a current state call it's Tick method. Remember the '?' checks if the reference is empty or not.
        currentState?.Tick(Time.deltaTime);
    }

    /*
     * Switches the state machine to a new state.
     * newState is used to know what state machine should transition to.
     */
    public void SwitchState(State newState)
    {
        //If there is a current state, call it's Exit method.
        currentState?.Exit();

        //Assign the newState we are transitioning to.
        currentState = newState;

        //If there is a current state, call it's enter method.
        currentState?.Enter();
    }
}
