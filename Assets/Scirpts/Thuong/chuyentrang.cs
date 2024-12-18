using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI; // Thêm thư viện này để sử dụng UI

public class chuyentrang : MonoBehaviour
{
    public  GameObject setting;

    void Start()
    {

    }



    void Update()
    {

    }
    public void opensetting()
    {AudioManager.instance.ButtonMusic();
        setting.SetActive(true);
    }
     public void closesetting()
    {AudioManager.instance.ButtonMusic();
        setting.SetActive(false);
    }
    public void UndoDNDK()
    {AudioManager.instance.ButtonMusic();
        SceneManager.LoadScene("DNDK");
        Time.timeScale = 1;
    }

    public void DN()
    {AudioManager.instance.ButtonMusic();
        SceneManager.LoadScene("DangNhap");
        Time.timeScale = 1;
    }

    public void DK()
    {AudioManager.instance.ButtonMusic();
        SceneManager.LoadScene("DangKi");
        Time.timeScale = 1;
    }

    public void QuenMK()
    {AudioManager.instance.ButtonMusic();
        SceneManager.LoadScene("QuenMK");
        Time.timeScale = 1;
    }

    public void Thoat()
    {AudioManager.instance.ButtonMusic();
        // Thoát ứng dụng
    Application.Quit();

    #if UNITY_EDITOR
    UnityEditor.EditorApplication.isPlaying = false; // Dừng chế độ chơi trong Unity Editor
    #endif
    }

    public void PK()
    {AudioManager.instance.ButtonMusic();
        SceneManager.LoadScene("PK");
        Time.timeScale = 1;
    }

    public void PK_SOLO()
    {AudioManager.instance.ButtonMusic();
        SceneManager.LoadScene("PK_SOLO");
        Time.timeScale = 1;
    }

    public void PK_TEAM()
    {AudioManager.instance.ButtonMusic();
        SceneManager.LoadScene("PK_TEAM");
        Time.timeScale = 1;
    }


    public void SearchRoom()
    {AudioManager.instance.ButtonMusic();
        SceneManager.LoadScene("SceneCreateRoom");
        Time.timeScale = 1;
    }

    public void PL()
    {AudioManager.instance.ButtonMusic();
        SceneManager.LoadScene("PL");
        Time.timeScale = 1;
    }

    public void PL_SOLO()
    {AudioManager.instance.ButtonMusic();
        SceneManager.LoadScene("PL_SOLO");
        Time.timeScale = 1;
    }

    public void PL_TEAM()
    {AudioManager.instance.ButtonMusic();
        SceneManager.LoadScene("PL_TEAM");
        Time.timeScale = 1;
    }



    // chuyển trang ở home

    public void shop()
    {AudioManager.instance.ButtonMusic();
        SceneManager.LoadScene("Shop");
        Time.timeScale = 1;
    }

    public void tudo()
    {AudioManager.instance.ButtonMusic();
        SceneManager.LoadScene("TuDo");
        Time.timeScale = 1;
    }


    public void tuong()
    {AudioManager.instance.ButtonMusic();
        SceneManager.LoadScene("tuong");
        Time.timeScale = 1;
    }






    // trở về trang chủ
    public void UndoHome()
    {AudioManager.instance.ButtonMusic();
        SceneManager.LoadScene("Home");
        Time.timeScale = 1;
    }


    // từ chi tiết tướng trở về tướng
    public void Undotuong()
    {AudioManager.instance.ButtonMusic();
        SceneManager.LoadScene("tuong");
        Time.timeScale = 1;
    }






}
