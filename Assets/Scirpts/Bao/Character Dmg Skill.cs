using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class CharacterDmgSkill : NetworkBehaviour
{
    public AttributesManager creatorAttributes;
    public NetworkObject creatorNetworkObject;
    public float DestroyTime = 3f;
    public int AtkBonus = 10;

    // Thời gian trễ khi gây sát thương
    public float damageInterval = 0.8f;
    // Từng quái vật sẽ có thời gian gây sát thương riêng
    private Dictionary<AttributesManager, float> enemyLastDamageTime = new Dictionary<AttributesManager, float>();

    void Start()
    {
        // Hủy đối tượng này sau một khoảng thời gian
        DestroyAfterTime();
    }

    void OnTriggerStay(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            // Kiểm tra xem đối tượng va chạm có phải là người tạo ra quả cầu hay không
            if (other.GetComponent<NetworkObject>() == creatorNetworkObject)
            {
                return; // Bỏ qua nếu đúng là người tạo ra
            }

            AttributesManager enemy = other.GetComponent<AttributesManager>();
            if (enemy != null)
            {
                // Nếu enemy không có trong dictionary, thêm nó vào với thời gian ban đầu là 0
                if (!enemyLastDamageTime.ContainsKey(enemy))
                {
                    enemyLastDamageTime[enemy] = 0f;
                }

                // Kiểm tra nếu đã qua thời gian trễ có thể gây sát thương
                if (Time.time >= enemyLastDamageTime[enemy] + damageInterval)
                {
                    creatorAttributes.DealDmg(enemy.gameObject, creatorAttributes.atk + AtkBonus);
                    enemyLastDamageTime[enemy] = Time.time;
                }
            }
        }
    }

    void DestroyAfterTime()
    {
        Destroy(gameObject, DestroyTime);
    }
}
