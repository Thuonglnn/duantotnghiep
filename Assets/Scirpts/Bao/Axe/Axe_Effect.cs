using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class Axe_Effect : NetworkBehaviour
{
    public ParticleSystem NormalAttack_1;
    public GameObject SkillAttack_1; // Prefab quả cầu
    public ParticleSystem SkillAttack_2;
    public ParticleSystem SkillAttack_3;
    public Transform axe;

    void Start()
    {
        NormalAttack_1.Stop();
        SkillAttack_2.Stop();
        SkillAttack_3.Stop();
    }

    void Update()
    {
        // Nếu có điều kiện kích hoạt hiệu ứng tại đây, có thể gọi ServerRpc
    }

    public void normalAttack1()
    {
        if (IsOwner)
        {
            TriggerNormalAttack1ServerRpc();
        }
    }

    public void skillAttack1()
    {
        if (IsOwner)
        {
            TriggerSkillAttack1ServerRpc();
        }
    }

    public void skillAttack2()
    {
        if (IsOwner)
        {
            TriggerSkillAttack2ServerRpc();
        }
    }

    public void skillAttack3()
    {
        if (IsOwner)
        {
            TriggerSkillAttack3ServerRpc();
        }
    }

    [ServerRpc]
    void TriggerNormalAttack1ServerRpc()
    {
        TriggerNormalAttack1ClientRpc();
    }

    [ServerRpc]
    void TriggerSkillAttack1ServerRpc()
    {
        TriggerSkillAttack1ClientRpc();
    }

    [ServerRpc]
    void TriggerSkillAttack2ServerRpc()
    {
        TriggerSkillAttack2ClientRpc();
    }

    [ServerRpc]
    void TriggerSkillAttack3ServerRpc()
    {
        TriggerSkillAttack3ClientRpc();
    }

    [ClientRpc]
    void TriggerNormalAttack1ClientRpc()
    {
        NormalAttack_1.Play();
    }

    [ClientRpc]
    void TriggerSkillAttack1ClientRpc()
    {
        GameObject ball = Instantiate(SkillAttack_1, axe.position, Quaternion.identity);
        var axeSkillScript = ball.GetComponent<CharacterDmgSkill>();
        axeSkillScript.creatorNetworkObject = GetComponent<NetworkObject>(); // Gán NetworkObject của người tạo
        axeSkillScript.creatorAttributes = GetComponent<AttributesManager>(); // Gán AttributesManager của người tạo
        ball.GetComponent<NetworkObject>().Spawn(); // Spawn qua mạng
    }

    [ClientRpc]
    void TriggerSkillAttack2ClientRpc()
    {
        SkillAttack_2.Play();
    }

    [ClientRpc]
    void TriggerSkillAttack3ClientRpc()
    {
        SkillAttack_3.Play();
    }
}
