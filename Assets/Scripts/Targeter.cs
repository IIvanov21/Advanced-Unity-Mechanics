using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Targeter : MonoBehaviour
{
    /*
     * A list which contains all the available targets to the Player.
     * A simple index which allows us to know which is the Target.
     */
    private List<Target> targets = new List<Target>();
    public Target CurrentTarget { get; private set; }

    /*
     * References to both cameras, to help with targeting calculations.
     */
    [SerializeField] private CinemachineTargetGroup cinemachineTargetGroup;
    private Camera mainCamera;

    private void Start()
    {
        //Grab the Main Camera reference at the start of the game.
        mainCamera=Camera.main;
    }

    /*
     * We will use Trigger Collision to manage Target Destroy behavior
     */
    private void OnTriggerEnter(Collider other)
    {
        if (!other.TryGetComponent<Target>(out Target target))
        {
            //If no Target is found, exit the method early.
            return;
        }

        //If target is found add it to the list of targets and subscribe the RemoveTarget method to the OnDestroyed event.
        targets.Add(target);
        target.OnDestroyed += RemoveTarget;

    }

    private void OnTriggerExit(Collider other)
    {
        if (!other.TryGetComponent<Target>(out Target target))
        {
            //If no Target is found, exit the method early.
            return;
        }

        //If target found run the RemoveTarget to tidy up.
        RemoveTarget(target);
    }

    // Method to select the closest target within the camera's viewport
    public bool SelectTarget()
    {
        // If there are no targets, return false
        if (targets.Count == 0) return false;

        // Variables to track the closest target and its distance from the center of the viewport
        Target closestTarget = null;
        float closestTargetDistance = Mathf.Infinity;

        // Iterate through all targets
        foreach (Target target in targets)
        {
            // Get the target's position in the camera's viewport coordinates
            Vector2 viewPos = mainCamera.WorldToViewportPoint(target.transform.position);

            // Skip the target if it's outside the viewport
            if (viewPos.x < 0.0f || viewPos.x > 1.0f || viewPos.y < 0.0f || viewPos.y > 1.0f) continue;

            // Calculate the squared distance from the center of the viewport
            Vector2 toCenter = viewPos - new Vector2(0.5f, 0.5f);

            // If this target is closer to the center than the previous closest target, update the closest target
            if (toCenter.sqrMagnitude < closestTargetDistance)
            {
                closestTarget = target;
                closestTargetDistance = toCenter.sqrMagnitude;
            }
        }

        // If no valid target was found, return false
        if (closestTarget == null) return false;

        // Set the closest target as the current target
        CurrentTarget = closestTarget;

        // Add the current target to the Cinemachine target group with specific weight and radius
        cinemachineTargetGroup.AddMember(CurrentTarget.transform, 1f, 2f);

        // Return true to indicate a target was selected
        return true;
    }

    public void Cancel()
    {
        //If there is no target selected, exit method early.
        if(CurrentTarget == null) return;

        //If we have a target remove it from the current cinemachine target list.
        cinemachineTargetGroup.RemoveMember(CurrentTarget.transform);
        //Release the current target reference
        CurrentTarget = null;
    }

    //Tidy up method, when done using a target.
    public void RemoveTarget(Target target)
    {
        //If target passed in is the same as Current Target, tidy up the current target reference.
        if(CurrentTarget == target)
        {
            cinemachineTargetGroup.RemoveMember(CurrentTarget.transform);
            CurrentTarget = null;
        }

        //Using the target passed in tidy up list and event references.
        target.OnDestroyed -= RemoveTarget;
        targets.Remove(target);
    }
}
