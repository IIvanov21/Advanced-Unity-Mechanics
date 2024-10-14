using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class EnemyBaseState : State
{
    protected EnemyStateMachine stateMachine;

    public EnemyBaseState(EnemyStateMachine stateMachine)
    {
        this.stateMachine = stateMachine;
    }

    protected void Move(Vector3 motion, float deltaTime)
    {
        stateMachine.Controller.Move((motion + stateMachine.ForceReceiver.Movement) * deltaTime);
    }

    protected void Move(float deltaTime)
    {
        stateMachine.Controller.Move((Vector3.zero + stateMachine.ForceReceiver.Movement) * deltaTime);
    }

    protected bool IsInChaseRange()
    {
        if (stateMachine.Player.IsDead) return false;

        float magnitude = (stateMachine.Player.transform.position - stateMachine.transform.position).sqrMagnitude;

        return magnitude <= stateMachine.PlayerChasingRange*stateMachine.PlayerChasingRange;
    }

   

    protected void FacePlayer()
    {
        if (stateMachine.Player == null) return;

        Vector3 facingVector = stateMachine.Player.transform.position - stateMachine.transform.position;

        facingVector.y = 0.0f;

        stateMachine.transform.rotation = Quaternion.LookRotation(facingVector);
    }
}
