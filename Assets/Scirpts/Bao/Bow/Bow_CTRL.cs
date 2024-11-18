using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using Unity.Netcode;
using UnityEngine;

public class Bow_CTRL : NetworkBehaviour
{
    Animator animator;
    Rigidbody rb;
    Camera mainCamera;
    public float runSpeed = 2f;
    public float walkSpeed = 1f; 
    public float rotationSpeed = 10f;
    public float jumpForce = 5f;
    float velocity = 0.0f;
    int VelocityHash;
    bool isGrounded = true;

    public bool isAiming;

    public CinemachineFreeLook freeLookCamera;

    // nhìn vao đâu khi ngắm
    public Transform LookAt;
    public Transform Follow;
    // nhìn vào đâu khi hết ngắm 
    public Transform LookAt1;
    public Transform Follow1;

    // giới hạn ngắm 
    public float minAimAngle = -30f;  
    public float maxAimAngle = 60f; 
    // giới hạn camera
    public float minAimYValue = 0.4f;   // Giới hạn nhìn xuống (ví dụ nhìn vào phần bụng)
    public float maxAimYValue = 0.6f; 
    Vector2 input;

    void Start()
    {
        animator = GetComponent<Animator>();
        VelocityHash = Animator.StringToHash("Velocity");
        rb = GetComponent<Rigidbody>();
        mainCamera = Camera.main;
        if (IsOwner)
        {
            freeLookCamera.gameObject.SetActive(true);
        }
        else
        {
            freeLookCamera.gameObject.SetActive(false);
        }
    }

    void Update()
    {
        if (!IsOwner) return; // Chỉ chạy mã nếu là client sở hữu

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
            JumpServerRpc(); // Gọi hàm nhảy qua server
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
            UpdateAimingStateServerRpc(isAiming); // Cập nhật trạng thái ngắm qua server
        }
        if(Input.GetMouseButtonUp(1))
        {
            isAiming = false;
            UpdateAimingStateServerRpc(isAiming); // Cập nhật trạng thái ngắm qua server
        }
    }

    void FixedUpdate()
    {
        if (!IsOwner) return; // Chỉ chạy mã nếu là client sở hữu

        Vector3 forward = mainCamera.transform.forward;
        Vector3 right = mainCamera.transform.right;

        forward.y = 0;
        right.y = 0;

        forward.Normalize();
        right.Normalize();

        // Tính toán hướng di chuyển trong XZ dựa trên hướng camera
        Vector3 movement = (forward * Input.GetAxis("Vertical") + right * Input.GetAxis("Horizontal")).normalized * velocity * Time.fixedDeltaTime;

        if (isAiming)
        {
            animator.SetBool("isAming", true);

            freeLookCamera.m_LookAt = LookAt; 
            freeLookCamera.m_Follow = Follow; 

            freeLookCamera.GetComponent<CinemachineCameraOffset>().m_Offset.x = 2f;
            freeLookCamera.GetComponent<CinemachineCameraOffset>().m_Offset.y = 0.4f;
            freeLookCamera.GetComponent<CinemachineCameraOffset>().m_Offset.z = 0.4f;

            input.x = Input.GetAxis("Horizontal");
            input.y = Input.GetAxis("Vertical");

            animator.SetFloat("InputX", input.x);
            animator.SetFloat("InputY", input.y);

            Quaternion aimRotation = Quaternion.Euler(0, mainCamera.transform.eulerAngles.y, 0);
            rb.MoveRotation(Quaternion.Slerp(rb.rotation, aimRotation, rotationSpeed * Time.fixedDeltaTime));

        }
        else
        {
            animator.SetBool("isAming",false);
            if (movement != Vector3.zero)
            {
                // Tính toán hướng quay mục tiêu trong  XZ
                Quaternion targetRotation = Quaternion.LookRotation(movement);
                // Xoay nhân vật quanh trục Y
                rb.MoveRotation(Quaternion.Slerp(rb.rotation, targetRotation, rotationSpeed * Time.fixedDeltaTime));
            }

            freeLookCamera.m_LookAt = LookAt1; 
            freeLookCamera.m_Follow = Follow1; 

            freeLookCamera.GetComponent<CinemachineCameraOffset>().m_Offset.x = 1.5f;
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
        }
    }

    [ServerRpc]
    void JumpServerRpc()
    {
        JumpClientRpc();
    }

    [ClientRpc]
    void JumpClientRpc()
    {
        if (IsOwner)
        {
            animator.SetBool("isJumping", true); 
            rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
            isGrounded = false;
            animator.SetBool("isFalling", false);  
            animator.SetBool("isLanding", false); 
        }
    }

    [ServerRpc]
    void UpdateAimingStateServerRpc(bool isAiming)
    {
        UpdateAimingStateClientRpc(isAiming);
    }

    [ClientRpc]
    void UpdateAimingStateClientRpc(bool isAiming)
    {
        this.isAiming = isAiming;
    }
}
