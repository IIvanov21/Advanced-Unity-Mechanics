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
        // Smoothly rotate the character towards the movement direction
        // The rotation is interpolated using Quaternion.Lerp with rotation damping
        stateMachine.transform.rotation = Quaternion.Lerp(stateMachine.transform.rotation,
            Quaternion.LookRotation(stateMachine.MovementVector), deltaTime * stateMachine.RotationDamping);
    }
}
