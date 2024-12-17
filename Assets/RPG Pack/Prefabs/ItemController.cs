using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemController : MonoBehaviour
{
    [Header("Thời gian tồn tại")]
    public float lifetime = 30f; // Thời gian vật phẩm tồn tại (giây)

    private void Start()
    {
        // Tự động hủy vật phẩm sau thời gian tồn tại
        Destroy(gameObject, lifetime);
    }

    private void OnTriggerEnter(Collider other)
    {
        // Kiểm tra va chạm với người chơi
        if (other.CompareTag("Player"))
        {
            // Xử lý logic khi người chơi nhận vật phẩm
            CollectItem(other);

            // Hủy vật phẩm sau khi thu thập
            Destroy(gameObject);
        }
    }

    private void CollectItem(Collider player)
    {
        // Thêm logic thu thập vật phẩm tại đây
        // Ví dụ: tăng máu hoặc thêm coin vào tổng điểm
        if (gameObject.name.Contains("HealthPotion"))
        {
            Debug.Log("Player đã nhận bình máu!");
            // Gọi hàm tăng máu trong PlayerController nếu cần
        }
        else if (gameObject.name.Contains("Coin"))
        {
            Debug.Log("Player đã nhận coin!");
            // Gọi hàm tăng điểm trong PlayerController nếu cần
        }
    }
}

