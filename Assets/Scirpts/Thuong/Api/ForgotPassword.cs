using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Newtonsoft.Json;
using UnityEngine.Networking;
using System.Text;
using TMPro;

public class ForgotPassword : MonoBehaviour
{
    public TMP_InputField txtUser;
    public TMP_Text txtError;

    // Start is called before the first frame update
    void Start()
    {
        txtError.text = ""; // Đặt lại thông báo lỗi khi khởi động
    }

    // Update is called once per frame
    void Update() { }

    public void SendOTP()
    {
        var user = txtUser.text;
        OTPModel oTPModel = new OTPModel(user);
        StartCoroutine(SendOTPAPI(oTPModel));
    }

    IEnumerator SendOTPAPI(OTPModel oTPModel)
    {
        string jsonStringRequest = JsonConvert.SerializeObject(oTPModel);

        using (var request = new UnityWebRequest("http://localhost:3000/users/send-mail", "POST"))
        {
            byte[] bodyRaw = Encoding.UTF8.GetBytes(jsonStringRequest);
            request.uploadHandler = new UploadHandlerRaw(bodyRaw);
            request.downloadHandler = new DownloadHandlerBuffer();
            request.SetRequestHeader("Content-Type", "application/json");
            yield return request.SendWebRequest();

            if (request.result != UnityWebRequest.Result.Success)
            {
                Debug.Log(request.error);
                txtError.text = "Lỗi khi kết nối đến máy chủ."; // Lỗi nếu không kết nối được server
            }
            else
            {
                var jsonString = request.downloadHandler.text;
                LoginReponseModel loginResponseModel = JsonConvert.DeserializeObject<LoginReponseModel>(jsonString);

                if (loginResponseModel.status == 1)
                {
                    txtError.text = "Mã OTP đã được gửi thành công!";
                }
                else
                {
                    txtError.text = loginResponseModel.message; // Hiển thị thông báo lỗi từ server
                }
            }
        }
    }

}
