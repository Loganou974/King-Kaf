using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
   
    public float moveSpeed = 1;
    public float jumpForce = 3;
    Rigidbody rb;

    public bool canMove = true;
    public bool canJump = true;
    

    public LayerMask groundMask;
    public bool debugIsGrounded;


    [SerializeField]
    bool isGrounded;


    void CheckGround()
    {
        if (debugIsGrounded)
        {
            Debug.DrawRay(transform.position, - Vector3.up * 1, Color.red, 1);
        }

        if (Physics.Raycast(transform.position, -Vector3.up, 1, groundMask))
        {
            if (debugIsGrounded) Debug.Log("isGrounded");
            isGrounded = true;
        }
        else
        {
            Debug.Log("isOnAir");
            isGrounded = false;
        }
          
    }

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
    }

    private void Update()
    {
        CheckGround();
    }


    private void FixedUpdate()
    {

        //CHeck move input
        float horizontal = Input.GetAxis(GlobalManager.GameInput.horizontal);

        if(horizontal != 0)
        {
            if(canMove) Move(horizontal);
        }

        //Check Jump
        if (Input.GetButtonDown(GlobalManager.GameInput.Jump))
        {
            Debug.Log("Button Pressed");

            if (canJump & isGrounded)
            {
                Debug.Log("CanJump and isGrounded");
                Jump();
            }
        }
        

       
    }

    private void Jump()
    {
        rb.velocity = new Vector3(rb.velocity.x, 0, rb.velocity.z);
        rb.AddForce(new Vector3(0, jumpForce, 0), ForceMode.VelocityChange);
        Debug.Log("Jump");
    }

    private void Move(float direction)
    {
        rb.position += new Vector3(direction * moveSpeed, 0,0);
    }
}
