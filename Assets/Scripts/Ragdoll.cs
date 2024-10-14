using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ragdoll : MonoBehaviour
{
    [SerializeField] private Animator animator;
    [SerializeField] private CharacterController controller;
    private Collider[] colliders;
    private Rigidbody[] allRigidBodies;
    
    void Start()
    {
        colliders = GetComponentsInChildren<Collider>(true);
        allRigidBodies = GetComponentsInChildren<Rigidbody>(true);

        ToggleRagdoll(false);
    }

    public void ToggleRagdoll(bool isRagdoll)
    {
        foreach(Collider collider in colliders)
        {
            if(collider.CompareTag("Ragdoll"))
            {
                collider.enabled = isRagdoll;
            }
        }

        foreach (Rigidbody rb in allRigidBodies)
        {
            if (rb.CompareTag("Ragdoll"))
            {
                rb.isKinematic = !isRagdoll;
                rb.useGravity = isRagdoll;
            }
        }

        controller.enabled = !isRagdoll;
        animator.enabled = !isRagdoll;
    }
   
}
