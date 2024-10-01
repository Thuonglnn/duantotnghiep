using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class pressanykey : MonoBehaviour
{
    // Start is called before the first frame update
    public GameObject canvastrue, canvasfalse;
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (Input.anyKeyDown)
        {
            canvastrue.SetActive(true);
            canvasfalse.SetActive(false);


        }
    }
}
