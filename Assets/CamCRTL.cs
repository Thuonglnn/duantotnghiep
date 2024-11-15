using UnityEngine;
using Cinemachine;

public class CamCRTL : MonoBehaviour
{
    public CinemachineVirtualCamera virtualCamera; // Gắn CinemachineVirtualCamera từ Inspector
    public Transform target;                      // Mục tiêu để camera theo dõi
    public float mouseSensitivity = 100f;         // Độ nhạy của chuột
    public Vector2 rotationLimits = new Vector2(-30f, 60f); // Giới hạn góc quay dọc

    private float pitch = 0f;  // Góc quay dọc
    private float yaw = 0f;    // Góc quay ngang

    void Start()
    {
        // Kiểm tra nếu virtualCamera chưa được gắn
        if (virtualCamera == null)
        {
            Debug.LogError("Virtual Camera is not assigned!");
            return;
        }

        // Khóa và ẩn con trỏ chuột
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    void Update()
    {
        if (target == null) return;

        // Lấy dữ liệu từ chuột
        float mouseX = Input.GetAxis("Mouse X") * mouseSensitivity * Time.deltaTime;
        float mouseY = Input.GetAxis("Mouse Y") * mouseSensitivity * Time.deltaTime;

        // Tính toán góc quay
        yaw += mouseX;
        pitch -= mouseY;

        // Giới hạn góc quay dọc
        pitch = Mathf.Clamp(pitch, rotationLimits.x, rotationLimits.y);

        // Xoay camera dựa trên góc
        virtualCamera.transform.position = target.position; // Camera theo sát mục tiêu
        virtualCamera.transform.rotation = Quaternion.Euler(pitch, yaw, 0);
    }
}
