using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;

public class Arrow_1 : NetworkBehaviour
{
    public float speed = 10f; 
    public float timeDestroy = 3f;

    void Start()
    {
        if (IsServer)
        {
            Invoke("DestroyArrow", timeDestroy);
        }
    }

    void Update()
    {
        transform.Translate(Vector3.forward * speed * Time.deltaTime);
    }

    [ServerRpc]
    public void RequestDestroyServerRpc()
    {
        DestroyArrow();
    }

    [ClientRpc]
    void DestroyArrowClientRpc()
    {
        Destroy(gameObject);
    }

    private void DestroyArrow()
    {
        DestroyArrowClientRpc();
        Destroy(gameObject);
    }
}
