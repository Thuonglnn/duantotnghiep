using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
using StarterAssets;
using UnityEngine.InputSystem;
using Unity.Mathematics;


public class Aiming : MonoBehaviour
{
    [SerializeField] private CinemachineFreeLook aimVitualCam;
    [SerializeField] private float nomalSensitivity;
    [SerializeField] private float aimSensitivity;
    [SerializeField] private LayerMask aimColliderLayerMask = new LayerMask();
    [SerializeField] private Transform debugTransform;
    [SerializeField] private Transform pfBulletProjectTile;
    [SerializeField] private Transform pfSkillRProjectTile;
    [SerializeField] private Transform pfSkillQProjectTile;
    [SerializeField] private Transform spawnBulletPosition;
    [SerializeField] private Transform vfxHitGreen;
    [SerializeField] private Transform vfxHitRed;

    private Animator animator;


    // private ThirdPersonController thirdPersonController;
    // private StarterAssetsInputs starterAssetsInputs;

    PlayerStatsController playerStatsController;

    private void Awake()
    {
        // thirdPersonController = GetComponent<ThirdPersonController>();
        // starterAssetsInputs = GetComponent<StarterAssetsInputs>();
        animator = GetComponent<Animator>();
        playerStatsController = GetComponent<PlayerStatsController>();
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

        if (Input.GetKeyDown(KeyCode.Mouse1))
        {
            aimVitualCam.gameObject.SetActive(true);
            // thirdPersonController.SetSensitivity(aimSensitivity);
            // thirdPersonController.SetRotateOnMove(false);
            animator.SetLayerWeight(1, Mathf.Lerp(animator.GetLayerWeight(1), 1f, Time.deltaTime * 10f));

            Vector3 worldAimTarget = MouseWorldPosition;
            worldAimTarget.y = transform.position.y;
            Vector3 aimDirection = (worldAimTarget - transform.position).normalized;
            transform.forward = Vector3.Lerp(transform.forward, aimDirection, Time.deltaTime * 20f);
        }
        else
        {
            aimVitualCam.gameObject.SetActive(false);
            // thirdPersonController.SetSensitivity(nomalSensitivity);
            // thirdPersonController.SetRotateOnMove(true);
            animator.SetLayerWeight(1, Mathf.Lerp(animator.GetLayerWeight(1), 0f, Time.deltaTime * 10f));
        }

        if (Input.GetKeyDown(KeyCode.Mouse0))
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
            // starterAssetsInputs.shoot = false;
        }


        if (Input.GetKeyUp(KeyCode.Q))
        {
            if (playerStatsController.IsSkillOnCooldown(PlayerStatsController.Skill.Q))
            {

            }
            else
            {
                animator.SetTrigger("SkillQ");

                Vector3 aimDir = (MouseWorldPosition - spawnBulletPosition.position).normalized;
                Instantiate(pfSkillQProjectTile, spawnBulletPosition.position, Quaternion.LookRotation(aimDir, Vector3.up));
                playerStatsController.UseSkill(PlayerStatsController.Skill.Q, 15);
            }

        }
        if (Input.GetKey(KeyCode.E))
        {
            if (playerStatsController.IsSkillOnCooldown(PlayerStatsController.Skill.E))
            {

            }
            else
            {
                animator.SetTrigger("SkillE");
                Transform childTransform = gameObject.transform.GetChild(0);
                childTransform.gameObject.SetActive(true);

                Invoke("SetActiveFalse", 4.0f);
                playerStatsController.UseSkill(PlayerStatsController.Skill.E, 10);
            }

        }
        if (Input.GetKeyUp(KeyCode.R))
        {
            if (playerStatsController.IsSkillOnCooldown(PlayerStatsController.Skill.R))
            {

            }
            else
            {
                animator.SetTrigger("SkillR");
                Vector3 aimDir = (MouseWorldPosition).normalized;
                Instantiate(pfSkillRProjectTile, MouseWorldPosition, Quaternion.LookRotation(aimDir, Vector3.up));
                playerStatsController.UseSkill(PlayerStatsController.Skill.R, 30);
            }

        }



    }
    void SetActiveFalse()
    {
        Transform childTransform = gameObject.transform.GetChild(0);
        childTransform.gameObject.SetActive(false);
    }
}
