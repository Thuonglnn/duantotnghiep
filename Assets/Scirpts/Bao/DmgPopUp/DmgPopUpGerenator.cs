using System;
using UnityEngine;
using TMPro;
using Unity.Netcode;

public class DmgPopUpGerenator : NetworkBehaviour
{
    public static DmgPopUpGerenator Instance { get; private set; }

    public GameObject prefabPopUp;
    public GameObject prefabPopUpCrit;

    private void Awake()
    {
        // Singleton Pattern
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    [ClientRpc]
    private void CreatePopUpClientRpc(Vector3 position, string text, Color color, bool isCrit = false)
    {
        GameObject popupPrefab = isCrit ? prefabPopUpCrit : prefabPopUp;
        var popup = Instantiate(popupPrefab, position, Quaternion.identity);
        var temp = popup.transform.GetChild(0).GetComponent<TextMeshProUGUI>();
        temp.text = text;
        temp.faceColor = color;

        Destroy(popup, 1f);
    }

    public void CreatePopUp(Vector3 position, string text, Color color, bool isCrit = false)
    {
        if (IsOwner)
        {
            CreatePopUpClientRpc(position, text, color, isCrit);
        }
    }
}
