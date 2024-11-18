using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.Netcode;
using UnityEngine;

public class Bow_Attack : NetworkBehaviour
{
    Animator animator;

    [SerializeField] private LayerMask aimColliderLayerMask;
    [SerializeField] private Transform debugTransform;
    [SerializeField] private Transform spawnBulletPosition;

    public GameObject arrowModel;
    public GameObject Arrow;
    public GameObject ArrowIce;
    public GameObject BigIceArrow;
    public Transform transformArrow;
    public ParticleSystem SkillAttack_1;

    public float[] cooldownTimes = { 10f, 8f, 12f }; 
    float[] cooldownTimers = { 0f, 0f, 0f };  
    public static bool[] isCooldowns = { false, false, false }; 
    public TextMeshProUGUI[] tmpCooldownTimers;

    float TimeSkill_1;
    private NetworkVariable<bool> Skill1 = new NetworkVariable<bool>(false, NetworkVariableReadPermission.Everyone, NetworkVariableWritePermission.Owner);
    

    private NetworkVariable<Vector3> mouseWorldPosition = new NetworkVariable<Vector3>();
    private bool isUsingSkill = false;
    void Start()
    {
        animator = GetComponent<Animator>();
        SkillAttack_1.Stop();
        arrowModel.SetActive(false);
        TimeSkill_1 = 0;
    }

    void Update()
    {
        if(!IsOwner)return;
        animator.SetBool("isAttacking", false);
        animator.SetBool("Skill2",false);
        animator.SetBool("Skill3",false);
        NormalAttack();
        AllSkills();
        CoolDownTime();
    }

    private void CastSkill(int skillIndex)
    {
        isCooldowns[skillIndex] = true;
        cooldownTimers[skillIndex] = cooldownTimes[skillIndex];
    }

    private void AllSkills()
    {
        
        if (Input.GetKey(KeyCode.Q) && !isCooldowns[0])
        {
            Skill1.Value = true;
            TimeSkill_1 = Time.time;
            SkillAttack_1.Play();
            CastSkill(0);
        }
        if (Skill1.Value)
        {
            if (Time.time - TimeSkill_1 > 4f)
            {
                Skill1.Value = false;
                TimeSkill_1 = 0;
                SkillAttack_1.Stop();
            }
        }

        if(!isUsingSkill)
        {
            if (Input.GetKey(KeyCode.E) && !isCooldowns[1])
            {
                StartCoroutine(UseSkill( 1, "Skill2"));
            }
            if (Input.GetKeyDown(KeyCode.R) && !isCooldowns[2])
            {
                StartCoroutine(UseSkill( 2, "Skill3"));
            }
        }
        
    }

    IEnumerator UseSkill( int index, string animationParameter)
    {
        isUsingSkill = true;

        AnimationSkill(animationParameter, true);

        // Thời gian thực hiện kỹ năng
        yield return new WaitForSeconds(animator.GetCurrentAnimatorStateInfo(0).length);

        //AnimationSkill(animationParameter, false);
        CastSkill(index);
        isUsingSkill = false;
    }

    void AnimationSkill(string parameter, bool state)
    {
        animator.SetBool(parameter, state);
    }



    private void CoolDownTime()
    {
        for (int i = 0; i < 3; i++)
        {
            if (isCooldowns[i])
            {
                cooldownTimers[i] -= Time.deltaTime;
                tmpCooldownTimers[i].text = "" + Mathf.Ceil(cooldownTimers[i]);

                if (cooldownTimers[i] <= 0)
                {
                    isCooldowns[i] = false;
                    tmpCooldownTimers[i].text = "";
                }
            }
        }
    }

    private void NormalAttack()
    {
        if (Input.GetMouseButtonDown(1))
        {
            animator.SetBool("DrawArrow", true);
        }
        if (Input.GetMouseButtonUp(1))
        {
            animator.SetBool("DrawArrow", false);
            arrowModel.SetActive(false);
        }
        if (Input.GetMouseButtonDown(0))
        {
            animator.SetBool("isAttacking", true);
        }
    }


    public void SetActiveArrowTrue()
    {
        arrowModel.SetActive(true);

        if (Skill1.Value)
        {
            SkillAttack_1.Play();
        }
        else
        {
            SkillAttack_1.Stop();
        }
    }

    public void SetActiveArrowFalse()
    {
        arrowModel.SetActive(false);
        if (Skill1.Value)
        {
            SkillAttack_1.Play();
        }
        else
        {
            SkillAttack_1.Stop();
        }
    }

    public void BowAttacking()
    {
        if (IsOwner) // Chỉ chủ sở hữu mới có thể gọi ServerRpc
        {
            HandleAttackingServerRpc(GetMouseWorldPosition());
        }
    }
    public void Skill3Attacking()
    {
        if (IsOwner) // Chỉ chủ sở hữu mới có thể gọi ServerRpc
        {
            HandleBigIceArrowServerRpc();
        }
    }

    [ServerRpc]
    void HandleAttackingServerRpc(Vector3 mouseWorldPosition)
    {
        HandleShooting(mouseWorldPosition);
    }

    [ServerRpc]
    void HandleBigIceArrowServerRpc()
    {
        GameObject bigarrow =Instantiate(BigIceArrow, spawnBulletPosition.position, spawnBulletPosition.rotation);
        var BowSkillScript = bigarrow.GetComponent<Arrow_2>();
        BowSkillScript.creatorNetworkObject = GetComponent<NetworkObject>(); // Gán NetworkObject của người tạo
        BowSkillScript.creatorAttributes = GetComponent<AttributesManager>(); // Gán AttributesManager của người tạo
        bigarrow.GetComponent<NetworkObject>().Spawn(); // Spawn qua mạng
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
        Vector3 aimDir = (mouseWorldPosition - spawnBulletPosition.position).normalized;
        if (Skill1.Value)
        {
            GameObject arrowice = Instantiate(ArrowIce, spawnBulletPosition.position, Quaternion.LookRotation(aimDir, Vector3.up));
            var BowSkillScript = arrowice.GetComponent<Arrow_1>();
            BowSkillScript.creatorNetworkObject = GetComponent<NetworkObject>(); // Gán NetworkObject của người tạo
            BowSkillScript.creatorAttributes = GetComponent<AttributesManager>(); // Gán AttributesManager của người tạo
            arrowice.GetComponent<NetworkObject>().Spawn(); // Spawn qua mạng
        }
        else
        {
            GameObject arrow = Instantiate(Arrow, spawnBulletPosition.position, Quaternion.LookRotation(aimDir, Vector3.up));
            var BowSkillScript = arrow.GetComponent<Arrow_1>();
            BowSkillScript.creatorNetworkObject = GetComponent<NetworkObject>(); // Gán NetworkObject của người tạo
            BowSkillScript.creatorAttributes = GetComponent<AttributesManager>(); // Gán AttributesManager của người tạo
            arrow.GetComponent<NetworkObject>().Spawn(); // Spawn qua mạng
        }

        
       
    }

    
    
}
