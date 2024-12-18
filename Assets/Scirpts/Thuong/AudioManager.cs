using UnityEngine;
using UnityEngine.SceneManagement; // Thêm thư viện này để dùng sự kiện SceneManager

public class AudioManager : MonoBehaviour
{
    public AudioSource nhacnen; // Nhạc nền
    public AudioSource nhacbtn; // Nhạc nút (button)

    public static AudioManager instance;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }

        // Đăng ký sự kiện khi scene được tải
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        // Kiểm tra xem scene vừa tải có phải là scene 1 không
        if (scene.name == "SceneCreateRoom") // Thay "Scene1" bằng tên thật của scene 1
        {
            if (nhacnen != null)
            {
                nhacnen.Pause(); // Tắt nhạc nền khi vào scene 1
            }
        }
        else
        {
            if (nhacnen != null && !nhacnen.isPlaying)
            {
                nhacnen.Play(); // Bật nhạc nền khi không ở scene 1
            }
        }
    }

    private void Start()
    {
        // Chạy nhạc nền liên tục
        if (nhacnen != null && !nhacnen.isPlaying)
        {
            nhacnen.loop = true;  // Đảm bảo nhạc nền phát lặp lại
            nhacnen.Play();       // Bắt đầu phát nhạc nền
        }
    }

    // Hàm phát nhạc nút
    public void ButtonMusic()
    {
        if (nhacbtn != null)
        {
            nhacbtn.Play();
        }
        else
        {
            Debug.LogError("Nhacbtn is not assigned in the inspector!");
        }
    }

    private void OnDestroy()
    {
        // Hủy đăng ký sự kiện khi đối tượng bị phá hủy
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }
}
