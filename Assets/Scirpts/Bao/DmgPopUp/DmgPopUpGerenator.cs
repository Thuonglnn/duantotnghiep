using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class DmgPopUpGerenator : MonoBehaviour
{
    public static DmgPopUpGerenator current;

    public GameObject prefabPopUp;
    public GameObject prefabPopUpCrit;

    
    private void Awake()
    {
        current = this;
    }

    void Update()
    {
    }

    public void CreaterPopUp(Vector3 postion, string text, Color color)
    {
        var popup = Instantiate(prefabPopUp, postion, Quaternion.identity);
        var temp = popup.transform.GetChild(0).GetComponent<TextMeshProUGUI>();
        temp.text = text;
        temp.faceColor = color;

        Destroy(popup,1f);
    }

    public void CreaterPopUpCrit(Vector3 postion, string text, Color color)
    {
        var popup = Instantiate(prefabPopUpCrit, postion, Quaternion.identity);
        var temp = popup.transform.GetChild(0).GetComponent<TextMeshProUGUI>();
        temp.text = text;
        temp.faceColor = color;

        Destroy(popup,1f);
    }

}
