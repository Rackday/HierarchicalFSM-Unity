using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

[CreateAssetMenu(menuName = "Finite State Machine/Action/Patrol/EntryAction")]
public class PatrolEntryAction : Action
{
    //This action will set a initial destination to the agent
    //This action only runs once, since is an entry action on the Patrol State
    private Vector3 startPosition;

    //Execute Action
    public override void Execute(FSM fsm)
    {
        NavMeshAgent agent = fsm.Controller.Agent;
        Waypoints waypoints = fsm.Controller.Waypoints;

        //Generates a random index
        int randomIndex = Random.Range(0, waypoints.WaypointsList.Length);

        //Sets agent destination to the random position
        startPosition = waypoints.WaypointsList[randomIndex].transform.position;
        agent.destination = startPosition;
    }
}
