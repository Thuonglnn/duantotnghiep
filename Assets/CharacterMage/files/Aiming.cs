using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
using StarterAssets;
using UnityEngine.InputSystem;
using Unity.Mathematics;
using Unity.Netcode;

public class Aiming : NetworkBehaviour
{
    [SerializeField] private float normalSensitivity;
    [SerializeField] private float aimSensitivity;
    [SerializeField] private LayerMask aimColliderLayerMask = new LayerMask();
    [SerializeField] private Transform debugTransform;
    [SerializeField] private GameObject pfBulletProjectTile;  // Sử dụng GameObject thay vì NetworkObject
    [SerializeField] private GameObject pfSkillRProjectTile;
    [SerializeField] private GameObject pfSkillQProjectTile;
    [SerializeField] private Transform spawnBulletPosition;
    [SerializeField] private Transform vfxHitGreen;
    [SerializeField] private Transform vfxHitRed;

    Vector2 input;

    private Animator animator;
    private PlayerStatsController playerStatsController;

    private void Awake()
    {
        animator = GetComponent<Animator>();
        playerStatsController = GetComponent<PlayerStatsController>();
        // Cursor.lockState = CursorLockMode.Locked;
        // Cursor.visible = false;
    }

    private void Update()
    {
        if (!IsOwner) return;

        Vector3 mouseWorldPosition = Vector3.zero;
        Vector2 screenCenterPoint = new Vector2(Screen.width / 2f, Screen.height / 2f);
        Ray ray = Camera.main.ScreenPointToRay(screenCenterPoint);
        Transform hitTransform = null;

        if (Physics.Raycast(ray, out RaycastHit raycastHit, 999f, aimColliderLayerMask))
        {
            debugTransform.position = raycastHit.point;
            mouseWorldPosition = raycastHit.point;
            hitTransform = raycastHit.transform;
        }

        
        if (Input.GetMouseButton(1))
        {
            animator.SetLayerWeight(1, Mathf.Lerp(animator.GetLayerWeight(1), 1f, Time.deltaTime * 10f));
            Vector3 worldAimTarget = mouseWorldPosition;
            worldAimTarget.y = transform.position.y;
            Vector3 aimDirection = (worldAimTarget - transform.position).normalized;
            transform.forward = Vector3.Lerp(transform.forward, aimDirection, Time.deltaTime * 20f);
            animator.SetLayerWeight(2, 1);
            animator.SetBool("isAming", true);

            input.x = Input.GetAxis("Horizontal");
            input.y = Input.GetAxis("Vertical");
            animator.SetFloat("InputX", input.x);
            animator.SetFloat("InputY", input.y);
        }
        else
        {
            animator.SetLayerWeight(1, Mathf.Lerp(animator.GetLayerWeight(1), 0f, Time.deltaTime * 10f));
            animator.SetLayerWeight(2, 0);
            animator.SetBool("isAming", false);
        }

        if (Input.GetKeyDown(KeyCode.Mouse0))
        {
            //animator.SetLayerWeight(1, Mathf.Lerp(animator.GetLayerWeight(1), 1f, Time.deltaTime * 10f));
            ShootServerRpc(mouseWorldPosition);
            
        }

        if (Input.GetKeyUp(KeyCode.Q))
        {
            if (!playerStatsController.IsSkillOnCooldown(PlayerStatsController.Skill.Q))
            {
                animator.SetTrigger("SkillQ");
                UseSkillQServerRpc(mouseWorldPosition);
                playerStatsController.UseSkill(PlayerStatsController.Skill.Q, 15);
            }
        }
        if (Input.GetKey(KeyCode.E))
        {
            if (!playerStatsController.IsSkillOnCooldown(PlayerStatsController.Skill.E))
            {
                animator.SetTrigger("SkillE");
                ActivateSkillEServerRpc();
                playerStatsController.UseSkill(PlayerStatsController.Skill.E, 10);
            }
        }
        if (Input.GetKeyUp(KeyCode.R))
        {
            if (!playerStatsController.IsSkillOnCooldown(PlayerStatsController.Skill.R))
            {
                animator.SetTrigger("SkillR");
                UseSkillRServerRpc(mouseWorldPosition);
                playerStatsController.UseSkill(PlayerStatsController.Skill.R, 30);
            }
        }
        if (Input.GetKeyUp(KeyCode.Escape))
        {
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = true;
        }
    }

    [ServerRpc]
    private void ShootServerRpc(Vector3 mouseWorldPosition)
    {
        Vector3 aimDir = (mouseWorldPosition - spawnBulletPosition.position).normalized;
        GameObject bullet = Instantiate(pfBulletProjectTile, spawnBulletPosition.position, Quaternion.LookRotation(aimDir, Vector3.up));
        bullet.GetComponent<NetworkObject>().Spawn();
    }

    [ServerRpc]
    private void UseSkillQServerRpc(Vector3 mouseWorldPosition)
    {
        Vector3 aimDir = (mouseWorldPosition - spawnBulletPosition.position).normalized;
        GameObject skillQ = Instantiate(pfSkillQProjectTile, spawnBulletPosition.position, Quaternion.LookRotation(aimDir, Vector3.up));
        skillQ.GetComponent<NetworkObject>().Spawn();
    }

    [ServerRpc]
    private void UseSkillRServerRpc(Vector3 mouseWorldPosition)
    {
        Vector3 aimDir = mouseWorldPosition.normalized;
        GameObject skillR = Instantiate(pfSkillRProjectTile, mouseWorldPosition, Quaternion.LookRotation(aimDir, Vector3.up));
        skillR.GetComponent<NetworkObject>().Spawn();
    }

    [ServerRpc]
    private void ActivateSkillEServerRpc()
    {
        ActivateSkillEClientRpc();
    }

    [ClientRpc]
    private void ActivateSkillEClientRpc()
    {
        Transform childTransform = gameObject.transform.GetChild(0);
        childTransform.gameObject.SetActive(true);
        Invoke("SetActiveFalse", 4.0f);
    }

    void SetActiveFalse()
    {
        Transform childTransform = gameObject.transform.GetChild(0);
        childTransform.gameObject.SetActive(false);
    }
}
