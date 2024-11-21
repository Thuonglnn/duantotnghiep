using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class DropdownCtrl : MonoBehaviour
{
    public GameObject image1, image2, image3, image4;
    public TMP_Dropdown tMP_Dropdown;
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        imgCtrl(tMP_Dropdown.value);
    }

    public void imgCtrl(int i)
    {
        switch (i)
        {
            case 0:
                image1.SetActive(true);
                image2.SetActive(false);
                image3.SetActive(false);
                image4.SetActive(false);
                break;
            case 1:
                image1.SetActive(false);
                image2.SetActive(true);
                image3.SetActive(false);
                image4.SetActive(false);
                break;
            case 2:
                image1.SetActive(false);
                image2.SetActive(false);
                image3.SetActive(true);
                image4.SetActive(false);
                break;
            case 3:
                image1.SetActive(false);
                image2.SetActive(false);
                image3.SetActive(false);
                image4.SetActive(true);
                break;
            default:
                image1.SetActive(false);
                image2.SetActive(false);
                image3.SetActive(false);
                image4.SetActive(false);
                break;

        }
    }
}
