using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerFreeLookState : PlayerBaseState
{
    /*
     * Constructor that passes the PlayerStateMachine reference to the base class.
     */
    public PlayerFreeLookState(PlayerStateMachine stateMachine) : base(stateMachine) { }

    /*
     * Animation variables
     */
    private readonly int FreeLookSpeedHash = Animator.StringToHash("FreeLookSpeed");
    private readonly int FreeLookBlendTree = Animator.StringToHash("FreeLookBlendTree");//Add a hash, for the Free Look Blend Tree
    private const float AnimatorDampTime = 0.1f;

    public override void Enter()
    {
        Debug.Log("Free State");
        //We need to smoothly transition back to the Free Look Blend Tree when coming back from other states
        stateMachine.Animator.CrossFadeInFixedTime(FreeLookBlendTree, AnimatorDampTime);
        //Bind our target event to the Input Target Event
        stateMachine.InputReader.TargetEvent += OnTarget;
    }

    public override void Exit()
    {
        //Tidy up, and unsubscribe the OnTarget event
        stateMachine.InputReader.TargetEvent += OnTarget;
    }

    public override void Tick(float deltaTime)
    {
        stateMachine.MovementVector = CalculateMovement();

        stateMachine.Controller.Move(stateMachine.MovementVector * deltaTime * stateMachine.FreeLookMovementSpeed);

        FaceMovementDirection(deltaTime);
    }

    /*
     * Movement Functions
     */
    Vector3 CalculateMovement()
    {
        // Get the forward direction of the camera
        Vector3 cameraForward = stateMachine.MainCameraTransform.transform.forward;
        // Zero out the Y component to keep movement on the horizontal plane
        cameraForward.y = 0;
        // Normalize the forward vector to ensure it has a magnitude of 1
        cameraForward.Normalize();

        // Get the right direction of the camera
        Vector3 cameraRight = stateMachine.MainCameraTransform.transform.right;
        // Zero out the Y component to keep movement on the horizontal plane
        cameraRight.y = 0;
        // Normalize the right vector to ensure it has a magnitude of 1
        cameraRight.Normalize();

        // Calculate the movement vector based on camera orientation and input values
        // 'MovementValue.y' corresponds to the input for forward/backward movement
        // 'MovementValue.x' corresponds to the input for right/left movement
        return cameraForward * stateMachine.InputReader.MovementValue.y + cameraRight * stateMachine.InputReader.MovementValue.x;
    }

    private void FaceMovementDirection(float deltaTime)//Call in the Tick method
    {
        //Animation Handling
        // Check if there is no movement input (i.e., the MovementValue is zero)
        if (stateMachine.InputReader.MovementValue == Vector2.zero)
        {
            // Set the FreeLookSpeed parameter in the Animator to 0 with damping, making the character stop moving in the animation
            stateMachine.Animator.SetFloat(FreeLookSpeedHash, 0, AnimatorDampTime, deltaTime);
            return;// Exit the method early as there's no movement
        }

        // If movement input is detected, set the FreeLookSpeed parameter in the Animator to 1 with damping
        stateMachine.Animator.SetFloat(FreeLookSpeedHash, 1, AnimatorDampTime, deltaTime);

        // Smoothly rotate the character towards the movement direction
        // The rotation is interpolated using Quaternion.Lerp with rotation damping
        stateMachine.transform.rotation = Quaternion.Lerp(stateMachine.transform.rotation,
            Quaternion.LookRotation(stateMachine.MovementVector), deltaTime * stateMachine.RotationDamping);
    }

    private void OnTarget()
    {

        //If there is no target within the Player's range. Exit the method without switching.
        if (!stateMachine.Targeter.SelectTarget()) return;
        Debug.Log("Tab");

        //If there is a target switch to the Targeting State
        stateMachine.SwitchState(new PlayerTargetingState(stateMachine));
    }
}
