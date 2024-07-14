using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Waypoints : MonoBehaviour
{
    [SerializeField] private GameObject[] waypointsList;
    [SerializeField] private float waypointRadius = 1.0f;

    public GameObject[] WaypointsList => waypointsList;

    private void OnDrawGizmos()
    {
        foreach (GameObject waypoint in waypointsList)
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(waypoint.transform.position, waypointRadius);
        }
    }
}
