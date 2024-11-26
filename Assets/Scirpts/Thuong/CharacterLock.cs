using System.Collections;
using System.Collections.Generic;
using Newtonsoft.Json;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

public class CharacterLock : MonoBehaviour
{
    public Image lockIconA; // Biểu tượng khóa cho tướng A
    public Image lockIconA1; // Biểu tượng khóa cho tướng B
    public Image lockIconB;
    public Image lockIconC;





    void Start()
    {
        // Hiển thị tất cả biểu tượng khóa
        lockIconA.gameObject.SetActive(true);
        lockIconA1.gameObject.SetActive(true);
        lockIconB.gameObject.SetActive(true);
        lockIconC.gameObject.SetActive(true);

        UnClock();
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
        var request = new UnityWebRequest("http://localhost:3005/users/get-general-id?username=" + username, "GET");
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
                    UnlockCharacter(lockIconA); // Mở khóa tướng A
                }
                if (responseModel.generalIds.Contains("a124"))
                {
                    UnlockCharacter(lockIconA1); // Mở khóa tướng B
                }
                if (responseModel.generalIds.Contains("b123"))
                {
                    UnlockCharacter(lockIconB);
                }
                if (responseModel.generalIds.Contains("c123"))
                {
                    UnlockCharacter(lockIconC);
                }
            }
            else
            {
                Debug.Log("Error fetching general IDs: " + responseModel.message); // In ra thông báo lỗi
            }
        }
    }

    // Hàm để mở khóa nhân vật
    private void UnlockCharacter(Image lockIcon)
    {
        if (lockIcon != null)
        {
            lockIcon.gameObject.SetActive(false); // Ẩn biểu tượng khóa
        }
    }
}
