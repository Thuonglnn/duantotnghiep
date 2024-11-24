using System.Collections;
using System.Collections.Generic;
using System.Text;
using Newtonsoft.Json;
using TMPro;
using UnityEditorInternal.Profiling.Memory.Experimental;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Shoptuong : MonoBehaviour
{
    // Các GameObject cho trạng thái "Mua" và "Đã sở hữu" cho từng tướng
    public GameObject BuydausiA0, BuydausiA1;
    public GameObject OwendausiA0, OwendausiA1;
    public GameObject BuycungthuB0;
    public GameObject OwencungthuB0;
    public GameObject BuyphapsuC0;
    public GameObject OwenphapsuC0;

    public GameObject BangdausiA0, BangdausiA1, BangcungthuB0, BangphapsuC0;

    public TextMeshProUGUI txtcointong;
    public TextMeshProUGUI txtdiamondtong;
     public TextMeshProUGUI txterror;
    public GameObject pnerror; 
    private int coin;
    private int diamond;

    private int coindausiA0, coindausiA1, coincungthuB0, coinphapsuC0;
    private int kcdausiA0, kcdausiA1, kccungthuB0, kcphapsuC0;

    private string generalId, itemId, weaponId;
    




   //item
  
    public GameObject BangHPnho, BangHPtb, BangHPto,BangMPnho, BangMPtb,BangMPto ;
     private int coinHPnho, coinHPtb, coinHPto, coinMPnho, coinMPtb, coinMPto;
    private int kcHPnho, kcHPtb, kcHPto, kcMPnho, kcMPtb, kcMPto;
      
   
   //weapon
       public GameObject BangMuVang, BangMuDo, BangNxVang, BangNxDo, BangSaoVang, BangSaoDo, BangQuanVang, BangQuanDo, BangAoVang, BangAoDo;
     private int coinMuVang, coinMuDo, coinNxVang, coinNxDo, coinSaoVang, coinSaoDo, coinQuanVang, coinQuanDo, coinAoVang, coinAoDo;
    private int kcMuVang, kcMuDo, kcNxVang, kcNxDo, kcSaoVang, kcSaoDo, kcQuanVang, kcQuanDo, kcAoVang, kcAoDo;






 


    void Start()
    {
          
         UnClock();
     updateCoin();
     UpdateDiamond();
    }



 public void updateCoin()
{
    var username = LoginUser.loginResponseModel.username; // Lấy tên người dùng từ thông tin đăng nhập
    StartCoroutine(GetCoin(username)); // Gọi coroutine với tên người dùng
}

IEnumerator GetCoin(string username)
{
    // Tạo yêu cầu GET đến API lấy điểm số
    var request = new UnityWebRequest("http://localhost:3000/users/get-score?username=" + username, "GET");
    request.downloadHandler = new DownloadHandlerBuffer();
    
    request.SetRequestHeader("Content-Type", "application/json");

    yield return request.SendWebRequest(); // Gửi yêu cầu

    if (request.result != UnityWebRequest.Result.Success)
    {
        coin = LoginUser.loginResponseModel.score;
        txtcointong.text = coin.ToString();
        Debug.Log(request.error); // Log lỗi nếu có
    }
    else
    {
        var jsonString = request.downloadHandler.text.ToString();
        LoginReponseModel loginResponseModel = JsonConvert.DeserializeObject<LoginReponseModel>(jsonString);

        if (loginResponseModel.status == 1)
        {
            // Cập nhật số điểm và hiển thị lên giao diện
            coin = loginResponseModel.score; // Giả sử bạn đã sửa `LoginReponseModel` để bao gồm điểm số
            txtcointong.text = coin.ToString();
            Debug.Log(loginResponseModel.message);
        }
        else
        {
            Debug.Log("Error retrieving score: " + loginResponseModel.message); // In ra thông báo lỗi
        }
    }
}




public void UpdateDiamond()
{
    var username = LoginUser.loginResponseModel.username; // Lấy tên người dùng từ thông tin đăng nhập
    StartCoroutine(Getdiamond(username)); // Gọi coroutine với tên người dùng
}

IEnumerator Getdiamond(string username)
{
    // Tạo yêu cầu GET đến API lấy điểm số
    var request = new UnityWebRequest("http://localhost:3000/users/get-diamond?username=" + username, "GET");
    request.downloadHandler = new DownloadHandlerBuffer();
    
    request.SetRequestHeader("Content-Type", "application/json");

    yield return request.SendWebRequest(); // Gửi yêu cầu

    if (request.result != UnityWebRequest.Result.Success)
    {
        diamond = LoginUser.loginResponseModel.diamond;
        txtdiamondtong.text = diamond.ToString();
        Debug.Log(request.error); // Log lỗi nếu có
    }
    else
    {
        var jsonString = request.downloadHandler.text.ToString();
        LoginReponseModel loginResponseModel = JsonConvert.DeserializeObject<LoginReponseModel>(jsonString);

        if (loginResponseModel.status == 1)
        {
            // Cập nhật số điểm và hiển thị lên giao diện
            diamond = loginResponseModel.diamond; // Giả sử bạn đã sửa `LoginReponseModel` để bao gồm điểm số
            txtdiamondtong.text = diamond.ToString();
            Debug.Log(loginResponseModel.message);
        }
        else
        {
            Debug.Log("Error retrieving diamond: " + loginResponseModel.message); // In ra thông báo lỗi
        }
    }
}














    

    public void UnClock()
    {
        // Gọi hàm GetGeneralIds với username
        var username = LoginUser.loginResponseModel.username;
        StartCoroutine(GetGeneralIds(username));
    }

    IEnumerator GetGeneralIds(string username)
    {
        // Địa chỉ URL cho yêu cầu
        var request = new UnityWebRequest("http://localhost:3000/users/get-general-id?username=" + username, "GET");
        request.downloadHandler = new DownloadHandlerBuffer();
        request.SetRequestHeader("Content-Type", "application/json");

        // Gửi yêu cầu và chờ kết quả
        yield return request.SendWebRequest();

        if (request.result != UnityWebRequest.Result.Success)
        {
            // In ra lỗi nếu có
            Debug.Log(request.error);
        }
        else
        {
            // Xử lý dữ liệu trả về
            var jsonString = request.downloadHandler.text;
            var responseModel = JsonConvert.DeserializeObject<LoginReponseModel>(jsonString);

            // In ra tất cả generalIds
            Debug.Log("General IDs: " + string.Join(", ", responseModel.generalIds));

            if (responseModel.status == 1)
            {
                // Mở khóa tướng dựa trên generalIds
                if (responseModel.generalIds.Contains("a123"))
                {
                    UnlockCharacter(BuydausiA0, OwendausiA0); // Mở khóa tướng A
                }
                if (responseModel.generalIds.Contains("a124"))
                {
                    UnlockCharacter(BuydausiA1, OwendausiA1); // Mở khóa tướng A1
                }
                if (responseModel.generalIds.Contains("b123"))
                {
                    UnlockCharacter(BuycungthuB0, OwencungthuB0); // Mở khóa cung thủ B
                }
                if (responseModel.generalIds.Contains("c123"))
                {
                    UnlockCharacter(BuyphapsuC0, OwenphapsuC0); // Mở khóa pháp sư C
                }
            }
            else
            {
                Debug.Log("Error fetching general IDs: " + responseModel.message); // In ra thông báo lỗi
            }
        }
    }

    // Hàm mở khóa tướng, tắt biểu tượng "Mua" và mở biểu tượng "Đã sở hữu"
    private void UnlockCharacter(GameObject buyIcon, GameObject ownIcon)
    {
        if (buyIcon != null && ownIcon != null)
        {
            buyIcon.SetActive(false);  // Ẩn biểu tượng mua
            ownIcon.SetActive(true);   // Hiển thị biểu tượng đã sở hữu
        }
    }


















// mua tướng với coin

  public void muaTuong(int giaTuong, GameObject bangThongTin, string idTuong)
{
    if (coin >= giaTuong) // Kiểm tra nếu coin đủ để mua
    {
        bangThongTin.SetActive(false); // Ẩn bảng thông tin
        generalId = idTuong; // Gán ID của tướng
        coin -= giaTuong; // Trừ coin

   

        buyshoptuong(); // Gọi phương thức để mua tướng
          txterror.text = ""; // Xóa thông báo lỗi
    }
    else
    {
         pnerror.SetActive(true);
         bangThongTin.SetActive(false);
        txterror.text = "Không đủ coin để mua tướng";
    }
}

// Sử dụng phương thức chung với các giá trị cụ thể cho từng tướng
public void muacoindausiA0()
{
    muaTuong(100, BangdausiA0, "a123");
}

public void muacoindausiA1()
{
    muaTuong(500, BangdausiA1, "a124");
}

public void muacoincungthuB0()
{
    muaTuong(100, BangcungthuB0, "b123");
}

public void muacoinphapsuC0()
{
    muaTuong(100, BangphapsuC0, "c123");
}




//mua tướng với diamond

   public void muaTieuKiem(string idTuong, GameObject bangThongTin, int kc)
{
    if (diamond >= kc) // Kiểm tra nếu diamond đủ để mua
    {
        bangThongTin.SetActive(false); // Ẩn bảng thông tin
        generalId = idTuong; // Gán ID của tướng
        diamond -= kc; // Trừ diamond
        buyshoptuong(); // Gọi phương thức để mua tướng
        txterror.text = ""; // Xóa thông báo lỗi
    }
    else
    {
        pnerror.SetActive(true); // Hiển thị thông báo lỗi nếu diamond không đủ
        bangThongTin.SetActive(false); 
        txterror.text = "Không đủ diamond để mua tướng";
    }
}

public void muakcdausiA0()
{
    muaTieuKiem("a123", BangdausiA0, 40);
}

public void muakcdausiA1()
{
    muaTieuKiem("a124", BangdausiA1, 200);
}

public void muakccungthuB0()
{
    muaTieuKiem("b123", BangcungthuB0, 40);
}

public void muakcphapsuC0()
{
    muaTieuKiem("c123", BangphapsuC0, 40);
}

    















// mua item
//coin
 public void muaItem(string itemId, int itemCost, GameObject itemPanel)
{
    if (coin >= itemCost) // Kiểm tra nếu đủ coin để mua item
    {
        coin -= itemCost; // Trừ coin
        itemPanel.SetActive(false); // Ẩn bảng thông tin
        this.itemId = itemId; // Cập nhật itemId
        buyshopitem(); // Gọi phương thức mua item
         txterror.text = ""; // Xóa thông báo lỗi
    }
    else
    {
         pnerror.SetActive(true); // Hiển thị thông báo lỗi nếu diamond không đủ
         itemPanel.SetActive(false); 
         txterror.text = "Không đủ coin để mua vật phẩm";
    }
}

public void muacoinHPnho()
{
    muaItem("HP1", 100, BangHPnho); // Gọi phương thức chung với các tham số tương ứng
}

public void muacoinHPtb()
{
    muaItem("HP2", 200, BangHPtb); // Gọi phương thức chung với các tham số tương ứng
}

public void muacoinHPto()
{
    muaItem("HP3", 300, BangHPto); // Gọi phương thức chung với các tham số tương ứng
}

public void muacoinMPnho()
{
    muaItem("MP1", 100, BangMPnho); // Gọi phương thức chung với các tham số tương ứng
}

public void muacoinMPtb()
{
    muaItem("MP2", 200, BangMPtb); // Gọi phương thức chung với các tham số tương ứng
}

public void muacoinMPto()
{
    muaItem("MP3", 300, BangMPto); // Gọi phương thức chung với các tham số tương ứng
}











//kc

public void muaItemKC(string itemId, int itemCost, GameObject itemPanel)
{
    if (diamond >= itemCost) // Kiểm tra nếu đủ diamond để mua item
    {
        diamond -= itemCost; // Trừ diamond
        itemPanel.SetActive(false); // Ẩn bảng thông tin
        this.itemId = itemId; // Cập nhật itemId
        buyshopitem(); // Gọi phương thức mua item
          txterror.text = ""; // Xóa thông báo lỗi
    }
    else
    {
         pnerror.SetActive(true); // Hiển thị thông báo lỗi nếu diamond không đủ
         itemPanel.SetActive(false);
        txterror.text = "Không đủ diamond để mua vật phẩm";
    }
}
public void muakcHPnho()
{
    muaItemKC("HP1", 40, BangHPnho); // Gọi phương thức chung với các tham số tương ứng
}

public void muakcHPtb()
{
    muaItemKC("HP2", 80, BangHPtb); // Gọi phương thức chung với các tham số tương ứng
}

public void muakcHPto()
{
    muaItemKC("HP3", 120, BangHPto); // Gọi phương thức chung với các tham số tương ứng
}

public void muakcMPnho()
{
    muaItemKC("MP1", 40, BangMPnho); // Gọi phương thức chung với các tham số tương ứng
}

public void muakcMPtb()
{
    muaItemKC("MP2", 80, BangMPtb); // Gọi phương thức chung với các tham số tương ứng
}

public void muakcMPto()
{
    muaItemKC("MP3", 120, BangMPto); // Gọi phương thức chung với các tham số tương ứng
}
























// mua weapon
//coin
public void muaWeapon(string weaponId, int weaponCost, GameObject weaponPanel)
{
    if (coin >= weaponCost) // Kiểm tra nếu đủ coin để mua vũ khí
    {
        coin -= weaponCost; // Trừ coin
        weaponPanel.SetActive(false); // Ẩn bảng thông tin
        this.weaponId = weaponId; // Cập nhật weaponId
        buyshopweapon(); // Gọi phương thức mua vũ khí
         txterror.text = ""; // Xóa thông báo lỗi
    }
    else
    {
          pnerror.SetActive(true); // Hiển thị thông báo lỗi nếu diamond không đủ
          weaponPanel.SetActive(false);
        txterror.text = "Không đủ coin để mua trang bị";
    }
}
public void muacoinMuVang()
{
    muaWeapon("MV", 200, BangMuVang); // Gọi phương thức chung với các tham số tương ứng
}

public void muacoinMuDo()
{
    muaWeapon("MD", 400, BangMuDo); // Gọi phương thức chung với các tham số tương ứng
}

public void muacoinNxVang()
{
    muaWeapon("NxV", 400, BangNxVang); // Gọi phương thức chung với các tham số tương ứng
}

public void muacoinNxDo()
{
    muaWeapon("NxD", 800, BangNxDo); // Gọi phương thức chung với các tham số tương ứng
}

public void muacoinSaoVang()
{
    muaWeapon("SV", 400, BangSaoVang); // Gọi phương thức chung với các tham số tương ứng
}

public void muacoinSaoDo()
{
    muaWeapon("SD", 800, BangSaoDo); // Gọi phương thức chung với các tham số tương ứng
}

public void muacoinQuanVang()
{
    muaWeapon("QV", 200, BangQuanVang); // Gọi phương thức chung với các tham số tương ứng
}

public void muacoinQuanDo()
{
    muaWeapon("QD", 400, BangQuanDo); // Gọi phương thức chung với các tham số tương ứng
}

public void muacoinAoVang()
{
    muaWeapon("AV", 200, BangAoVang); // Gọi phương thức chung với các tham số tương ứng
}

public void muacoinAoDo()
{
    muaWeapon("AD", 400, BangAoDo); // Gọi phương thức chung với các tham số tương ứng
}


















//kc

 public void muaWeaponWithDiamond(string weaponId, int diamondCost, GameObject weaponPanel)
{
    if (diamond >= diamondCost) // Kiểm tra nếu đủ diamond để mua vũ khí
    {
        diamond -= diamondCost; // Trừ diamond
        weaponPanel.SetActive(false); // Ẩn bảng thông tin vũ khí
        this.weaponId = weaponId; // Cập nhật weaponId
        buyshopweapon(); // Gọi phương thức mua vũ khí
         txterror.text = ""; // Xóa thông báo lỗi
    }
    else
    {
          pnerror.SetActive(true); // Hiển thị thông báo lỗi nếu diamond không đủ
          weaponPanel.SetActive(false); // Ẩn bảng thông tin vũ khí
        txterror.text = "Không đủ diamond để mua trang bị";
    }
}
public void muakcMuVang()
{
    muaWeaponWithDiamond("MV", 80, BangMuVang); // Gọi phương thức chung với các tham số tương ứng
}

public void muakcMuDo()
{
    muaWeaponWithDiamond("MD", 160, BangMuDo); // Gọi phương thức chung với các tham số tương ứng
}

public void muakcNxVang()
{
    muaWeaponWithDiamond("NxV", 160, BangNxVang); // Gọi phương thức chung với các tham số tương ứng
}

public void muakcNxDo()
{
    muaWeaponWithDiamond("NxD", 320, BangNxDo); // Gọi phương thức chung với các tham số tương ứng
}

public void muakcSaoVang()
{
    muaWeaponWithDiamond("SV", 160, BangSaoVang); // Gọi phương thức chung với các tham số tương ứng
}

public void muakcSaoDo()
{
    muaWeaponWithDiamond("SD", 320, BangSaoDo); // Gọi phương thức chung với các tham số tương ứng
}

public void muakcQuanVang()
{
    muaWeaponWithDiamond("QV", 80, BangQuanVang); // Gọi phương thức chung với các tham số tương ứng
}

public void muakcQuanDo()
{
    muaWeaponWithDiamond("QD", 160, BangQuanDo); // Gọi phương thức chung với các tham số tương ứng
}

public void muakcAoVang()
{
    muaWeaponWithDiamond("AV", 80, BangAoVang); // Gọi phương thức chung với các tham số tương ứng
}

public void muakcAoDo()
{
    muaWeaponWithDiamond("AD", 160, BangAoDo); // Gọi phương thức chung với các tham số tương ứng
}

























   
public void buyshoptuong()
{   
    var username = LoginUser.loginResponseModel.username;
    ShoptuongModel shoptuongModel = new ShoptuongModel(username, generalId, coin, diamond);

    // Trừ số coin/diamond và cập nhật lên UI ngay lập tức
    txtcointong.text = coin.ToString();
    txtdiamondtong.text = diamond.ToString();

    // Bắt đầu coroutine để lưu thông tin mua tướng
    StartCoroutine(Shoptuong1(shoptuongModel));
}

IEnumerator Shoptuong1(ShoptuongModel shoptuongModel)
{
    string jsonStringRequest = JsonConvert.SerializeObject(shoptuongModel);
    var request = new UnityWebRequest("http://localhost:3000/users/buy-general", "POST");
    byte[] bodyRaw = Encoding.UTF8.GetBytes(jsonStringRequest);
    request.uploadHandler = new UploadHandlerRaw(bodyRaw);
    request.downloadHandler = new DownloadHandlerBuffer();
    request.SetRequestHeader("Content-Type", "application/json");
    yield return request.SendWebRequest();

    if (request.result != UnityWebRequest.Result.Success)
    {
        Debug.Log(request.error);
    }
    else
    {
        var jsonString = request.downloadHandler.text;
        var loginResponseModel = JsonConvert.DeserializeObject<LoginReponseModel>(jsonString);

        if (loginResponseModel.status == 1)
        {
            Debug.Log(loginResponseModel.message);
            // Cập nhật lại từ server để đảm bảo dữ liệu chính xác
            luuscore();
            luudiamond();
            UnClock();
        }
        else
        {
            Debug.Log("Error saving general: " + loginResponseModel.message);
        }
    }
}



public void luuscore()
{   
    var username = LoginUser.loginResponseModel.username;
    ScoreModel scoreModel = new ScoreModel(username,coin);
    StartCoroutine(postscore(scoreModel));
}

IEnumerator postscore(ScoreModel scoreModel)
{
    string jsonStringRequest = JsonConvert.SerializeObject(scoreModel);
    var request = new UnityWebRequest("http://localhost:3000/users/update-score", "POST");
    byte[] bodyRaw = Encoding.UTF8.GetBytes(jsonStringRequest);
    request.uploadHandler = new UploadHandlerRaw(bodyRaw);
    request.downloadHandler = new DownloadHandlerBuffer();
    request.SetRequestHeader("Content-Type", "application/json");
    yield return request.SendWebRequest();

    if (request.result != UnityWebRequest.Result.Success)
    {
        Debug.Log(request.error);
    }
    else
    {
        var jsonString = request.downloadHandler.text;
        var loginResponseModel = JsonConvert.DeserializeObject<LoginReponseModel>(jsonString);

        if (loginResponseModel.status == 1)
        {
            Debug.Log(loginResponseModel.message);
            // Cập nhật lại từ server để đảm bảo dữ liệu chính xác
         
            
        }
        else
        {
            Debug.Log("Error saving general: " + loginResponseModel.message);
        }
    }
}




public void luudiamond()
{   
    var username = LoginUser.loginResponseModel.username;
    DiamondModel diamondModel = new DiamondModel(username,diamond);
    StartCoroutine(postdiamond(diamondModel));
}

IEnumerator postdiamond(DiamondModel diamondModel)
{
    string jsonStringRequest = JsonConvert.SerializeObject(diamondModel);
    var request = new UnityWebRequest("http://localhost:3000/users/update-diamond", "POST");
    byte[] bodyRaw = Encoding.UTF8.GetBytes(jsonStringRequest);
    request.uploadHandler = new UploadHandlerRaw(bodyRaw);
    request.downloadHandler = new DownloadHandlerBuffer();
    request.SetRequestHeader("Content-Type", "application/json");
    yield return request.SendWebRequest();

    if (request.result != UnityWebRequest.Result.Success)
    {
        Debug.Log(request.error);
    }
    else
    {
        var jsonString = request.downloadHandler.text;
        var loginResponseModel = JsonConvert.DeserializeObject<LoginReponseModel>(jsonString);

        if (loginResponseModel.status == 1)
        {
            Debug.Log(loginResponseModel.message);
            // Cập nhật lại từ server để đảm bảo dữ liệu chính xác
         
            
        }
        else
        {
            Debug.Log("Error saving general: " + loginResponseModel.message);
        }
    }
}




























public void buyshopitem()
{   
    var username = LoginUser.loginResponseModel.username;
    ShopitemModel shopitemModel = new ShopitemModel(username, itemId, coin, diamond);

    // Trừ số coin/diamond và cập nhật lên UI ngay lập tức
    txtcointong.text = coin.ToString();
    txtdiamondtong.text = diamond.ToString();

    // Bắt đầu coroutine để lưu thông tin mua tướng
    StartCoroutine(Shopitem1(shopitemModel));
}

IEnumerator Shopitem1(ShopitemModel shopitemModel)
{
    string jsonStringRequest = JsonConvert.SerializeObject(shopitemModel);
    var request = new UnityWebRequest("http://localhost:3000/users/buy-item", "POST");
    byte[] bodyRaw = Encoding.UTF8.GetBytes(jsonStringRequest);
    request.uploadHandler = new UploadHandlerRaw(bodyRaw);
    request.downloadHandler = new DownloadHandlerBuffer();
    request.SetRequestHeader("Content-Type", "application/json");
    yield return request.SendWebRequest();

    if (request.result != UnityWebRequest.Result.Success)
    {
        Debug.Log(request.error);
    }
    else
    {
        var jsonString = request.downloadHandler.text;
        var loginResponseModel = JsonConvert.DeserializeObject<LoginReponseModel>(jsonString);

        if (loginResponseModel.status == 1)
        {
            Debug.Log(loginResponseModel.message);
            // Cập nhật lại từ server để đảm bảo dữ liệu chính xác
            luuscore();
            luudiamond();
          
        }
        else
        {
            Debug.Log("Error saving general: " + loginResponseModel.message);
        }
    }
}








public void buyshopweapon()
{   
    var username = LoginUser.loginResponseModel.username;
    ShopweaponModel shopweaponModel = new ShopweaponModel(username, weaponId, coin, diamond);

    // Trừ số coin/diamond và cập nhật lên UI ngay lập tức
    txtcointong.text = coin.ToString();
    txtdiamondtong.text = diamond.ToString();

    // Bắt đầu coroutine để lưu thông tin mua tướng
    StartCoroutine(Shopweapon1(shopweaponModel));
}

IEnumerator Shopweapon1(ShopweaponModel shopweaponModel)
{
    string jsonStringRequest = JsonConvert.SerializeObject(shopweaponModel);
    var request = new UnityWebRequest("http://localhost:3000/users/buy-weapon", "POST");
    byte[] bodyRaw = Encoding.UTF8.GetBytes(jsonStringRequest);
    request.uploadHandler = new UploadHandlerRaw(bodyRaw);
    request.downloadHandler = new DownloadHandlerBuffer();
    request.SetRequestHeader("Content-Type", "application/json");
    yield return request.SendWebRequest();

    if (request.result != UnityWebRequest.Result.Success)
    {
        Debug.Log(request.error);
    }
    else
    {
        var jsonString = request.downloadHandler.text;
        var loginResponseModel = JsonConvert.DeserializeObject<LoginReponseModel>(jsonString);

        if (loginResponseModel.status == 1)
        {
            Debug.Log(loginResponseModel.message);
            // Cập nhật lại từ server để đảm bảo dữ liệu chính xác
            luuscore();
            luudiamond();
          
        }
        else
        {
            Debug.Log("Error saving general: " + loginResponseModel.message);
        }
    }
}
}
