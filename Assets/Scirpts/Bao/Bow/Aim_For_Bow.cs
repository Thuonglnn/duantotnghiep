using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
using StarterAssets;
using UnityEngine.InputSystem;
using Unity.Mathematics;

public class Aim_For_Bow : MonoBehaviour
{
    [SerializeField] private LayerMask aimColliderLayerMask;
    [SerializeField] private Transform debugTransform;
    [SerializeField] private Transform pfBulletProjectTile;
    [SerializeField] private Transform spawnBulletPosition;
    private Animator animator;

    private void Awake()
    {
        animator = GetComponent<Animator>();
    }

    private void Update()
    {
        Vector3 mouseWorldPosition = GetMouseWorldPosition();
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

    private void HandleShooting(Vector3 mouseWorldPosition)
    {
        if (Input.GetKeyDown(KeyCode.Mouse0))
        {
            Vector3 aimDir = (mouseWorldPosition - spawnBulletPosition.position).normalized;
            Instantiate(pfBulletProjectTile, spawnBulletPosition.position, Quaternion.LookRotation(aimDir, Vector3.up));
        }
    }
}
