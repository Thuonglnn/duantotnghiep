using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using UnityEngine;

public class Bow_CTRL : MonoBehaviour
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

    public static bool isAiming;

    public CinemachineFreeLook freeLookCamera;
    public Transform LookAt;
    public Transform Follow;

    public Transform LookAt1;
    public Transform Follow1;

    void Start()
    {
        animator = GetComponent<Animator>();

        VelocityHash = Animator.StringToHash("Velocity");
        rb = GetComponent<Rigidbody>();

        //virtualCamera.GetComponent<CinemachineFreeLook>();
        mainCamera = Camera.main;
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

        if(Input.GetMouseButtonDown(1))
        {
            isAiming = true;
        }
        if(Input.GetMouseButtonUp(1))
        {
            isAiming = false;
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

        if (isAiming)
        {
            freeLookCamera.m_LookAt = LookAt; 
            freeLookCamera.m_Follow = Follow; 

            freeLookCamera.GetComponent<CinemachineCameraOffset>().m_Offset.x = 0.4f;
            freeLookCamera.GetComponent<CinemachineCameraOffset>().m_Offset.y = 0.4f;
            freeLookCamera.GetComponent<CinemachineCameraOffset>().m_Offset.z = 0.4f;

            //freeLookCamera.m_XAxis.m_MaxSpeed = 0f;
            //freeLookCamera.m_YAxis.m_MaxSpeed = 0f;
            
            Quaternion aimRotation = Quaternion.Euler(0, 90, 0) * Quaternion.LookRotation(forward);
            rb.MoveRotation(Quaternion.Slerp(rb.rotation, aimRotation, rotationSpeed * Time.fixedDeltaTime));
            
        }
        else
        {
            freeLookCamera.m_LookAt = LookAt1; 
            freeLookCamera.m_Follow = Follow1; 

            freeLookCamera.GetComponent<CinemachineCameraOffset>().m_Offset.x = 0f;
            freeLookCamera.GetComponent<CinemachineCameraOffset>().m_Offset.y = 0f;
            freeLookCamera.GetComponent<CinemachineCameraOffset>().m_Offset.z = 0f;

            freeLookCamera.m_XAxis.m_MaxSpeed = 300f; 
            freeLookCamera.m_YAxis.m_MaxSpeed = 2f;
        }
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

    public static Bow_CTRL instance;

    void Awake()
    {
        instance = this;
    }


   
}

