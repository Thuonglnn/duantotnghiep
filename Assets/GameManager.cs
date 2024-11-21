using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using TMPro;
using UnityEngine;
using Unity.Netcode;

public class GameManager : NetworkBehaviour
{
    int gamemode, map, character;
    public Transform pvpArena1, pvpArena2, pveArena;
    public Transform pvpForest1, pvpForest2, pveForest;
    public GameObject Arena, Forest;

    public GameObject GamepveArenaManager, GamepveForestManager;

    PlayerSpawner playerSpawne;

    public TMP_Dropdown dropdownGameMode, dropdownMap, dropdownChar;



    void Awake()
    {
        playerSpawne = GetComponent<PlayerSpawner>();
    }

    // Start is called before the first frame update
    void Start()
    {

        gamemode = dropdownGameMode.value;
        character = dropdownChar.value;

    }

    // Update is called once per frame
    void Update()
    {
        gamemode = dropdownGameMode.value;
        map = dropdownMap.value;

        // mode0: pvp  mode1: pve  mode2: sv
        switch (gamemode)
        {
            case 0:
                if (map == 0)
                {
                    playerSpawne.hostSpawnPoint = pvpArena1;
                    playerSpawne.clientSpawnPoint = pvpArena2;
                }
                else if (map == 1)
                {
                    playerSpawne.hostSpawnPoint = pvpForest1;
                    playerSpawne.clientSpawnPoint = pvpForest2;
                }



                break;
            case 1:
                if (map == 0)
                {
                    playerSpawne.hostSpawnPoint = pveArena;
                    playerSpawne.clientSpawnPoint = pveArena;
                    GamepveArenaManager.SetActive(true);
                    GamepveForestManager.SetActive(false);
                }
                else if (map == 1)
                {
                    playerSpawne.hostSpawnPoint = pveArena;
                    playerSpawne.clientSpawnPoint = pveArena;
                    GamepveForestManager.SetActive(true);
                    GamepveArenaManager.SetActive(false);
                }


                break;
            case 2:
                if (map == 0)
                {
                }
                else if (map == 1)
                {
                }


                break;
            default:
                break;

        }



        // CharSelect(character);

    }

    // public void CharSelect(int i)
    // {
    //     switch (i)
    //     {
    //         case 0:
    //             playerSpawne.playerPrefab = Char1;
    //             playerSpawne.playerPrefab2 = Char1;
    //             break;
    //         case 1:
    //             playerSpawne.playerPrefab = Char2;
    //             break;
    //         case 2:
    //             playerSpawne.playerPrefab = Char3;
    //             playerSpawne.playerPrefab2 = Char3;
    //             break;
    //         case 3:
    //             playerSpawne.playerPrefab = Char4;
    //             playerSpawne.playerPrefab2 = Char4;
    //             break;
    //         default:
    //             break;

    //     }
    // }


    public void ReloadScript()
    {
        // Gọi lại hàm Start hoặc khởi tạo lại
        Start();
    }

}
