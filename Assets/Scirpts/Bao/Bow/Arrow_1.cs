using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;

public class Arrow_1 : NetworkBehaviour
{
    public float speed = 10f;
    public float timeDestroy = 3f;
    public int dmgBonus = 2;
    public AttributesManager creatorAttributes;
    public NetworkObject creatorNetworkObject;


    void Start()
    {
        if (IsServer)
        {
            Invoke("DestroyArrow", timeDestroy);
        }
    }
    void Update()
    {
        transform.Translate(Vector3.forward * speed * Time.deltaTime);
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player1"))
        {
            // Kiểm tra xem đối tượng va chạm có phải là người tạo ra quả cầu hay không
            if (other.GetComponent<NetworkObject>() == creatorNetworkObject)
            {
                return; // Bỏ qua nếu đúng là người tạo ra
            }

            AttributesManager enemy = other.GetComponent<AttributesManager>();
            if (enemy != null)
            {
                creatorAttributes.DealDmg(enemy.gameObject, creatorAttributes.atk + dmgBonus);
                Destroy(gameObject);
            }
        }
        if (other.gameObject.CompareTag("Enemy"))
        {
            // Kiểm tra xem đối tượng va chạm có phải là người tạo ra quả cầu hay không
            if (other.GetComponent<NetworkObject>() == creatorNetworkObject)
            {
                return; // Bỏ qua nếu đúng là người tạo ra
            }

            AttributesManager enemy = other.GetComponent<AttributesManager>();
            if (enemy != null)
            {
                creatorAttributes.DealDmg(enemy.gameObject, creatorAttributes.atk + dmgBonus);
                Destroy(gameObject);
            }
        }
    }

    void DestroyAfterTime()
    {
        Destroy(gameObject, 3f);
    }



    [ServerRpc]
    public void RequestDestroyServerRpc()
    {
        DestroyArrow();
    }

    [ClientRpc]
    void DestroyArrowClientRpc()
    {
        Destroy(gameObject);
    }

    private void DestroyArrow()
    {
        DestroyArrowClientRpc();
        Destroy(gameObject);
    }
}
