using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/*
 * State machine specifically for managing player states.
 */
public class PlayerStateMachine : StateMachine
{
    // The input reader provides the player input data.
    [field:SerializeField]
    public InputReader InputReader{  get; private set; }

    /*
     * Intialise the player state machine by setting the initial state.
     */
    private void Start()
    {
        //Switch the initial player state at the start of the game.
        SwitchState(new PlayerTestState(this));
    }
}
