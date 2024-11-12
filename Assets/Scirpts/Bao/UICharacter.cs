using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class UICharacter : NetworkBehaviour
{
    void Start()
    {
        if(IsOwner)
        {
            gameObject.SetActive(true);
        }
        else
        {
            gameObject.SetActive(false);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
