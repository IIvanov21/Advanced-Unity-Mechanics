using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerTargetingState : PlayerBaseState
{
    /*
    * Constructor that passes the PlayerStateMachine reference to the base class.
    */
    public PlayerTargetingState(PlayerStateMachine stateMachine) : base(stateMachine) { }

    /*
     * Animation references
     * We will use different type of movement, so we need aditional references for forwards/backwards and left/right.
     */
    private readonly int TargetingBlendTree = Animator.StringToHash("TargetingBlendTree");
    private readonly int TargetingForward = Animator.StringToHash("TargetingForward");
    private readonly int TargetingRight = Animator.StringToHash("TargetingRight");
    private const float AnimatorDampTime = 0.1f;


    public override void Enter()
    {
        Debug.Log("Target State");
        //Transition to Tageting Animation Blend Tree
        stateMachine.Animator.CrossFadeInFixedTime(TargetingBlendTree, AnimatorDampTime);
        //Subscribe to the Cancel Target event
        stateMachine.InputReader.CancelTargetEvent += OnCancel;
    }

    public override void Exit()
    {//Unsubscribe from the Cancel Target Event
        stateMachine.InputReader.CancelTargetEvent -= OnCancel;
    }

    public override void Tick(float deltaTime)
    {
        //If there is no target, we want to switch states immediatly to the FreeLookState
        if(stateMachine.Targeter.CurrentTarget == null)
        {
            stateMachine.SwitchState(new PlayerFreeLookState(stateMachine));
        }

        //Calculate normal movememnt
        stateMachine.MovementVector = CalculateMovement();
        stateMachine.Controller.Move(stateMachine.MovementVector * deltaTime*stateMachine.TargetingMovementSpeed);

        //Face the target
        FaceTarget();

        //Update Animations
        UpdateAnimator(deltaTime);

    }

    private void OnCancel()
    {
        //Tidy up Targeting behaviour
        stateMachine.Targeter.Cancel();

        //Switch back to normal Free Look State
        stateMachine.SwitchState(new PlayerFreeLookState(stateMachine));
    }

    //Simple calculation for horizontal and vertical movememnt.
    private Vector3 CalculateMovement()
    {
        Vector3 movement = new Vector3();

        movement += stateMachine.transform.right * stateMachine.InputReader.MovementValue.x;
        movement += stateMachine.transform.forward * stateMachine.InputReader.MovementValue.y;

        return movement;
    }

    private void UpdateAnimator(float deltaTime)
    {
        //If the player is Idle, zero out both animation movement values
        if (stateMachine.InputReader.MovementValue == Vector2.zero)
        {
            stateMachine.Animator.SetFloat(TargetingRight, 0, AnimatorDampTime, deltaTime);
            stateMachine.Animator.SetFloat(TargetingForward, 0, AnimatorDampTime, deltaTime);
            return;
        }
        //If the player is moving pass in Input values into the animation values.
        stateMachine.Animator.SetFloat(TargetingForward, stateMachine.InputReader.MovementValue.y, AnimatorDampTime, deltaTime);
        stateMachine.Animator.SetFloat(TargetingRight, stateMachine.InputReader.MovementValue.x, AnimatorDampTime, deltaTime);

    }

    private void FaceTarget()
    {
        //If there is no target, exit early.
        if (stateMachine.Targeter.CurrentTarget == null) return;

        //If there is a target make the player always face that target.
        //A simple version of LookAt function.
        Vector3 facingVector = stateMachine.Targeter.CurrentTarget.transform.position-stateMachine.transform.position;

        facingVector.y = 0.0f;

        stateMachine.transform.rotation = Quaternion.LookRotation(facingVector);
    }
}
