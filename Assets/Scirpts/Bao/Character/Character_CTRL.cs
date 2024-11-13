using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using Unity.Netcode;
using UnityEngine;

public class Character_CTRL : NetworkBehaviour
{
    Animator animator;
    Rigidbody rb;
    Camera mainCamera;

    public CinemachineFreeLook freeLookCamera;

    public float runSpeed = 2f;
    public float walkSpeed = 1f;
    public float rotationSpeed = 10f;
    public float jumpForce = 5f;

    NetworkVariable<float> velocity = new NetworkVariable<float>(writePerm: NetworkVariableWritePermission.Server);
    NetworkVariable<bool> isJumping = new NetworkVariable<bool>(writePerm: NetworkVariableWritePermission.Server);
    NetworkVariable<bool> isFalling = new NetworkVariable<bool>(writePerm: NetworkVariableWritePermission.Server);
    NetworkVariable<bool> isLanding = new NetworkVariable<bool>(writePerm: NetworkVariableWritePermission.Server);

    bool isGrounded = true;

    void Start()
    {
        animator = GetComponent<Animator>();
        rb = GetComponent<Rigidbody>();
        mainCamera = Camera.main;

        if (mainCamera == null)
        {
            Debug.LogError("Main camera not found!");
        }
        if (IsOwner)
        {
            freeLookCamera.gameObject.SetActive(true);
        }
        else
        {
            freeLookCamera.gameObject.SetActive(false);
        }

        velocity.OnValueChanged += (oldValue, newValue) => animator.SetFloat("Velocity", newValue);
        isJumping.OnValueChanged += (oldValue, newValue) => animator.SetBool("isJumping", newValue);
        isFalling.OnValueChanged += (oldValue, newValue) => animator.SetBool("isFalling", newValue);
        isLanding.OnValueChanged += (oldValue, newValue) => animator.SetBool("isLanding", newValue);
    }

    void Update()
    {
        if (!IsOwner) return;

        float horizontalInput = Input.GetAxis("Horizontal");
        float verticalInput = Input.GetAxis("Vertical");
        bool runPressed = Input.GetKey(KeyCode.LeftShift);

        if ((verticalInput != 0 || horizontalInput != 0))
        {
            if (runPressed)
            {
                UpdateVelocityServerRpc(runSpeed);
            }
            else
            {
                UpdateVelocityServerRpc(walkSpeed);
            }
        }
        else
        {
            UpdateVelocityServerRpc(0f);
        }

        if (Input.GetKeyDown(KeyCode.Space) && isGrounded)
        {
            UpdateJumpingStateServerRpc(true);
            JumpServerRpc();
            isGrounded = false;
            UpdateFallingStateServerRpc(false);
            UpdateLandingStateServerRpc(false);
        }

        if (!isGrounded && rb.velocity.y < 0)
        {
            UpdateFallingStateServerRpc(true);
            UpdateJumpingStateServerRpc(false);
        }

        if (isGrounded && rb.velocity.y == 0)
        {
            UpdateLandingStateServerRpc(true);
            UpdateFallingStateServerRpc(false);
        }

        MoveServerRpc(horizontalInput, verticalInput, runPressed);
    }

    void FixedUpdate()
    {
        if (!IsOwner) return;

        Vector3 forward = mainCamera.transform.forward;
        Vector3 right = mainCamera.transform.right;
        forward.y = 0;
        right.y = 0;
        forward.Normalize();
        right.Normalize();

        Vector3 movement = (forward * Input.GetAxis("Vertical") + right * Input.GetAxis("Horizontal")).normalized * velocity.Value * Time.fixedDeltaTime;

        if (movement != Vector3.zero)
        {
            Quaternion targetRotation = Quaternion.LookRotation(movement);
            rb.MoveRotation(Quaternion.Slerp(rb.rotation, targetRotation, rotationSpeed * Time.fixedDeltaTime));
        }
    }

    [ServerRpc]
    void MoveServerRpc(float horizontalInput, float verticalInput, bool runPressed)
    {
        Vector3 forward = Camera.main.transform.forward;
        Vector3 right = Camera.main.transform.right;
        forward.y = 0;
        right.y = 0;
        forward.Normalize();
        right.Normalize();

        Vector3 movement = (forward * verticalInput + right * horizontalInput).normalized * (runPressed ? runSpeed : walkSpeed) * Time.fixedDeltaTime;

        if (movement != Vector3.zero)
        {
            Quaternion targetRotation = Quaternion.LookRotation(movement);
            rb.MoveRotation(Quaternion.Slerp(rb.rotation, targetRotation, rotationSpeed * Time.fixedDeltaTime));
        }
    }

    [ServerRpc]
    void JumpServerRpc()
    {
        rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
    }

    [ServerRpc]
    void UpdateVelocityServerRpc(float newVelocity)
    {
        velocity.Value = newVelocity;
    }

    [ServerRpc]
    void UpdateJumpingStateServerRpc(bool newState)
    {
        isJumping.Value = newState;
    }

    [ServerRpc]
    void UpdateFallingStateServerRpc(bool newState)
    {
        isFalling.Value = newState;
    }

    [ServerRpc]
    void UpdateLandingStateServerRpc(bool newState)
    {
        isLanding.Value = newState;
    }

    void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.CompareTag("Ground"))
        {
            isGrounded = true;
            UpdateLandingStateServerRpc(true);
            UpdateJumpingStateServerRpc(false);
            UpdateFallingStateServerRpc(false);
        }
    }
}
