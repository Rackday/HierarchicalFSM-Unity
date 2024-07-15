using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;

[CreateAssetMenu(menuName = "Finite State Machine/Action/Patrol/WalkAction")]
public class PatrolWalkAction : Action
{
    //Wait time
    [SerializeField] private float waitTime;

    //Distance offset
    [SerializeField] private float distOffSet;

    [SerializeField] private bool isWaiting = false; // Flag to check if the agent is currently waiting

    //Execute action
    public override void Execute(FSM fsm)
    {
        //Required components for the action
        NavMeshAgent agent = fsm.Controller.Agent;
        Waypoints waypoints = fsm.Controller.Waypoints;
        Animator animator = fsm.Controller.Animator;

        animator.SetBool("IsMoving", true); //Sets the agent animation to true
        fsm.Controller.Agent.isStopped = false; //Agent starts moving

        if (agent == null || waypoints == null || animator == null)
        {
            return; // If essential components are missing, returns
        }

        if (!isWaiting)
        {
            //Checks the remaing distance
            if (agent.remainingDistance <= distOffSet)
            {
                //Generates a random index
                int randomIndex = Random.Range(0, waypoints.WaypointsList.Length);

                //Sets agent destination to the random position
                Vector3 randomPos = waypoints.WaypointsList[randomIndex].transform.position;
                agent.destination = randomPos;

                //Runs the Coroutine
                fsm.StartCoroutine(WaitInWaypoint(fsm)); //Agent waits at the waypoint
            }
        }

    }

    private IEnumerator WaitInWaypoint(FSM fsm)
    {
        // Set the agent to stop moving and update the animation
        isWaiting = true;

        fsm.Controller.Agent.isStopped = true; //Agent stops moving
        fsm.Controller.Animator.SetBool("IsMoving", false); //Sets the agent movement to false while is wayting at the waypoint
        Debug.Log("Waiting at waypoint");
        yield return new WaitForSeconds(waitTime);

        // After waiting, set the agent to move again
        isWaiting = false;
    }

}
