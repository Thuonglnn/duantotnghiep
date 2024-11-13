using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Arrow_1 : MonoBehaviour
{
    public float speed = 10f; 

    public float timeDestroy = 3f;

    void Start()
    {
       
    }

    void Update()
    {
        // Di chuyển mũi tên theo hướng mà nó đang đối diện
        transform.Translate(Vector3.forward * speed * Time.deltaTime);

        Destroy(gameObject,timeDestroy);
    }
}
