using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Warrior_CTRL : MonoBehaviour
{
    Animator animator;
    Rigidbody rb;
    Camera mainCamera;

    public float runSpeed = 2f;
    public float walkSpeed = 1f; 
    public float rotationSpeed = 10f;
    public float jumpForce =5f;
    float velocity = 0.0f;
    int VelocityHash;
    bool isGrounded = true;



    void Start()
    {
        animator = GetComponent<Animator>();
        VelocityHash = Animator.StringToHash("Velocity");
        rb = GetComponent<Rigidbody>();

        mainCamera = Camera.main;

        if (mainCamera == null)
        {
            Debug.LogError("Main camera not found!");
        }
    }

    void Update()
    {
        float horizontalInput = Input.GetAxis("Horizontal");
        float verticalInput = Input.GetAxis("Vertical");

       


        bool runPressed = Input.GetKey(KeyCode.LeftShift);
        if ((verticalInput != 0 || horizontalInput != 0 ))
        {
            if (runPressed)
            {
                velocity = runSpeed; 
            }
            else
            {
                velocity = walkSpeed; 
            }
        }
        else
        {
            velocity = 0f; 
        }

        animator.SetFloat(VelocityHash, velocity);

        if (Input.GetKeyDown(KeyCode.Space) && isGrounded)
        {
            animator.SetBool("isJumping", true); 
            rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
            isGrounded = false;
            
            animator.SetBool("isFalling", false);  
            animator.SetBool("isLanding", false); 
        }

        
        if (!isGrounded && rb.velocity.y < 0)
        {
            animator.SetBool("isFalling", true);   
            animator.SetBool("isJumping", false); 
        }

        
        if (isGrounded && rb.velocity.y == 0)
        {
            animator.SetBool("isLanding", true);  
            animator.SetBool("isFalling", false);
        }
    }

    void FixedUpdate()
    {
        Vector3 forward = mainCamera.transform.forward;
        Vector3 right = mainCamera.transform.right;

        forward.y = 0;
        right.y = 0;

        forward.Normalize();
        right.Normalize();

        // Tính toán hướng di chuyển trong XZ dựa trên hướng camera
        Vector3 movement = (forward * Input.GetAxis("Vertical") + right * Input.GetAxis("Horizontal")).normalized * velocity * Time.fixedDeltaTime;

        if (movement != Vector3.zero)
        {
            // Tính toán hướng quay mục tiêu trong  XZ
            Quaternion targetRotation = Quaternion.LookRotation(movement);

            // Xoay nhân vật quanh trục Y
            rb.MoveRotation(Quaternion.Slerp(rb.rotation, targetRotation, rotationSpeed * Time.fixedDeltaTime));
        }
        
        // // Di chuyển nhân vật
        // if(isGrounded)
        // {
        //     rb.MovePosition(rb.position + movement);
        // }
    }


    void OnCollisionEnter(Collision other)
    {
        if(other.gameObject.CompareTag("Ground"))
        {
            isGrounded = true;
            // animator.SetBool("isLanding", true); 
            // animator.SetBool("isJumping", false); 
            // animator.SetBool("isFalling", false);  
        }
        
        
    }

   
   
}

