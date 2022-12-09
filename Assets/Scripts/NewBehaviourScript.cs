using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NewBehaviourScript : MonoBehaviour
{
    Rigidbody rb;
    Animator anim;
    public GameObject camera;
    private string currentState;
    const string walk_anim = "Walk";
    const string idle_anim = "Idle";

    [Header("Movement")]
    public float moveSpeed;
    public Transform orientation;

    public float groundDrag;

    [Header("Ground check")]
    public float playerHeigh;
    public LayerMask whatIsGround;
    bool grounded;

    float horizontalInput;
    float verticalInput;

    Vector3 moveDirection;
    void Start()
    {
        anim = GetComponent<Animator>();
        rb = GetComponent<Rigidbody>();
        rb.freezeRotation = true;
    }

    private void FixedUpdate()
    {
        Moveplayer();
    }

    void Update()
    {
        Myinput();
        

        if (Input.GetKey("left"))
        {
            transform.Rotate(0, -75 * Time.deltaTime,0);
            ChangeAnimationState(idle_anim);
        }
        else
        {
            if (Input.GetKey("right"))
            {
                transform.Rotate(0, 75 * Time.deltaTime, 0);
                ChangeAnimationState(idle_anim);

            }
            else
            {
                if (Input.GetKey("up"))
                {
                    rb.AddForce(-transform.forward * 150f, ForceMode.Impulse);
                    ChangeAnimationState(walk_anim);
                }
                else
                {
                    ChangeAnimationState(idle_anim);
                }
                
            }
        }
        grounded = Physics.Raycast(transform.position, Vector3.down, playerHeigh * 0.5f + 0.2f, whatIsGround);

        if (grounded)
        {
            rb.drag = groundDrag;
        }
        else
        {
            rb.drag = 0;
        }



    }

    public void ChangeAnimationState(string newAnim)
    {
        if (currentState != newAnim)
        {
            anim.Play(newAnim);
            currentState = newAnim; 
        }

    }


    private void Myinput()
    {
        horizontalInput = Input.GetAxisRaw("Horizontal");
        verticalInput = Input.GetAxisRaw("Vertical");
    }

    private void Moveplayer()
    {
        moveDirection = orientation.forward * verticalInput + orientation.right * horizontalInput;

        rb.AddForce(moveDirection.normalized * moveSpeed * 10f, ForceMode.Force);
    }

    private void SpeedControl()
    {
        Vector3 flatVel = new Vector3(rb.velocity.x, 0f, rb.velocity.z);


        if(flatVel.magnitude > moveSpeed)
        {
            Vector3 limitedVel = flatVel.normalized * moveSpeed;
            rb.velocity = new Vector3(limitedVel.x, rb.velocity.y, limitedVel.z);
        }
    }
}
