using Unity.Netcode;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuManager : NetworkBehaviour
{
    // private bool isCursorVisible = false, isPanelVisible = false; // Trạng thái mặc định: ẩn con trỏ
    // //public GameObject panel;

    // RoomManager roomManager;
    // void Start()
    // {
    //     // // Khởi tạo con trỏ bị ẩn và khóa vào giữa màn hình
        
    //     //panel.SetActive(false);
    // }

    // void Update()
    // {
    //     // Kiểm tra nếu nhấn phím Esc
    //     if (Input.GetKeyDown(KeyCode.Escape))
    //     {
    //         // Chuyển đổi trạng thái con trỏ và panel
    //         isCursorVisible = !isCursorVisible;
    //         isPanelVisible = !isPanelVisible;

    //         // Cập nhật trạng thái con trỏ
    //         Cursor.visible = isCursorVisible;
    //         // Cursor.lockState = !isPanelVisible ? CursorLockMode.None : CursorLockMode.Locked;


    //         // Hiển thị hoặc ẩn panel
    //         //panel.SetActive(isPanelVisible);
    //     }
    //     //panel.SetActive(isPanelVisible);
    //     // if (isPanelVisible)
    //     // {
    //     //     Cursor.lockState = CursorLockMode.None;
    //     // }
    //     // else
    //     // {
    //     //     Cursor.lockState = CursorLockMode.Locked;
    //     // }


    // }

    // // public void ReloadCurrentScene()
    // // {
       
    // //     SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    // // }

    

    // // public void ChangeScene()
    // // {
    // //     SceneManager.LoadScene("Home");
    // // }

    // public void IsPanelActive()
    // {
    //     isPanelVisible = !isPanelVisible;
    // }
}
