using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class listweapon : MonoBehaviour
{
    public string Name;                   // Tên item
    public Sprite Image;                  // Hình ảnh item
    public string Info;                   // Thông tin mô tả item
    public int Quantity;                  // Số lượng item
    public TextMeshProUGUI itemNameText;  // Tên item
    public TextMeshProUGUI itemQuantityText;  // Số lượng item
    public TextMeshProUGUI infoText;      // Thông tin mô tả item
    public Image itemImageUI;             // Hình ảnh item
    public Button btnuse;                 // Nút sử dụng vật phẩm
    public Button btndestroy;             // Nút hủy vật phẩm

    private bool hasUsedItem = false;
    private string currentID;  // Biến lưu ID hiện tại đang sử dụng vật phẩm

    // Dictionary lưu trạng thái đã sử dụng vật phẩm cho từng nhân vật
    private Dictionary<string, bool> usedItemsForCharacter = new Dictionary<string, bool>();

    // Thiết lập các thông tin của item
    public void Setup(string name, string info, int itemQuantity, Sprite itemImage)
    {
        Name = name;
        Info = info;
        Quantity = itemQuantity;
        Image = itemImage;

        itemNameText.text = Name;
        infoText.text = Info;
        itemQuantityText.text = Quantity.ToString();
        itemImageUI.sprite = Image;
        btnuse.onClick.AddListener(UseItem);
        btndestroy.onClick.AddListener(DestroyItem);
        btndestroy.interactable = false;
    }

    // Cập nhật currentID khi chọn item
    public void SetCurrentID(string id)
    {
        currentID = id;
    }

    // Sử dụng vật phẩm nếu đủ điều kiện
    void UseItem()
    {
        if (Quantity > 0)
        {
            // Kiểm tra nếu vật phẩm chưa được sử dụng cho nhân vật hiện tại
            if (currentID == "dausia" || currentID == "dausib" || currentID == "cungthu" || currentID == "phapsu")
            {
                if (!usedItemsForCharacter.ContainsKey(currentID) || !usedItemsForCharacter[currentID])
                {
                    Quantity--;
                    itemQuantityText.text = Quantity.ToString();
                    usedItemsForCharacter[currentID] = true; // Đánh dấu vật phẩm đã được sử dụng cho nhân vật này
                    hasUsedItem = true;
                    btndestroy.interactable = true;
                    Debug.Log(currentID + " đã sử dụng vật phẩm " + Name);
                }
                else
                {
                    Debug.Log(currentID + " đã sử dụng vật phẩm này trước đó.");
                }
            }
            else
            {
                Debug.Log("Vật phẩm không còn sẵn.");
            }
        }
    }

    // Hủy vật phẩm và cho phép sử dụng lại
    void DestroyItem()
    {
        if (hasUsedItem)
        {
            // Kiểm tra và hủy vật phẩm chỉ cho nhân vật hiện tại
            if (usedItemsForCharacter.ContainsKey(currentID) && usedItemsForCharacter[currentID])
            {
                Quantity++;
                itemQuantityText.text = Quantity.ToString();
                Debug.Log("Hủy vật phẩm: " + Name + " | Số lượng còn lại: " + Quantity);

                // Reset lại trạng thái đã sử dụng cho nhân vật
                usedItemsForCharacter[currentID] = false;
                hasUsedItem = false;
                btndestroy.interactable = false;
            }
        }
    }
}
