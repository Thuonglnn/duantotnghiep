using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Unity.Netcode;

public class AttributesManager : NetworkBehaviour
{
    public NetworkVariable<int> hp = new NetworkVariable<int>(100, NetworkVariableReadPermission.Everyone, NetworkVariableWritePermission.Server);
    public int def = 100;
    public int atk = 10;
    public float critRate = 0.5f; // 50%
    public float critDamage = 2f; // 200%

    public Slider healthBar;

    private void Start()
    {
        if (IsOwner && healthBar != null)
        {
            healthBar.maxValue = hp.Value;
            healthBar.minValue = 0;
        }
    }

    private void Update()
    {
        if (IsOwner && healthBar != null)
        {
            healthBar.value = hp.Value;
        }
    }

    [ServerRpc(RequireOwnership = false)]
    public void DealDmgServerRpc(NetworkObjectReference targetRef, int attack)
    {
        if (targetRef.TryGet(out NetworkObject targetObj))
        {
            var targetAttributes = targetObj.GetComponent<AttributesManager>();

            if (targetAttributes != null)
            {
                bool isCrit = Random.value <= critRate;
                int finalDamage = isCrit ? Mathf.RoundToInt(attack * critDamage) : attack;

                targetAttributes.TakeDmg(finalDamage, isCrit);
            }
        }
    }

    public void DealDmg(GameObject target, int attack)
    {
        if (IsOwner)
        {
            NetworkObjectReference targetRef = target.GetComponent<NetworkObject>();
            DealDmgServerRpc(targetRef, attack);
        }
    }

    public void TakeDmg(int amount, bool isCrit)
    {
        if (IsServer)
        {
            int damage = Mathf.Max(0, amount - def);
            hp.Value -= damage;

            if (!isCrit)
            {
                DmgPopUpGerenator.Instance.CreatePopUp(transform.position, damage.ToString(), Color.red, false);
            }
            else
            {
                DmgPopUpGerenator.Instance.CreatePopUp(transform.position, damage.ToString(), Color.yellow, true);
            }
        }
    }



}
