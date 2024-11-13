using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.Netcode;
using UnityEngine;

public class Character_Attack : NetworkBehaviour
{
    Animator animator;
    public float[] cooldownTimes = { 5f, 5f, 5f };
    float[] cooldownTimers = { 0f, 0f, 0f };
    public bool[] isCooldowns = { false, false, false };
    public TextMeshProUGUI[] tmpCooldownTimers;

    private NetworkVariable<bool> normalAttack = new NetworkVariable<bool>(false, NetworkVariableReadPermission.Everyone, NetworkVariableWritePermission.Owner);
    private NetworkVariable<bool> skill1 = new NetworkVariable<bool>(false, NetworkVariableReadPermission.Everyone, NetworkVariableWritePermission.Owner);
    private NetworkVariable<bool> skill2 = new NetworkVariable<bool>(false, NetworkVariableReadPermission.Everyone, NetworkVariableWritePermission.Owner);
    private NetworkVariable<bool> skill3 = new NetworkVariable<bool>(false, NetworkVariableReadPermission.Everyone, NetworkVariableWritePermission.Owner);
    private NetworkVariable<bool> blocking = new NetworkVariable<bool>(false, NetworkVariableReadPermission.Everyone, NetworkVariableWritePermission.Owner);
    private bool isUsingSkill = false;

    void Start()
    {
        animator = GetComponent<Animator>();
        if (animator == null)
        {
            Debug.LogError("Animator component is missing!");
        }
    }

    void Update()
    {
        if (animator == null)
        {
            Debug.LogError("Animator component is missing in Update method!");
            return;
        }

        if (IsOwner)
        {
            HandleInput();
        }

        // Cập nhật các tham số của animator dựa trên các biến mạng
        animator.SetBool("NormalAttack", normalAttack.Value);
        animator.SetBool("Skill1", skill1.Value);
        animator.SetBool("Skill2", skill2.Value);
        animator.SetBool("Skill3", skill3.Value);
        animator.SetBool("Blocking", blocking.Value);

        UpdateCooldowns();
    }

    private void HandleInput()
    {
        if (Input.GetMouseButtonDown(0) && !isUsingSkill)
        {
            normalAttack.Value = true;
            UpdateAnimationStateClientRpc("NormalAttack", true);
        }
        else
        {
            normalAttack.Value = false;
            UpdateAnimationStateClientRpc("NormalAttack", false);
        }

        if (!isUsingSkill)
        {
            if (Input.GetKey(KeyCode.Q) && !isCooldowns[0])
            {
                StartCoroutine(UseSkill(skill1, 0, "Skill1"));
            }

            if (Input.GetKey(KeyCode.E) && !isCooldowns[1])
            {
                StartCoroutine(UseSkill(skill2, 1, "Skill2"));
            }

            if (Input.GetKey(KeyCode.R) && !isCooldowns[2])
            {
                StartCoroutine(UseSkill(skill3, 2, "Skill3"));
            }
        }

        if (Input.GetKeyDown(KeyCode.F))
        {
            blocking.Value = true;
            UpdateAnimationStateClientRpc("Blocking", true);
        }
        else
        {
            blocking.Value = false;
            UpdateAnimationStateClientRpc("Blocking", false);
        }
    }

    IEnumerator UseSkill(NetworkVariable<bool> skill, int index, string animationParameter)
    {
        isUsingSkill = true;
        skill.Value = true;
        UpdateAnimationStateClientRpc(animationParameter, true);

        // Thời gian thực hiện kỹ năng
        yield return new WaitForSeconds(animator.GetCurrentAnimatorStateInfo(0).length);

        skill.Value = false;
        UpdateAnimationStateClientRpc(animationParameter, false);
        CastSkill(index);
        isUsingSkill = false;
    }

    [ClientRpc]
    void UpdateAnimationStateClientRpc(string parameter, bool state)
    {
        if (animator == null)
        {
            Debug.LogError("Animator component is missing in ClientRpc!");
            return;
        }
        animator.SetBool(parameter, state);
    }

    void UpdateCooldowns()
    {
        for (int i = 0; i < cooldownTimes.Length; i++)
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

    void CastSkill(int skillIndex)
    {
        isCooldowns[skillIndex] = true;
        cooldownTimers[skillIndex] = cooldownTimes[skillIndex];
    }
}
