using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    private Rigidbody rb;
    private Vector2 velocity;
    private Animator animator;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
        animator = GetComponent<Animator>();
    }

    // Start is called before the first frame update
    void Start()
    {
            
    }

    // Update is called once per frame
    void Update()
    {
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
        Debug.Log("IsRunning: " + IsRunning());
        Debug.Log("Horizontal: " + velocity.x);
        Debug.Log("Vertical: " + velocity.y);
    }

    private bool IsRunning() => Input.GetKey(KeyCode.LeftShift) ? true : false;

}
