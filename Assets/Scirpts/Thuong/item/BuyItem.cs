using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class BuyItem : MonoBehaviour
{
    public string Name;            // Tên item
    public Sprite Image;           // Hình ảnh item
    public string Info;            // Thông tin mô tả item
    public int Quantity;           // Số lượng item
    public TextMeshProUGUI itemNameText;         // Tên item
    public TextMeshProUGUI itemQuantityText;    // Số lượng item
    public Image itemImageUI;                          // Hình ảnh item
    public Button itemButton;
   


    public void Setup(string name, string info, int itemQuantity, Sprite itemImage)
    {
        Name = name;
        Info = info;
        Quantity = itemQuantity;
        Image = itemImage;

        itemNameText.text = Name;
        itemQuantityText.text = Quantity.ToString();
        itemImageUI.sprite = Image;

        itemButton.onClick.AddListener(OnItemClick);
    }

    public void Setup1(string name, string info, int itemQuantity, Sprite itemImage)
    {
        Name = name;
        Info = info;
        Quantity = itemQuantity;
        Image = itemImage;

        itemNameText.text = Name;
        itemQuantityText.text = Quantity.ToString();
        itemImageUI.sprite = Image;

        itemButton.onClick.AddListener(OnweaponClick);
    }

    public void OnItemClick()
    {
        ItemManager.Instance.ShowItemDetails(Name, Info, Quantity, Image);
    }

    public void OnweaponClick()
    {
        ItemManager.Instance.ShowweaponDetails(Name, Info, Quantity, Image);
    }

   
}
