using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;

public class Weapon_Dmg : NetworkBehaviour
{
    public AttributesManager player;

    private void OnTriggerEnter(Collider other)
    {
        if (IsOwner)
        {
            if (other.CompareTag("Player1"))
            {
                AttributesManager enemy = other.GetComponent<AttributesManager>();
                if (player != null && enemy != null)
                {
                    player.DealDmg(enemy.gameObject, player.atk);
                }
            }
            if (other.CompareTag("Enemy"))
            {
                AttributesManager enemy = other.GetComponent<AttributesManager>();
                if (player != null && enemy != null)
                {
                    player.DealDmg(enemy.gameObject, player.atk);
                }
            }
        }
    }
}
