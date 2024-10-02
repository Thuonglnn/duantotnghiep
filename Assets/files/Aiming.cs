using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
using StarterAssets;
using UnityEngine.InputSystem;
using Unity.Mathematics;


public class Aiming : MonoBehaviour
{
    [SerializeField] private CinemachineVirtualCamera aimVitualCam;
    [SerializeField] private float nomalSensitivity;
    [SerializeField] private float aimSensitivity;
    [SerializeField] private LayerMask aimColliderLayerMask = new LayerMask();
    [SerializeField] private Transform debugTransform;
    [SerializeField] private Transform pfBulletProjectTile;
    [SerializeField] private Transform pfSkillRProjectTile;
    [SerializeField] private Transform spawnBulletPosition;
    [SerializeField] private Transform vfxHitGreen;
    [SerializeField] private Transform vfxHitRed;

    private Animator animator;


    private ThirdPersonController thirdPersonController;
    private StarterAssetsInputs starterAssetsInputs;

    private void Awake()
    {
        thirdPersonController = GetComponent<ThirdPersonController>();
        starterAssetsInputs = GetComponent<StarterAssetsInputs>();
        animator = GetComponent<Animator>();
    }
    private void Update()
    {
        Vector3 MouseWorldPosition = Vector3.zero;


        Vector2 screenCenterPoint = new Vector2(Screen.width / 2f, Screen.height / 2f);
        Ray ray = Camera.main.ScreenPointToRay(screenCenterPoint);
        Transform hitTransform = null;

        if (Physics.Raycast(ray, out RaycastHit raycastHit, 999f, aimColliderLayerMask))
        {
            debugTransform.position = raycastHit.point;
            MouseWorldPosition = raycastHit.point;
            hitTransform = raycastHit.transform;
        }

        if (starterAssetsInputs.aim)
        {
            aimVitualCam.gameObject.SetActive(true);
            thirdPersonController.SetSensitivity(aimSensitivity);
            thirdPersonController.SetRotateOnMove(false);
            animator.SetLayerWeight(1, Mathf.Lerp(animator.GetLayerWeight(1), 1f, Time.deltaTime * 10f));

            Vector3 worldAimTarget = MouseWorldPosition;
            worldAimTarget.y = transform.position.y;
            Vector3 aimDirection = (worldAimTarget - transform.position).normalized;
            transform.forward = Vector3.Lerp(transform.forward, aimDirection, Time.deltaTime * 20f);
        }
        else
        {
            aimVitualCam.gameObject.SetActive(false);
            thirdPersonController.SetSensitivity(nomalSensitivity);
            thirdPersonController.SetRotateOnMove(true);
            animator.SetLayerWeight(1, Mathf.Lerp(animator.GetLayerWeight(1), 0f, Time.deltaTime * 10f));
        }

        if (starterAssetsInputs.shoot)
        {
            animator.SetLayerWeight(1, Mathf.Lerp(animator.GetLayerWeight(1), 1f, Time.deltaTime * 10f));
            // if (hitTransform != null)
            // {
            //     if (hitTransform.GetComponent<BulletTarget>() != null)
            //     {
            //         // Hit target
            //         Instantiate(vfxHitGreen, transform.position, Quaternion.identity);
            //     }
            //     else
            //     {
            //         // Hit something else
            //         Instantiate(vfxHitRed, transform.position, Quaternion.identity);
            //     }
            // }
            Vector3 aimDir = (MouseWorldPosition - spawnBulletPosition.position).normalized;
            Instantiate(pfBulletProjectTile, spawnBulletPosition.position, Quaternion.LookRotation(aimDir, Vector3.up));
            starterAssetsInputs.shoot = false;
        }


        if (Input.GetKey(KeyCode.Q))
        {
            animator.SetTrigger("SkillQ");
        }
        if (Input.GetKey(KeyCode.E))
        {
            animator.SetTrigger("SkillE");
            Transform childTransform = gameObject.transform.GetChild(0);
            childTransform.gameObject.SetActive(true);

            Invoke("SetActiveFalse", 4.0f);
        }
        if (Input.GetKey(KeyCode.R))
        {
            animator.SetTrigger("SkillR");
            GameObject obj = GameObject.Find("Meteors AOE");
            // if (obj != null)
            // {
            //     obj.transform.position = debugTransform.position;
            //     obj.SetActive(true);
            //     Invoke("SetActiveFalse", 4.0f);

            // }
            //Instantiate(pfSkillRProjectTile, debugTransform.position, Quaternion.LookRotation(MouseWorldPosition, Vector3.up));
        }



    }
    void SetActiveFalse()
    {
        Transform childTransform = gameObject.transform.GetChild(0);
        childTransform.gameObject.SetActive(false);
        // GameObject obj = GameObject.Find("Meteors AOE");
        // if (obj != null)
        // {
        //     obj.SetActive(false);
        // }
    }
}
