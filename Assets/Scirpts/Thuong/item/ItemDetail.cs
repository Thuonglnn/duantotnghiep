using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class ItemDetail : MonoBehaviour
{
    public TextMeshProUGUI itemNameText;  // Hiển thị tên item
    public TextMeshProUGUI itemInfoText;  // Hiển thị thông tin mô tả item
    public TextMeshProUGUI itemQuantityText;  // Hiển thị số lượng item
    public Image itemImageUI;             // Hiển thị hình ảnh item

     public Button btntrangbituong;


     
    private void Start()
    {
        // Đăng ký sự kiện cho nút btntrangbituong
        if (btntrangbituong != null)
        {
            btntrangbituong.onClick.AddListener(onclicktrangbituong);
        }
    }

    // Hàm này sẽ được gọi để thiết lập thông tin chi tiết item
    public void Setup(string name, string info, int quantity, Sprite image)
    {
        itemNameText.text = name;       // Hiển thị tên item
        itemInfoText.text = info;       // Hiển thị mô tả item
        itemQuantityText.text = quantity.ToString(); // Hiển thị số lượng item
        itemImageUI.sprite = image;     // Hiển thị hình ảnh item
    }

       public void Setup1(string name, string info, int quantity, Sprite image)
    {
        itemNameText.text = name;       // Hiển thị tên item
        itemInfoText.text = info;       // Hiển thị mô tả item
        itemQuantityText.text = quantity.ToString(); // Hiển thị số lượng item
        itemImageUI.sprite = image;     // Hiển thị hình ảnh item
    }

     public void onclicktrangbituong()
    {
        SceneManager.LoadScene("trangbituong");
        Time.timeScale = 1;
    }
}
