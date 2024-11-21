using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CharSelect : MonoBehaviour
{
    public TMP_Dropdown dropdown;
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        Select(dropdown.value);
    }

    public void Select(int i)
    {
        switch (i)
        {
            case 0:
                this.gameObject.transform.GetChild(0).gameObject.SetActive(true);
                this.gameObject.transform.GetChild(1).gameObject.SetActive(false);
                this.gameObject.transform.GetChild(2).gameObject.SetActive(false);
                this.gameObject.transform.GetChild(3).gameObject.SetActive(false);
                break;
            case 2:
                this.gameObject.transform.GetChild(0).gameObject.SetActive(false);
                this.gameObject.transform.GetChild(1).gameObject.SetActive(true);
                this.gameObject.transform.GetChild(2).gameObject.SetActive(false);
                this.gameObject.transform.GetChild(3).gameObject.SetActive(false);
                break;
            case 3:
                this.gameObject.transform.GetChild(0).gameObject.SetActive(false);
                this.gameObject.transform.GetChild(1).gameObject.SetActive(false);
                this.gameObject.transform.GetChild(2).gameObject.SetActive(true);
                this.gameObject.transform.GetChild(3).gameObject.SetActive(false);
                break;
            case 4:
                this.gameObject.transform.GetChild(0).gameObject.SetActive(false);
                this.gameObject.transform.GetChild(1).gameObject.SetActive(false);
                this.gameObject.transform.GetChild(2).gameObject.SetActive(false);
                this.gameObject.transform.GetChild(3).gameObject.SetActive(true);
                break;
            default:
                break;
        }
    }

    public void ReloadCode()
    {
        Start();
    }


}
