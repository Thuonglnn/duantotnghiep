using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement; 
using UnityEngine.UI; // Thêm thư viện này để sử dụng UI

public class chuyentrang : MonoBehaviour
{

    void Start()
    {
     
    }



    void Update()
    {
        
    }

    public void UndoDNDK()
    {
        SceneManager.LoadScene("DNDK");
        Time.timeScale = 1;
    }

    public void DN()
    {
        SceneManager.LoadScene("DangNhap");
        Time.timeScale = 1;
    }

    public void DK()
    {
        SceneManager.LoadScene("DangKi");
        Time.timeScale = 1;
    }

    public void Thoat()
    {
        Application.Quit(); // Thoát ứng dụng
        #if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false; // Dừng chế độ chơi trong Unity Editor
        #endif
    }

    public void PK()
    {
        SceneManager.LoadScene("PK");
        Time.timeScale = 1;
    }

    public void PK_SOLO()
    {
        SceneManager.LoadScene("PK_SOLO");
        Time.timeScale = 1;
    }

    public void PK_TEAM()
    {
        SceneManager.LoadScene("PK_TEAM");
        Time.timeScale = 1;
    }

    public void PL()
    {
        SceneManager.LoadScene("PL");
        Time.timeScale = 1;
    }

    public void PL_SOLO()
    {
        SceneManager.LoadScene("PL_SOLO");
        Time.timeScale = 1;
    }

    public void PL_TEAM()
    {
        SceneManager.LoadScene("PL_TEAM");
        Time.timeScale = 1;
    }





    public void tuong()
    {
        SceneManager.LoadScene("tuong");
        Time.timeScale = 1;
    }

   

   

  

    public void UndoHome()
    {
        SceneManager.LoadScene("Home");
        Time.timeScale = 1;
    }

    public void Undotuong()
    {
        SceneManager.LoadScene("tuong");
        Time.timeScale = 1;
    }


    
}
