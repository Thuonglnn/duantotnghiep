using System.Collections;
using UnityEngine;
using UnityEngine.Networking;
using Newtonsoft.Json;
using System.Collections.Generic;
using UnityEngine.UI;

public class ItemManager : MonoBehaviour
{
    public static ItemManager Instance; // Đảm bảo có thể gọi từ mọi nơi trong dự án
    public Transform contentParent; // Nơi chứa các BuyItem trong ScrollView
    public GameObject buyItemPrefab;

    public Transform contentParentweapon; // Nơi chứa các BuyItem trong ScrollView
    public GameObject weaponPrefab;
    public GameObject itemDetailPrefab;

    public GameObject weaponDetailPrefab;



    public Transform itemDetailParent; // Nơi hiển thị chi tiết item

    // Các hình ảnh item
    public Sprite HP1Image;
    public Sprite HP2Image;
    public Sprite HP3Image;


    public Sprite MP1Image;
    public Sprite MP2Image;
    public Sprite MP3Image;





    public Sprite MVImage;
    public Sprite MDImage;


    public Sprite NxVImage;
    public Sprite NxDImage;


    public Sprite SVImage;
    public Sprite SDImage;


    public Sprite QVImage;
    public Sprite QDImage;

    public Sprite AVImage;
    public Sprite ADImage;










