using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class Weapon_Manager : NetworkBehaviour
{
    public GameObject weapon; // Vũ khí
    public GameObject Skill3; // Kỹ năng 3
    private Collider weaponCollider; // Va chạm của vũ khí
    private Collider skill3Collider; // Va chạm của kỹ năng 3

    void Start()
    {
        weaponCollider = weapon.GetComponent<Collider>();
        if(Skill3 == null)
        {
            Debug.Log("chua gan");
        }
        else{
            skill3Collider = Skill3.GetComponent<Collider>();
        }
        // Tắt va chạm khi bắt đầu
        SetWeaponCollider(false);
        SetSkill3Collider(false);
    }

    [ServerRpc(RequireOwnership = false)]
    public void EnableWeaponColliderServerRpc()
    {
        SetWeaponCollider(true);
        EnableWeaponColliderClientRpc();
    }

    [ServerRpc(RequireOwnership = false)]
    public void DisableWeaponColliderServerRpc()
    {
        SetWeaponCollider(false);
        DisableWeaponColliderClientRpc();
    }

    [ServerRpc(RequireOwnership = false)]
    public void EnableSkill3ColliderServerRpc()
    {
        SetSkill3Collider(true);
        EnableSkill3ColliderClientRpc();
    }

    [ServerRpc(RequireOwnership = false)]
    public void DisableSkill3ColliderServerRpc()
    {
        SetSkill3Collider(false);
        DisableSkill3ColliderClientRpc();
    }

    public void SetWeaponCollider(bool isEnabled)
    {
        if (weaponCollider != null)
        {
            weaponCollider.enabled = isEnabled;
        }
        else
        {
            Debug.LogWarning("Weapon collider không được tìm thấy!");
        }
    }

    public void SetSkill3Collider(bool isEnabled)
    {
        if (skill3Collider != null)
        {
            skill3Collider.enabled = isEnabled;
        }
        else
        {
            Debug.LogWarning("Skill3 collider không được tìm thấy!");
        }
    }

    [ClientRpc]
    public void EnableWeaponColliderClientRpc()
    {
        SetWeaponCollider(true);
    }

    [ClientRpc]
    public void DisableWeaponColliderClientRpc()
    {
        SetWeaponCollider(false);
    }

    [ClientRpc]
    public void EnableSkill3ColliderClientRpc()
    {
        SetSkill3Collider(true);
    }

    [ClientRpc]
    public void DisableSkill3ColliderClientRpc()
    {
        SetSkill3Collider(false);
    }
}
