using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
using StarterAssets;
using UnityEngine.InputSystem;
using Unity.Mathematics;

public class Aim_For_Bow : MonoBehaviour
{
    [SerializeField] private float normalSensitivity;
    [SerializeField] private float aimSensitivity;
    [SerializeField] private LayerMask aimColliderLayerMask;
    [SerializeField] private Transform debugTransform;
    [SerializeField] private Transform pfBulletProjectTile;
    [SerializeField] private Transform spawnBulletPosition;
    [SerializeField] private Transform vfxHitGreen;
    [SerializeField] private Transform vfxHitRed;

    private Animator animator;
    private PlayerStatsController playerStatsController;

    private void Awake()
    {
        animator = GetComponent<Animator>();
        playerStatsController = GetComponent<PlayerStatsController>();
    }

    private void Update()
    {
        Vector3 mouseWorldPosition = GetMouseWorldPosition();
        HandleAiming(mouseWorldPosition);
        HandleShooting(mouseWorldPosition);
    }

    private Vector3 GetMouseWorldPosition()
    {
        Vector2 screenCenterPoint = new Vector2(Screen.width / 2f, Screen.height / 2f);
        Ray ray = Camera.main.ScreenPointToRay(screenCenterPoint);
        Vector3 worldPosition = Vector3.zero;

        if (Physics.Raycast(ray, out RaycastHit raycastHit, 999f, aimColliderLayerMask))
        {
            debugTransform.position = raycastHit.point;
            worldPosition = raycastHit.point;
        }

        return worldPosition;
    }

    private void HandleAiming(Vector3 mouseWorldPosition)
    {
        if (Input.GetKey(KeyCode.Mouse1))
        {
            animator.SetLayerWeight(1, Mathf.Lerp(animator.GetLayerWeight(1), 1f, Time.deltaTime * 10f));

            Vector3 worldAimTarget = mouseWorldPosition;
            worldAimTarget.y = transform.position.y;
            Vector3 aimDirection = (worldAimTarget - transform.position).normalized;

            transform.forward = Vector3.Lerp(transform.forward, aimDirection, Time.deltaTime * 20f);
        }
        else
        {
            animator.SetLayerWeight(1, Mathf.Lerp(animator.GetLayerWeight(1), 0f, Time.deltaTime * 10f));
        }
    }

    private void HandleShooting(Vector3 mouseWorldPosition)
    {
        if (Input.GetKeyDown(KeyCode.Mouse0))
        {
            Vector3 aimDir = (mouseWorldPosition - spawnBulletPosition.position).normalized;
            Instantiate(pfBulletProjectTile, spawnBulletPosition.position, Quaternion.LookRotation(aimDir, Vector3.up));
        }
    }

    private void SetActiveFalse()
    {
        Transform childTransform = gameObject.transform.GetChild(0);
        childTransform.gameObject.SetActive(false);
    }
}
