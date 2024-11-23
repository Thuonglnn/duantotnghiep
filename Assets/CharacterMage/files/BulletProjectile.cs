using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using Unity.Netcode;

public class BulletProjectile : NetworkBehaviour
{

    [SerializeField] private Transform vfxHitGreen;
    [SerializeField] private Transform vfxHitRed;

    public AttributesManager creatorAttributes;
    public NetworkObject creatorNetworkObject;

    private Rigidbody bulletRigidbody;

    public int dmgBonus = 2;

    private void Awake()
    {
        bulletRigidbody = GetComponent<Rigidbody>();
    }
    //thisok
    private void Start()
    {
        float speed = 50f;
        bulletRigidbody.velocity = transform.forward * speed;
    }



    private void OnTriggerEnter(Collider other)
    {
        if (other.GetComponent<BulletTarget>() != null)
        {
            // Hit target
            Instantiate(vfxHitGreen, transform.position, Quaternion.identity);



        }
        else
        {
            // Hit something else
            Instantiate(vfxHitRed, transform.position, Quaternion.identity);


        }
        // Destroy(gameObject);

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