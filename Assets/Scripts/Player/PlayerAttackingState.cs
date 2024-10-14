using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAttackingState : PlayerBaseState
{
    private Attack attack;
    private float previousFrameTime;
    private bool alreadyAppliedForce = false;

    public PlayerAttackingState(PlayerStateMachine stateMachine, int attackIndex) : base(stateMachine)
    {
        attack = stateMachine.Attacks[attackIndex];
    }

    // This method is called when the state is entered
    public override void Enter()
    {
        // Start the attack animation using a crossfade transition
        stateMachine.Animator.CrossFadeInFixedTime(attack.AnimationName, attack.TransitionDuration);

        // Set the damage value for the weapon based on the current attack
        stateMachine.WeaponDamage.SetAttack(attack.Damage,attack.Knockback);
    }

    // This method is called on each frame update while in this state
    public override void Tick(float deltaTime)
    {
        // Handle player movement
        Move(deltaTime);

        // Ensure the player is facing the target
        FaceTarget();

        // Get the normalized time of the current attack animation (progression of the animation from 0 to 1)
        float normalisedTime = GetNormalisedTime();

        // If the animation hasn't completed
        if (normalisedTime < 1f)
        {
            // Apply force at the designated time during the attack animation
            if (normalisedTime >= attack.ForceTime)
            {
                TryApplyForce();
            }

            // If the player is attempting a combo attack, check if it's the right time to transition to the combo state
            if (stateMachine.InputReader.IsAttacking)
            {
                TryComboAttack(normalisedTime);
            }
        }
        else // If the animation is complete
        {
            // Switch to the appropriate state based on whether there's a current target or not
            if (stateMachine.Targeter.CurrentTarget != null)
                stateMachine.SwitchState(new PlayerTargetingState(stateMachine));
            else
                stateMachine.SwitchState(new PlayerFreeLookState(stateMachine));
        }

        // Store the current normalized time for use in the next frame
        previousFrameTime = normalisedTime;
    }

    // This method is called when the state is exited
    public override void Exit()
    {
        // No special actions needed when exiting this state
    }

    // Attempt to transition to a combo attack state if the conditions are met
    private void TryComboAttack(float normalisedTime)
    {
        // If there's no combo available, return early
        if (attack.ComboStateIndex == -1) return;

        // If the normalized time hasn't reached the combo attack window, return early
        if (normalisedTime < attack.ComboAttackTime) return;

        // Transition to the next combo attack state
        stateMachine.SwitchState
            (
            new PlayerAttackingState
                (
                    stateMachine,
                    attack.ComboStateIndex
                )
            );
    }

    // Get the normalized time of the current or next attack animation
    private float GetNormalisedTime()
    {
        // Get the current and next animator state info
        AnimatorStateInfo currentInfo = stateMachine.Animator.GetCurrentAnimatorStateInfo(0);
        AnimatorStateInfo nextInfo = stateMachine.Animator.GetNextAnimatorStateInfo(0);

        // If the animator is transitioning into an attack state, return the normalized time of the next state
        if (stateMachine.Animator.IsInTransition(0) && nextInfo.IsTag("Attack"))
        {
            return nextInfo.normalizedTime;
        }
        // If not transitioning but already in an attack state, return the normalized time of the current state
        else if (!stateMachine.Animator.IsInTransition(0) && currentInfo.IsTag("Attack"))
        {
            return currentInfo.normalizedTime;
        }
        else
        {
            // If neither condition is met, return 0.0f
            return 0.0f;
        }
    }

    // Apply force to the player during the attack if it hasn't been applied yet
    private void TryApplyForce()
    {
        // If force has already been applied, return early
        if (alreadyAppliedForce) return;

        // Apply force in the direction the player is facing based on the attack's force value
        stateMachine.ForceReceiver.AddForce(stateMachine.transform.forward * attack.Force);

        // Mark that the force has been applied
        alreadyAppliedForce = true;
    }
}
