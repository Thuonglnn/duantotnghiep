using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;

public class TrollDMG : NetworkBehaviour
{
    public AttributesManager player;

    private void OnTriggerEnter(Collider other)
    {
        if (IsOwner)
        {
            if (other.CompareTag("Player"))
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
