using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class PatrolAction : Action
{
    public override void Execute(FSM fsm)
    {
        NavMeshAgent agent = fsm.Controller.Agent;
        Waypoints waypoints = fsm.Controller.Waypoints;

        if (agent != null && waypoints != null)
        {
            agent.destination = waypoints.WaypointsList[0].transform.position;
        }
    }

}
