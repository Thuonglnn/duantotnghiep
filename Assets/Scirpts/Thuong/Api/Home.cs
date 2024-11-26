using System.Collections;
using System.Collections.Generic;
using Newtonsoft.Json;
using TMPro;
using UnityEngine;
using UnityEngine.Networking;

public class Home : MonoBehaviour
{
    public TMP_Text txtnickname;
    void Start()
    {
        updatename();
    }

    public void updatename()
    {
        var username = LoginUser.loginResponseModel.username; // Lấy tên người dùng từ thông tin đăng nhập
        StartCoroutine(Getname(username)); // Gọi coroutine với tên người dùng
    }

    IEnumerator Getname(string username)
    {
        // Tạo yêu cầu GET đến API lấy tên người dùng
        var request = new UnityWebRequest("http://localhost:3005/users/getName?username=" + username, "GET");
        request.downloadHandler = new DownloadHandlerBuffer();

        request.SetRequestHeader("Content-Type", "application/json");

        yield return request.SendWebRequest(); // Gửi yêu cầu

        if (request.result != UnityWebRequest.Result.Success)
        {
            Debug.Log(request.error); // Log lỗi nếu có
        }
        else
        {
            var jsonString = request.downloadHandler.text.ToString();
            LoginReponseModel loginResponseModel = JsonConvert.DeserializeObject<LoginReponseModel>(jsonString);

            if (loginResponseModel.status == 1)
            {
                // Cập nhật nickname và hiển thị lên giao diện
                txtnickname.text = loginResponseModel.name; // Cập nhật nickname từ API
                Debug.Log("Nickname updated: " + loginResponseModel.name);
            }
            else
            {
                Debug.Log("Error retrieving name: " + loginResponseModel.message); // In ra thông báo lỗi
            }
        }
    }
}
