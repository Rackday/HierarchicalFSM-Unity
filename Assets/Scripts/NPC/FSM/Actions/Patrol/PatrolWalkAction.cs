using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;

[CreateAssetMenu(menuName = "Finite State Machine/Action/Patrol/WalkAction")]
public class PatrolWalkAction : Action
{
    [SerializeField] private float waitTime; // Wait time at waypoints
    [SerializeField] private float distOffSet; // Distance offset to the waypoint
    [SerializeField] private bool isWaiting = false; // Flag to check if the agent is currently waiting
    [SerializeField] private bool isTurning = false; // Flag to check if the agent is currently turning
    [SerializeField] private float turnThreshold = 75f; // Angle threshold for turning

    // Execute action
    public override void Execute(FSM fsm)
    {
        NavMeshAgent agent = fsm.Controller.Agent;
        Waypoints waypoints = fsm.Controller.Waypoints;
        Animator animator = fsm.Controller.Animator;

        Debug.Log("TurnLeft: " + animator.GetBool("LeftTurn90"));
        Debug.Log("TurnRight: " + animator.GetBool("RightTurn90"));

        if (agent == null || waypoints == null || animator == null)
        {
            return; // If essential components are missing, return
        }

        if (!isWaiting && !isTurning)
        {
            animator.SetBool("IsMoving", true); // Sets the agent animation to true
            agent.isStopped = false; // Agent starts moving

            // Checks the remaining distance
            if (agent.remainingDistance <= distOffSet)
            {
                // Start waiting at the waypoint
                fsm.StartCoroutine(WaitAndTurn(fsm)); // Agent waits at the waypoint and then turns if needed
            }
        }
    }

    private IEnumerator WaitAndTurn(FSM fsm)
    {
        isWaiting = true;
        fsm.Controller.Agent.isStopped = true; // Agent stops moving
        fsm.Controller.Animator.SetBool("IsMoving", false); // Sets the agent movement to false while waiting at the waypoint
        yield return new WaitForSeconds(waitTime);

        isWaiting = false;
        CheckForTurn(fsm); // Check for turn after waiting
    }

    private void CheckForTurn(FSM fsm)
    {
        Vector3 directionToTarget = fsm.Controller.Agent.steeringTarget - fsm.transform.position;
        directionToTarget.y = 0;
        float angle = Vector3.Angle(fsm.transform.forward, directionToTarget);

        if (angle > turnThreshold)
        {
            float signedAngle = Vector3.SignedAngle(fsm.transform.forward, directionToTarget, Vector3.up);
            bool isClockwise = signedAngle < 0;

            fsm.StartCoroutine(PerformTurn(fsm, directionToTarget, isClockwise));
        }
        else
        {
            // If no turn is needed, continue to the next waypoint
            SetNextWaypoint(fsm);
        }
    }

    private IEnumerator PerformTurn(FSM fsm, Vector3 directionToTarget, bool isClockwise)
    {
        isTurning = true;
        fsm.Controller.Agent.isStopped = true;

        // Calculate the target rotation
        Quaternion targetRotation = Quaternion.LookRotation(directionToTarget);

        // Play turn animation
        fsm.Controller.Animator.SetTrigger(isClockwise ? "RightTurn90" : "LeftTurn90");

        // Smoothly rotate towards the target direction
        while (Quaternion.Angle(fsm.transform.rotation, targetRotation) > 0.5f)
        {
            fsm.transform.rotation = Quaternion.RotateTowards(fsm.transform.rotation, targetRotation, 360 * Time.deltaTime);
            yield return null;
        }

        // Reset trigger and resume movement
        fsm.Controller.Animator.ResetTrigger("TurnRight90");
        fsm.Controller.Animator.ResetTrigger("TurnLeft90");
        isTurning = false;

        // After turning, proceed to the next waypoint
        SetNextWaypoint(fsm);
    }

    private void SetNextWaypoint(FSM fsm)
    {
        NavMeshAgent agent = fsm.Controller.Agent;
        Waypoints waypoints = fsm.Controller.Waypoints;

        if (agent == null || waypoints == null)
        {
            return; // If essential components are missing, return
        }

        // Generate a random index
        int randomIndex = Random.Range(0, waypoints.WaypointsList.Length);

        // Set agent destination to the random position
        Vector3 randomPos = waypoints.WaypointsList[randomIndex].transform.position;
        agent.destination = randomPos;

        // Allow agent to move again
        agent.isStopped = false;
        fsm.Controller.Animator.SetBool("IsMoving", true);
    }
}