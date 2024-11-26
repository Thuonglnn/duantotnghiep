using System.Collections;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEngine.Networking;
using System.Text;
using Newtonsoft.Json;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

public class RegisterUser : MonoBehaviour
{
    public TMP_InputField edtnickname,edtUser, edtPass, edtRepass;
    public TMP_Text txtError;
    public Selectable first;
    private EventSystem enventsytem;

    // Start is called before the first frame update
    void Start()
    {
        enventsytem = EventSystem.current;
        first.Select();
    }

    // Update is called once per frame
    void Update() {
        
     }

    public void checkregister()
    {   var name = edtnickname.text;
        var user = edtUser.text;
        var pass = edtPass.text;
        var repass = edtRepass.text;

        // Kiểm tra xem mật khẩu và mật khẩu nhập lại có khớp không
        if (pass != repass)
        {
            txtError.text = "Mật khẩu và mật khẩu nhập lại không khớp!";
            return;
        }

        RegisterModel userModel = new RegisterModel(user,name, pass);
        StartCoroutine(Register(userModel));
    }

    IEnumerator Register(RegisterModel userModel)
    {
        string jsonStringRequest = JsonConvert.SerializeObject(userModel);

        var request = new UnityWebRequest("http://localhost:3000/users/register", "POST");
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
            var jsonString = request.downloadHandler.text.ToString();
            LoginReponseModel loginResponseModel = JsonConvert.DeserializeObject<LoginReponseModel>(jsonString);

            if (loginResponseModel.status == 1)
            {
                SceneManager.LoadScene("DangNhap");
            }
            else
            {
                txtError.text = loginResponseModel.message;
            }
        }
    }
}
