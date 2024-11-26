using System.Collections;
using System.Collections.Generic;
using Newtonsoft.Json;
using TMPro;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

public class weaponmanager : MonoBehaviour
{
    public Transform contentParent; 
    public GameObject listweaponPrefab;

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

    public Image Skill1, Skill2, Skill3;    
    public Sprite KNdausia1, KNdausia2, KNdausia3;
    public Sprite KNdausib1, KNdausib2, KNdausib3;
    public Sprite KNcungthu1, KNcungthu2, KNcungthu3;
    public Sprite KNphapsu1, KNphapsu2, KNphapsu3;
    
    public TextMeshProUGUI nametuong , HP, MP, ATK, DEF;
    public TextMeshProUGUI HPthem, MPthem, ATKthem, DEFthem;
    public TextMeshProUGUI HPtong, MPtong, ATKtong, DEFtong;

    public Button dausia, dausib, cungthu, phapsu;
    public Image tbdausia1, tbdausia2, tbdausia3, tbdausia4, tbdausia5;   
    public Image tbdausib1, tbdausib2, tbdausib3, tbdausib4, tbdausib5; 
    public Image tbcungthu1, tbcungthu2, tbcungthu3, tbcungthu4, tbcungthu5; 
    public Image tbphapsu1, tbphapsu2, tbphapsu3, tbphapsu4, tbphapsu5;

    void Start()
    {
        UpdateWeapons();    

        dausia.onClick.AddListener(() => OnButtonClick("dausia"));
        dausib.onClick.AddListener(() => OnButtonClick("dausib"));
        cungthu.onClick.AddListener(() => OnButtonClick("cungthu"));
        phapsu.onClick.AddListener(() => OnButtonClick("phapsu")); 
    }

    public void UpdateWeapons()
    {
        var username = LoginUser.loginResponseModel.username; // Lấy username từ LoginUser
        StartCoroutine(GetUserWeapons(username));               // Gọi API
    }

    IEnumerator GetUserWeapons(string username)
    {
        var url = "http://localhost:3000/users/get-user-weapons?username=" + username;
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

    private void CreateWeapon(int quantity, string name, Sprite itemImage, string info)
    {
        var newItem = Instantiate(listweaponPrefab, contentParent);  // Tạo item từ prefab
        var listScript = newItem.GetComponent<listweapon>();

        if (listScript != null)
        {
            listScript.Setup(name, info, quantity, itemImage);  // Truyền thông tin vào prefab
        }
        else
        {
            Debug.LogError("Không thể lấy BuyItem script từ prefab!");
        }
    }

    void OnButtonClick(string buttonName)
{
    if (buttonName == "dausia")
    {
        SetCharacterStats("Đấu Sĩ A", 1000, 700, 500, 800, KNdausia1, KNdausia2, KNdausia3);
        SetCurrentIDForWeapons("dausia");  // Gửi currentID là "dausia"
    }
    else if (buttonName == "dausib")
    {
        SetCharacterStats("Đấu Sĩ B", 1200, 600, 550, 750, KNdausib1, KNdausib2, KNdausib3);
        SetCurrentIDForWeapons("dausib");  // Gửi currentID là "dausib"
    }
    else if (buttonName == "cungthu")
    {
        SetCharacterStats("Cung Thủ", 900, 1000, 600, 500, KNcungthu1, KNcungthu2, KNcungthu3);
        SetCurrentIDForWeapons("cungthu");  // Gửi currentID là "cungthu"
    }
    else if (buttonName == "phapsu")
    {
        SetCharacterStats("Pháp Sư", 800, 1000, 700, 500, KNphapsu1, KNphapsu2, KNphapsu3);
        SetCurrentIDForWeapons("phapsu");  // Gửi currentID là "phapsu"
    }
}

void SetCurrentIDForWeapons(string currentID)
{
    // Lấy tất cả các item vũ khí đã tạo từ prefab
    foreach (Transform child in contentParent)
    {
        var listWeaponScript = child.GetComponent<listweapon>();
        if (listWeaponScript != null)
        {
            listWeaponScript.SetCurrentID(currentID);  // Cập nhật currentID cho mỗi item vũ khí
        }
    }
}


  

    void SetCharacterStats(string name, int hp, int mp, int atk, int def, Sprite skill1, Sprite skill2, Sprite skill3)
    {
        // Cập nhật thông tin nhân vật
        nametuong.text = name;
        HP.text = hp.ToString();
        MP.text = mp.ToString();
        ATK.text = atk.ToString();
        DEF.text = def.ToString();

        // Cập nhật hình ảnh kỹ năng
        Skill1.sprite = skill1;
        Skill2.sprite = skill2;
        Skill3.sprite = skill3;
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
