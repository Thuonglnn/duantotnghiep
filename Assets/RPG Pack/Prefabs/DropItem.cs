using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DropItem : MonoBehaviour
{
    [Header("Vật phẩm")]
    public GameObject healthPotion; // Bình máu
    public GameObject coin;         // Coin

    [Header("Tỉ lệ rơi (%)")]
    [Range(0, 100)] public float healthPotionDropRate = 20f; // Tỉ lệ rơi bình máu
    [Range(0, 100)] public float coinDropRate = 50f;         // Tỉ lệ rơi coin

    public void Drop()
    {
        // Random một số từ 0 đến 100
        float randomValue = Random.Range(0f, 100f);

        // Kiểm tra tỉ lệ rơi vật phẩm
        if (randomValue < healthPotionDropRate)
        {
            // Rơi bình máu
            Instantiate(healthPotion, transform.position, Quaternion.identity);
        }
        else if (randomValue < healthPotionDropRate + coinDropRate)
        {
            // Rơi coin
            Instantiate(coin, transform.position, Quaternion.identity);
        }
    }
}
