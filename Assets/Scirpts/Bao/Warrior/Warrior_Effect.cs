using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class Warrior_Effect : NetworkBehaviour
{
    public ParticleSystem NormalAttack_1;
    public ParticleSystem SkillAttack_1_1;
    public ParticleSystem SkillAttack_1_2;
    public ParticleSystem SkillAttack_1_3;
    public ParticleSystem SkillAttack_2;
    public ParticleSystem SkillAttack_3;
    public GameObject SkillAttack_3_1;

    public Transform player;

    void Start()
    {
        StopAllEffects();
    }

    void StopAllEffects()
    {
        NormalAttack_1.Stop();
        SkillAttack_1_1.Stop();
        SkillAttack_1_2.Stop();
        SkillAttack_1_3.Stop();
        SkillAttack_2.Stop();
        SkillAttack_3.Stop();
    }

    // Method to trigger normal attack effect
    public void PlayNormalAttack1()
    {
        if (IsOwner)
        {
            TriggerNormalAttack1ServerRpc();
        }
    }

    // Method to trigger skill attack 1_1 effect
    public void PlaySkillAttack1_1()
    {
        if (IsOwner)
        {
            TriggerSkillAttack1_1ServerRpc();
        }
    }

    // Method to trigger skill attack 1_2 effect
    public void PlaySkillAttack1_2()
    {
        if (IsOwner)
        {
            TriggerSkillAttack1_2ServerRpc();
        }
    }

    // Method to trigger skill attack 1_3 effect
    public void PlaySkillAttack1_3()
    {
        if (IsOwner)
        {
            TriggerSkillAttack1_3ServerRpc();
        }
    }

    // Method to trigger skill attack 2 effect
    public void PlaySkillAttack2()
    {
        if (IsOwner)
        {
            TriggerSkillAttack2ServerRpc();
        }
    }

    // Method to trigger skill attack 3 effect
    public void PlaySkillAttack3()
    {
        if (IsOwner)
        {
            TriggerSkillAttack3ServerRpc();
        }
    }

    public void StopSkillAttack3()
    {
        if (IsOwner)
        {
            TriggerStopSkillAttack3ServerRpc();
        }
    }

    // Method to trigger skill attack 3_1 effect
    public void PlaySkillAttack3_1()
    {
        if (IsOwner)
        {
            TriggerSkillAttack3_1ServerRpc();
        }
    }

    [ServerRpc]
    void TriggerNormalAttack1ServerRpc()
    {
        TriggerNormalAttack1ClientRpc();
    }

    [ServerRpc]
    void TriggerSkillAttack1_1ServerRpc()
    {
        TriggerSkillAttack1_1ClientRpc();
    }

    [ServerRpc]
    void TriggerSkillAttack1_2ServerRpc()
    {
        TriggerSkillAttack1_2ClientRpc();
    }

    [ServerRpc]
    void TriggerSkillAttack1_3ServerRpc()
    {
        TriggerSkillAttack1_3ClientRpc();
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
    
    [ServerRpc]
    void TriggerStopSkillAttack3ServerRpc()
    {
        TriggerStopSkillAttack3ClientRpc();
    }

    [ServerRpc]
    void TriggerSkillAttack3_1ServerRpc()
    {
        TriggerSkillAttack3_1ClientRpc();
    }

    [ClientRpc]
    void TriggerNormalAttack1ClientRpc()
    {
        NormalAttack_1.Play();
    }

    [ClientRpc]
    void TriggerSkillAttack1_1ClientRpc()
    {
        SkillAttack_1_1.Play();
    }

    [ClientRpc]
    void TriggerSkillAttack1_2ClientRpc()
    {
        SkillAttack_1_2.Play();
    }

    [ClientRpc]
    void TriggerSkillAttack1_3ClientRpc()
    {
        SkillAttack_1_3.Play();
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
     [ClientRpc]
    void TriggerStopSkillAttack3ClientRpc()
    {
        SkillAttack_3.Stop();
    }

    [ClientRpc]
    void TriggerSkillAttack3_1ClientRpc()
    {
        GameObject skill3 = Instantiate(SkillAttack_3_1, player.position, player.rotation);
        var axeSkillScript = skill3.GetComponent<CharacterDmgSkill>();
        axeSkillScript.creatorNetworkObject = GetComponent<NetworkObject>(); // Gán NetworkObject của người tạo
        axeSkillScript.creatorAttributes = GetComponent<AttributesManager>(); // Gán AttributesManager của người tạo
        skill3.GetComponent<NetworkObject>().Spawn(); // Spawn qua mạng
    }
}
