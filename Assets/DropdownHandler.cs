using UnityEngine;
using TMPro;
using Unity.Netcode;

public class DropdownHandler : MonoBehaviour
{
    public TMP_Dropdown tMP_Dropdown;
    public PlayerSpawner playerSpawner;

    void Start()
    {
        if (tMP_Dropdown != null)
        {
            // Đăng ký sự kiện OnValueChanged của Dropdown
            tMP_Dropdown.onValueChanged.AddListener(OnDropdownValueChanged);
        }
    }

    public void OnDropdownValueChanged(int value)
    {
        if (NetworkManager.Singleton.IsClient && playerSpawner != null)
        {
            // Gửi lựa chọn nhân vật lên server
            playerSpawner.SelectCharacterServerRpc(value);
        }
    }
}
