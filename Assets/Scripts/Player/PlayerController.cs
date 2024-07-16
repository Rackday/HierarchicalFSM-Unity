using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    private Rigidbody rb;
    private Vector2 velocity;
    private Animator animator;

    [SerializeField] private bool crouch = false;
    [SerializeField] private bool canCrouchCover = false;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
        animator = GetComponent<Animator>();
    }

    // Start is called before the first frame update
    void Start()
    {
        animator.applyRootMotion = true;
    }

    // Update is called once per frame
    void Update()
    {
        IsCrouched();
        // Read input values once per frame
        float horizontal = Input.GetAxis("Horizontal");
        float vertical = Input.GetAxis("Vertical");

        // Determine velocity based on whether the player is running
        if (IsRunning())
        {
            velocity = new Vector2(horizontal, vertical) * 2f;
        }
        else
        {
            velocity = new Vector2(horizontal, vertical);
        }

        // Update animator parameters
        animator.SetFloat("Horizontal", velocity.x);
        animator.SetFloat("Vertical", velocity.y);

        // Debug logs
        //Debug.Log("IsRunning: " + IsRunning());
        //Debug.Log("Horizontal: " + velocity.x);
        //Debug.Log("Vertical: " + velocity.y);

        animator.SetBool("Crouched", crouch);
        animator.SetBool("CrouchCover", canCrouchCover);
    }

    //Checks if the player is running
    private bool IsRunning() => Input.GetKey(KeyCode.LeftShift) ? true : false;


    private void IsCrouched()
    {
        if (Input.GetKeyDown(KeyCode.LeftControl))
        {
            crouch = !crouch;
        }
    }

    void OnCollisionStay(Collision collision)
    {
        if (Input.GetKeyDown(KeyCode.Q))
        {
            Debug.Log("COLLISION!");
            foreach (ContactPoint contact in collision.contacts)
            {
                if (contact.otherCollider.tag == "SmallWall") canCrouchCover = !canCrouchCover;
                break;
            }
        }
    }

    private void OnAnimatorMove()
    {
        Vector3 pos;
        if(canCrouchCover)
        {
            pos = new Vector3(transform.position.x, transform.position.y, animator.rootPosition.z);
            transform.position = pos;
        }
        
        else
        {
            transform.position = animator.rootPosition;
            transform.rotation = animator.rootRotation;
        }
    }
}
