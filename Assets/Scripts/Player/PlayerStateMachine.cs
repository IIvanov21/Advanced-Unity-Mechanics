using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/*
 * State machine specifically for managing player states.
 */
public class PlayerStateMachine : StateMachine
{
    // The input reader provides the player input data.
    [field:SerializeField] public InputReader InputReader{  get; private set; }
    [field:SerializeField] public CharacterController Controller { get; private set; }
    public Vector3 MovementVector;

    //Camera movement variables
    [field: SerializeField] public float RotationDamping { get; private set; }
    [field: SerializeField] public float FreeLookMovementSpeed { get; private set; }
    public Transform MainCameraTransform { get; private set; }

    //Animation Variables
    [field: SerializeField] public Animator Animator { get; private set; }

    //Targeting References
    [field: SerializeField]public Targeter Targeter { get; private set; }
    [field: SerializeField]public float TargetingMovementSpeed { get; private set; }
    /*
     * Intialise the player state machine by setting the initial state.
     */
    private void Start()
    {
        MainCameraTransform=Camera.main.transform;
        //Switch the initial player state at the start of the game.
        SwitchState(new PlayerFreeLookState(this));
    }
}
