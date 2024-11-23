using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using TMPro;
using UnityEngine.UI;

public class ListManager : MonoBehaviour
{
    public GameObject contentPanel;
    public GameObject ItemPrefab;
    public TMP_Text RoomId;
    public TMP_Text MapName;
    public TMP_Text GameMode;

    private List<RoomData> items = new List<RoomData>();

    int indexList;

    void Start()
    {
        StartCoroutine(GetRoomList());
    }

    IEnumerator GetRoomList()
    {
        UnityWebRequest request = UnityWebRequest.Get("http://localhost:3005/RoomId/getroomid");
        yield return request.SendWebRequest();

        if (request.result == UnityWebRequest.Result.Success)
        {
            string jsonResponse = request.downloadHandler.text;
            RoomList roomList = JsonUtility.FromJson<RoomList>(jsonResponse);
            items.Clear();
            items.AddRange(roomList.data);
            indexList = items.Count; // Cập nhật indexList bằng với số lượng phần tử trong items
            PopulateList();
        }
        else
        {
            Debug.LogError("Error: " + request.error);
        }
    }

    void PopulateList()
    {
        // Clear old items
        foreach (Transform child in contentPanel.transform)
        {
            Destroy(child.gameObject);
        }

        // Add new items to the list
        for (int i = 0; i < indexList && i < items.Count; i++)
        {
            GameObject newItem = Instantiate(ItemPrefab, contentPanel.transform);

            // Update the text of the item with the corresponding value from the items list
            RoomData item = items[i];
            TMP_Text[] texts = newItem.GetComponentsInChildren<TMP_Text>();
            foreach (TMP_Text text in texts)
            {
                if (text.name == "RoomId")
                {
                    text.text = item.roomId;
                }
                else if (text.name == "MapName")
                {
                    text.text = item.mapName;
                }
                else if (text.name == "GameMode")
                {
                    text.text = item.gameMode;
                }
            }
        }
    }

    public void IncreaseIndex()
    {
        if (indexList < items.Count)
        {
            indexList++;
            PopulateList();
        }
    }

    public void DecreaseIndex()
    {
        if (indexList > 0)
        {
            indexList--;
            PopulateList();
        }
    }
}

[System.Serializable]
public class RoomData
{
    public string roomId;
    public string mapName;
    public string gameMode;
}

[System.Serializable]
public class RoomList
{
    public List<RoomData> data;
}
