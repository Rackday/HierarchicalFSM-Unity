using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;

[CreateAssetMenu(menuName = "Finite State Machine/Action/Patrol/WalkAction")]
public class PatrolWalkAction : Action
{
    [SerializeField] private float waitTime = 2f; // Wait time at each waypoint
    [SerializeField] private float distOffSet = 0.1f; // Distance offset to determine when the agent has reached the waypoint

    [SerializeField] private bool isWaiting = false; // Flag to check if the agent is currently waiting

    public override void Execute(FSM fsm)
    {
        // Required components for the action
        NavMeshAgent agent = fsm.Controller.Agent;
        Waypoints waypoints = fsm.Controller.Waypoints;
        Animator animator = fsm.Controller.Animator;

        if (agent == null || waypoints == null)
        {
            return; // If essential components are missing, exit
        }

        if (!isWaiting)
        {
            animator.SetBool("IsMoving", true); // Sets the agent animation to true
            fsm.Controller.Agent.isStopped = false; // Agent starts moving

            // Check the remaining distance to the destination
            if (!agent.pathPending && agent.remainingDistance <= distOffSet)
            {
                // Generates a random index
                int randomIndex = Random.Range(0, waypoints.WaypointsList.Length);

                // Sets agent destination to the random position
                Vector3 randomPos = waypoints.WaypointsList[randomIndex].transform.position;
                agent.SetDestination(randomPos);

                Debug.Log($"Moving to waypoint: {randomPos}");

                // Start the Coroutine to wait at the waypoint
                fsm.StartCoroutine(WaitInWaypoint(fsm));
            }
        }
    }

    private IEnumerator WaitInWaypoint(FSM fsm)
    {
        // Set the agent to stop moving and update the animation
        isWaiting = true;
        fsm.Controller.Agent.isStopped = true;
        fsm.Controller.Animator.SetBool("IsMoving", false);

        Debug.Log("Waiting at waypoint");

        // Wait for the specified time
        yield return new WaitForSeconds(waitTime);

        // After waiting, set the agent to move again
        isWaiting = false;
    }
}

