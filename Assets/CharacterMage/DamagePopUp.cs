using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class DamagePopUp : MonoBehaviour
{
    public static DamagePopUp damagePopUp;
    public void Awake()
    {
        damagePopUp = this;
    }

    public GameObject prefab;
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    public void createPopUp(Vector3 position, string text, Color color)
    {
        var dp = Instantiate(prefab, position, Quaternion.identity);
        TextMeshProUGUI temp = prefab.transform.GetChild(0).GetComponent<TextMeshProUGUI>();
        temp.text = text;
        temp.color = color;
        Destroy(dp, 1f);

    }
}
