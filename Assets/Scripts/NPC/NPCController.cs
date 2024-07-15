using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

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
        agent.updateRotation = true;
    }

    // Update is called once per frame
    void Update()
    {
        Debug.Log("IsMoving: " + animator.GetBool("IsMoving"));
        SynchronizeAnimatoraAndAgent();
    }

    private void OnAnimatorMove()
    {
        Vector3 rootPosition = animator.rootPosition;
        rootPosition.y = agent.nextPosition.y;
        transform.position = rootPosition;
        agent.nextPosition = rootPosition;
    }

    private void SynchronizeAnimatoraAndAgent()
    {
        //Distance between the agent next position and the Game Object position
        Vector3 worldDeltaPosition = agent.nextPosition - transform.position;
        worldDeltaPosition.y = 0f;

        float dx = Vector3.Dot(transform.right, worldDeltaPosition);
        float dy = Vector3.Dot(transform.forward, worldDeltaPosition);

        Vector2 deltaPosition = new Vector2(dx, dy);

        float smooth = Mathf.Min(1, Time.deltaTime / 0.1f);
        SmoothDeltaPosition = Vector2.Lerp(SmoothDeltaPosition, deltaPosition, smooth);

        //Calculate the velocity
        Velocity = SmoothDeltaPosition / Time.deltaTime;

        if (agent.remainingDistance <= agent.stoppingDistance)
        {
            Velocity = Vector2.Lerp(Vector2.zero, Velocity, agent.remainingDistance / agent.stoppingDistance);
        }

        bool shouldMove = Velocity.magnitude > 0.5f && agent.remainingDistance > agent.stoppingDistance;

        animator.SetBool("IsMoving", shouldMove);
        animator.SetFloat("Velocity", Velocity.magnitude);

        float deltaMagnitude = worldDeltaPosition.magnitude;

        if (deltaMagnitude > agent.radius / 2f)
        {
            transform.position = Vector3.Lerp(animator.rootPosition, agent.nextPosition, smooth);
        }
    }
}
