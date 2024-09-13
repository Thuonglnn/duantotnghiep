using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement; // Thêm cái này để quản lý cảnh

public class nhanutbk : MonoBehaviour
{
   
    void Start()
    {
        
    }


    void Update()
    {
        // Kiểm tra nếu nhấn bất kỳ phím nào
        if (Input.anyKeyDown)
        {
            // Chuyển sang scene
            SceneManager.LoadScene("DNDK"); 
             Time.timeScale = 1;
        }
    }

}
