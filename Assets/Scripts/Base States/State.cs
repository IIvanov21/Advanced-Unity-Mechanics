using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
 * Base class for defining states in a state machine.
 */

public abstract class State : MonoBehaviour
{
    /*
     * Called when the state is entered. Used to prepare the current state.
     */
    public abstract void Enter();
    /*
     * Called every frame while the State is entered. 
     * deltaTime is the elapsed time since the last frame. a.k.a FPS
     */
    public abstract void Tick(float deltaTime);
    /*
     * Called when the state is exited. Used to tidy up and transition to next state.
     */
    public abstract void Exit();
}
