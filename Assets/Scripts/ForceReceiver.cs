using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
public class ForceReceiver : MonoBehaviour
{
    [SerializeField]private CharacterController characterController;
    [SerializeField] private NavMeshAgent agent;

    private float verticalVelocity;
    private Vector3 impact;
    private Vector3 dampingVelocity;
    public Vector3 Movement => impact + Vector3.up * verticalVelocity;
    [SerializeField] private float drag = 0.3f;
    private void Update()
    {
        //Gravity Handler
        if (characterController.isGrounded && verticalVelocity<0.0f)
        {
            verticalVelocity = Physics.gravity.y*Time.deltaTime;  
        }
        else
        { 
            verticalVelocity += Physics.gravity.y*Time.deltaTime;
        }

        impact = Vector3.SmoothDamp(impact, Vector3.zero, ref dampingVelocity,drag);

        if (impact.sqrMagnitude < 0.2f*0.2f)
        {
            impact = Vector3.zero;
            if (agent != null)agent.enabled = true;
        }
    }

    public void AddForce(Vector3 force)
    {
        impact += force;
        if (agent != null)
        {
            agent.enabled = false;
        }

    }

    public void Jump(float jumpForce)
    {
        verticalVelocity += jumpForce;
    }

    internal void Reset()
    {
        impact = Vector3.zero;
        verticalVelocity = 0.0f;
    }
}
