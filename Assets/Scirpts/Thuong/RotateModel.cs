using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking; // Thêm thư viện cho UnityWebRequest
using UnityEngine.SceneManagement; // Thêm thư viện cho SceneManager
using UnityEngine.UI; // Thêm thư viện UI để sử dụng Text
using Newtonsoft.Json;
using System.Text;

public class RotateModel : MonoBehaviour
{
    public List<Transform> modelTransforms;  // Danh sách các Transform của mô hình
    private bool isRotate;                   // Cờ kiểm tra trạng thái xoay
    private Vector3 startPoint;              // Điểm bắt đầu khi nhấn chuột
    private Vector3 startAngel;              // Góc quay ban đầu của mô hình
    [Range(0.1f, 1f)]
    public float rotateScale = 1f;           // Tốc độ xoay
    private int currentModelIndex = 0;       // Mô hình hiện tại trong danh sách

    // Các panel cho thông tin tướng
    public GameObject panel1;                 // Panel cho mô hình 1
    public GameObject panel2;                 // Panel cho mô hình 2
    public GameObject panel3;                 // Panel cho mô hình 3

    private string generalId;

    void Start()
    {
        // Ẩn tất cả các panel khi bắt đầu
        panel1.SetActive(false);
        panel2.SetActive(false);
        panel3.SetActive(false);

        // Chọn mô hình đầu tiên và cập nhật panel
        SelectModel1(); // Gọi SelectModel1 để hiển thị mô hình 1 và panel 1
    }

    void Update()
    {
        if (modelTransforms.Count == 0) return;  // Kiểm tra nếu không có mô hình nào

        // Bắt đầu xoay khi nhấn chuột trái
        if (Input.GetMouseButtonDown(0) && !isRotate)
        {
            isRotate = true;
            startPoint = Input.mousePosition;  // Lấy vị trí chuột khi bắt đầu nhấn
            startAngel = modelTransforms[currentModelIndex].eulerAngles;  // Lưu lại góc ban đầu của mô hình
        }

        // Dừng xoay khi thả chuột trái
        if (Input.GetMouseButtonUp(0))
        {
            isRotate = false;
        }

        // Xử lý xoay mô hình khi giữ chuột trái
        if (isRotate)
        {
            var currentPoint = Input.mousePosition;  // Lấy vị trí hiện tại của chuột
            var x = startPoint.x - currentPoint.x;   // Tính toán khoảng cách di chuyển chuột
            modelTransforms[currentModelIndex].eulerAngles = startAngel + new Vector3(0, x * rotateScale, 0);  // Xoay theo trục Y
        }
    }

    // Chọn mô hình 1
    public void SelectModel1()
    {
        currentModelIndex = 0; // Chỉ định chỉ số của mô hình 1
        UpdateActiveModel();
        generalId = "a123";
    }

    // Chọn mô hình 2
    public void SelectModel2()
    {
        currentModelIndex = 1; // Chỉ định chỉ số của mô hình 2
        generalId = "b123";
        UpdateActiveModel();
    }

    // Chọn mô hình 3
    public void SelectModel3()
    {
        currentModelIndex = 2; // Chỉ định chỉ số của mô hình 3
        generalId = "c123";
        UpdateActiveModel();
    }

    public void checkgeneral()
    {
        var username = LoginUser.loginResponseModel.username;
        GeneralModel generalModel = new GeneralModel(username, generalId);
        StartCoroutine(General(generalModel));
    }

    IEnumerator General(GeneralModel generalModel)
    {
        string jsonStringRequest = JsonConvert.SerializeObject(generalModel);

        var request = new UnityWebRequest("http://localhost:3000/users/add-general", "POST");
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
            LoginReponseModel loginReponseModel = JsonConvert.DeserializeObject<LoginReponseModel>(jsonString);

            if (loginReponseModel.status == 1)
            {
                SceneManager.LoadScene("Home");
            }
            else
            {
                Debug.Log("Error saving general: " + loginReponseModel.message); // In ra thông báo lỗi
            }
        }
    }

    private void UpdateActiveModel()
    {
        for (int i = 0; i < modelTransforms.Count; i++)
        {
            modelTransforms[i].gameObject.SetActive(i == currentModelIndex); // Chỉ giữ mô hình hiện tại hoạt động
        }

        // Ẩn hoặc hiện các panel tương ứng với mô hình hiện tại
        panel1.SetActive(currentModelIndex == 0);
        panel2.SetActive(currentModelIndex == 1);
        panel3.SetActive(currentModelIndex == 2);
    }
}
