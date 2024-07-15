using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;
using static UnityEngine.UI.GridLayoutGroup;

[RequireComponent(typeof(NavMeshAgent))]
public class NPCController : MonoBehaviour
{
    private NavMeshAgent agent;
    private Animator animator;
    [SerializeField] private Waypoints waypoints;
    private Vector2 Velocity;
    private Vector2 SmoothDeltaPosition;

    public Animator Animator => animator;
    public NavMeshAgent Agent => agent;
    public Waypoints Waypoints => waypoints;



    void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
        animator = GetComponent<Animator>();
    }
    // Start is called before the first frame update
    void Start()
    {
        agent.stoppingDistance = 0.1f;
        agent.updatePosition = false;
        animator.applyRootMotion = true;
        agent.updateRotation = false;
    }

    // Update is called once per frame
    void Update()
    {
        Debug.Log("IsMoving: " + animator.GetBool("IsMoving"));
        SynchronizeAnimatorAndAgent();
    }

    private void OnAnimatorMove()
    {
        // Update position
        Vector3 rootPosition = animator.rootPosition;
        rootPosition.y = agent.nextPosition.y;
        transform.position = rootPosition;
        agent.nextPosition = rootPosition;

        // Update rotation
        Quaternion rootRotation = animator.rootRotation;
        transform.rotation = rootRotation;
        agent.nextPosition = transform.position; // Ensure agent's next position is synchronized
    }

    private void OnDrawGizmos()
    {
        DisplayCorners();
    }

    private void SynchronizeAnimatorAndAgent()
    {
        Vector3 worldDeltaPosition = agent.nextPosition - transform.position;
        worldDeltaPosition.y = 0f;

        float dx = Vector3.Dot(transform.right, worldDeltaPosition);
        float dy = Vector3.Dot(transform.forward, worldDeltaPosition);

        Vector2 deltaPosition = new Vector2(dx, dy);

        float smooth = Mathf.Min(1, Time.deltaTime / 0.1f);
        SmoothDeltaPosition = Vector2.Lerp(SmoothDeltaPosition, deltaPosition, smooth);

        Velocity = SmoothDeltaPosition / Time.deltaTime;

        if (agent.remainingDistance <= agent.stoppingDistance)
        {
            Velocity = Vector2.Lerp(Vector2.zero, Velocity, agent.remainingDistance / agent.stoppingDistance);
        }

        bool shouldMove = Velocity.magnitude > 0.5f && agent.remainingDistance > agent.stoppingDistance;

        animator.SetFloat("Horizontal", Velocity.normalized.x);
        animator.SetFloat("Vertical", Velocity.normalized.y);

        float deltaMagnitude = worldDeltaPosition.magnitude;

        if (deltaMagnitude > agent.radius / 2f)
        {
            transform.position = Vector3.Lerp(animator.rootPosition, agent.nextPosition, smooth);
        }

        // Ensure agent's rotation matches the transform's rotation
        agent.transform.rotation = transform.rotation;
    }

    private void DisplayCorners()
    {
        Gizmos.color = Color.red;
        if (agent != null)
        {
            Gizmos.DrawLine(transform.position, agent.steeringTarget);
        }
    }
}
