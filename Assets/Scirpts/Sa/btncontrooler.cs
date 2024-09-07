using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class btncontrooler : MonoBehaviour
{
    public GameObject scrollviewiv, scrollviewshop;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKey(KeyCode.P))
        {
            scrollviewshop.SetActive(true);
            scrollviewiv.SetActive(false);
        }
        if (Input.GetKey(KeyCode.I))
        {
            scrollviewiv.SetActive(true);
            scrollviewshop.SetActive(false);
        }
    }
}
