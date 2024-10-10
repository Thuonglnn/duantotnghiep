using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class cameralock : MonoBehaviour
{
    // Hướng mà camera sẽ bị khóa
    public Vector3 lockDirection = new Vector3(0, 0, -1); // Hướng phía trước (trục Z)

    void LateUpdate()
    {
        // Lấy hướng mà camera đang hướng tới
        Quaternion rotation = Quaternion.LookRotation(lockDirection);
        
        // Áp dụng rotation cho camera
        transform.rotation = rotation;
    }
}