    void Awake()
    {
        // Đảm bảo chỉ có một instance duy nhất của ShopManager
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    void Start()
    {
        UpdateUserItems();
        UpdateUserWeapons();



    }

    public void khoaitem()
    {
        // Duyệt qua tất cả các đối tượng trong scene
        foreach (GameObject obj in GameObject.FindObjectsOfType<GameObject>())
        {
            // Kiểm tra nếu tên của đối tượng là "pnuse1(Clone)"
            if (obj.name == "pnuse1(Clone)")
            {
                Destroy(obj);  // Xóa đối tượng khỏi scene
            }
        }
    }

    public void khoaweapon()
    {
        // Duyệt qua tất cả các đối tượng trong scene
        foreach (GameObject obj in GameObject.FindObjectsOfType<GameObject>())
        {
            // Kiểm tra nếu tên của đối tượng là "pnweapon(Clone)"
            if (obj.name == "pnweapon(Clone)")
            {
                Destroy(obj);  // Xóa đối tượng khỏi scene
            }
        }
    }

    public void xoaCaHai()
    {
        // Duyệt qua tất cả các đối tượng trong scene
        foreach (GameObject obj in GameObject.FindObjectsOfType<GameObject>())
        {
            // Kiểm tra nếu tên của đối tượng là "pnuse1(Clone)" hoặc "pnweapon(Clone)"
            if (obj.name == "pnuse1(Clone)" || obj.name == "pnweapon(Clone)")
            {
                Destroy(obj);  // Xóa đối tượng khỏi scene
            }
        }
    }






    public void UpdateUserItems()
    {
        var username = LoginUser.loginResponseModel.username; // Lấy username từ LoginUser
        StartCoroutine(GetUserItems(username));               // Gọi API
    }

    IEnumerator GetUserItems(string username)
    {
        var url = "http://localhost:3005/users/get-user-items?username=" + username;
        var request = UnityWebRequest.Get(url);
        request.SetRequestHeader("Content-Type", "application/json");

        yield return request.SendWebRequest();

        if (request.result != UnityWebRequest.Result.Success)
        {
            Debug.LogError("API Error: " + request.error);
        }
        else
        {
            var jsonString = request.downloadHandler.text;
            Debug.Log("Response: " + jsonString); // Debug response từ API

            var responseModel = JsonConvert.DeserializeObject<ResponseModel>(jsonString);
            if (responseModel.status == 1)
            {
                foreach (var item in responseModel.items)
                {
                    // Kiểm tra từng itemId và tạo item với các thông tin cụ thể
                    if (item.itemId == "HP1")
                        CreateItem(item.quantity, "Bình máu nhỏ", HP1Image, "HP+10");
                    else if (item.itemId == "HP2")
                        CreateItem(item.quantity, "Bình máu trung bình", HP2Image, "HP+20");
                    else if (item.itemId == "HP3")
                        CreateItem(item.quantity, "Bình máu to", HP3Image, "HP+30");




                    if (item.itemId == "MP1")
                        CreateItem(item.quantity, "Bình mana nhỏ", MP1Image, "MP+10");
                    else if (item.itemId == "MP2")
                        CreateItem(item.quantity, "Bình mana trung bình", MP2Image, "MP+20");
                    else if (item.itemId == "MP3")
                        CreateItem(item.quantity, "Bình mana to", MP3Image, "MP+30");



                }
            }
            else
            {
                Debug.LogError("API Error: " + responseModel.message);
            }
        }
    }

    // Tạo item và thêm vào danh sách
    private void CreateItem(int quantity, string name, Sprite itemImage, string info)
    {
        var newItem = Instantiate(buyItemPrefab, contentParent);  // Tạo item từ prefab
        var buyItemScript = newItem.GetComponent<BuyItem>();

        if (buyItemScript != null)
        {
            buyItemScript.Setup(name, info, quantity, itemImage);  // Truyền thông tin vào prefab
            newItem.GetComponent<Button>().onClick.AddListener(buyItemScript.OnItemClick); // Đăng ký sự kiện nhấn vào item
            Debug.Log("Created item: " + name + " with quantity: " + quantity);
        }
        else
        {
            Debug.LogError("Không thể lấy BuyItem script từ prefab!");
        }
    }







    public void ShowItemDetails(string name, string info, int quantity, Sprite image)
    {
        // Tạo prefab chi tiết item
        var newItemDetail = Instantiate(itemDetailPrefab, itemDetailParent);
        var itemDetailScript = newItemDetail.GetComponent<ItemDetail>();

        if (itemDetailScript != null)
        {
            itemDetailScript.Setup(name, info, quantity, image);  // Truyền thông tin vào
        }
    }

















    public void UpdateUserWeapons()
    {
        var username = LoginUser.loginResponseModel.username; // Lấy username từ LoginUser
        StartCoroutine(GetUserWeapons(username));               // Gọi API
    }

    IEnumerator GetUserWeapons(string username)
    {
        var url = "http://localhost:3005/users/get-user-weapons?username=" + username;
        var request = UnityWebRequest.Get(url);
        request.SetRequestHeader("Content-Type", "application/json");

        yield return request.SendWebRequest();

        if (request.result != UnityWebRequest.Result.Success)
        {
            Debug.LogError("API Error: " + request.error);
        }
        else
        {
            var jsonString = request.downloadHandler.text;
            Debug.Log("Response: " + jsonString); // Debug response từ API

            var responseWeaponModel = JsonConvert.DeserializeObject<ResponseWeaponModel>(jsonString);
            if (responseWeaponModel.status == 1)
            {
                foreach (var weapon in responseWeaponModel.weapons)

                {
                    // Kiểm tra từng itemId và tạo item với các thông tin cụ thể
                    if (weapon.weaponId == "MV")
                        CreateWeapon(weapon.quantity, "Mũ Vàng", MVImage, "Hp+100");
                    else if (weapon.weaponId == "MD")
                        CreateWeapon(weapon.quantity, "Mũ Đỏ", MDImage, "HP+200");

                    else if (weapon.weaponId == "NxV")
                        CreateWeapon(weapon.quantity, "Nhẫn Xương Vàng", NxVImage, "ATK+100");
                    else if (weapon.weaponId == "NxD")
                        CreateWeapon(weapon.quantity, "Nhẫn Xương Đỏ", NxDImage, "ATK+200");

                    else if (weapon.weaponId == "SV")
                        CreateWeapon(weapon.quantity, "Sao Vàng", SVImage, "MP+100");
                    else if (weapon.weaponId == "SD")
                        CreateWeapon(weapon.quantity, "Sao Đỏ", SDImage, "MP+200");

                    else if (weapon.weaponId == "QV")
                        CreateWeapon(weapon.quantity, "Quần Vàng", QVImage, "DEF+100");
                    else if (weapon.weaponId == "QD")
                        CreateWeapon(weapon.quantity, "Quần Đỏ", QDImage, "DEF+200");


                    else if (weapon.weaponId == "AV")
                        CreateWeapon(weapon.quantity, "Áo Vàng", AVImage, "DEF+100");
                    else if (weapon.weaponId == "AD")
                        CreateWeapon(weapon.quantity, "Áo Đỏ", ADImage, "DEF+200");
                }
            }
            else
            {
                Debug.LogError("API Error: " + responseWeaponModel.message);
            }
        }
    }

    // Tạo item và thêm vào danh sách
    private void CreateWeapon(int quantity, string name, Sprite itemImage, string info)
    {
        var newItem = Instantiate(weaponPrefab, contentParentweapon);  // Tạo item từ prefab
        var buyItemScript = newItem.GetComponent<BuyItem>();

        if (buyItemScript != null)
        {
            buyItemScript.Setup1(name, info, quantity, itemImage);  // Truyền thông tin vào prefab
            newItem.GetComponent<Button>().onClick.AddListener(buyItemScript.OnweaponClick); // Đăng ký sự kiện nhấn vào item
            Debug.Log("Created item: " + name + " with quantity: " + quantity);
        }
        else
        {
            Debug.LogError("Không thể lấy BuyItem script từ prefab!");
        }
    }





    public void ShowweaponDetails(string name, string info, int quantity, Sprite image)
    {
        // Tạo prefab chi tiết item
        var newItemDetail = Instantiate(weaponDetailPrefab, itemDetailParent);
        var itemDetailScript = newItemDetail.GetComponent<ItemDetail>();

        if (itemDetailScript != null)
        {
            itemDetailScript.Setup1(name, info, quantity, image);  // Truyền thông tin vào
        }
    }













    // Hiển thị thông tin chi tiết khi nhấn vào item


    // Các lớp để xử lý dữ liệu API
    public class ResponseModel
    {
        public int status;
        public string message;
        public List<ItemData> items;
    }

    public class ItemData
    {
        public string itemId;
        public int quantity;
    }



    public class ResponseWeaponModel
    {
        public int status;
        public string message;
        public List<weaponData> weapons;
    }

    public class weaponData
    {
        public string weaponId;
        public int quantity;
    }
}
