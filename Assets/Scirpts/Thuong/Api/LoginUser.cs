using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEngine.Networking;
using System.Text;
using Newtonsoft.Json;





public class LoginUser : MonoBehaviour
{
    public TMP_InputField edtUser, edtPass;
    public TMP_Text txtError;

    public Selectable first;
    private EventSystem enventsytem;

    public static LoginReponseModel loginResponseModel; // Đảm bảo đây là tĩnh

    // Start is called before the first frame update
    void Start()
    {
        enventsytem = EventSystem.current;
        first.Select();

    }

    // Update is called once per frame
    void Update() { }
    public void checklogin()
    {
        var username = edtUser.text;
        var pass = edtPass.text;

        UserModel userModel = new UserModel(username, pass);
        StartCoroutine(Login(userModel));
        Login(userModel);
    }

    IEnumerator Login(UserModel userModel)
    {
        string jsonStringRequest = JsonConvert.SerializeObject(userModel);

        var request = new UnityWebRequest("http://localhost:3005/users/login", "POST");
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
            loginResponseModel = JsonConvert.DeserializeObject<LoginReponseModel>(jsonString); // Sửa ở đây

            if (loginResponseModel.status == 20)
            {
                SceneManager.LoadScene("HeroInfo");
            }
            else if (loginResponseModel.status == 10)
            {
                SceneManager.LoadScene("Home");
            }
            else
            {
                txtError.text = loginResponseModel.message;
            }
        }
    }






}


