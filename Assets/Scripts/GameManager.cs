using UnityEngine;
using TMPro; 
using UnityEngine.SceneManagement;
using UnityEngine.UI; 

public class GameManager : MonoBehaviour
{
    [Header("--- THAM CHIẾU GAMEPLAY ---")]
    public HomeZone khuVucNha;
    public GarageDoor cuaCuon;
    public string tagGhe = "Chair";

    [Header("--- GIAO DIỆN KẾT THÚC NGÀY ---")]
    public GameObject panelDoanhThu;
    public GameObject panelCanhBao;
    public TextMeshProUGUI textLoi;

    [Header("--- HỆ THỐNG PAUSE ---")]
    public GameObject pausePanel; 
    public Slider volumeSlider;   

    private bool isPaused = false;
    
    // Biến để nhớ xem trước khi Pause thì game đang chạy (1) hay đang dừng (0) (ví dụ đang chiếu Intro)
    private float thoiGianTruocKhiPause = 1f; 

    void Start()
    {
        // ĐÃ MỞ KHÓA LUỒNG THỜI GIAN GỐC
        Time.timeScale = 1; 

        if (pausePanel != null) pausePanel.SetActive(false);
        if (panelCanhBao != null) panelCanhBao.SetActive(false);
        if (panelDoanhThu != null) panelDoanhThu.SetActive(false);

        if (volumeSlider != null)
        {
            volumeSlider.value = AudioListener.volume;
            volumeSlider.onValueChanged.AddListener(SetVolume);
        }
    }

    void Update()
    {
        // Bắt sự kiện bấm phím ESC để Pause/Resume
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (isPaused) ResumeGame();
            else PauseGame();
        }
    }

    public void PauseGame()
    {
        isPaused = true;
        if (pausePanel != null) pausePanel.SetActive(true);
        
        // Ghi nhớ lại thời gian hiện tại (lỡ như đang Intro Time=0 thì nhớ là 0)
        thoiGianTruocKhiPause = Time.timeScale; 
        Time.timeScale = 0; // Đóng băng mọi thứ

        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;
    }

    public void ResumeGame()
    {
        isPaused = false;
        if (pausePanel != null) pausePanel.SetActive(false);
        
        // Trả lại đúng luồng thời gian lúc nãy
        Time.timeScale = thoiGianTruocKhiPause; 

        // Chỉ khóa chuột lại nếu game thực sự đang chạy (Time > 0)
        if (Time.timeScale > 0) 
        {
            Cursor.visible = false;
            Cursor.lockState = CursorLockMode.Locked;
        }
    }

    public void RestartLevel()
    {
        Time.timeScale = 1; 
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void GoToMainMenu()
    {
        Time.timeScale = 1;
        SceneManager.LoadScene("HomeScene");
    }

    public void SetVolume(float volume)
    {
        AudioListener.volume = volume; 
    }

    public void KiemTraKetThucNgay()
    {
        if (khuVucNha != null && khuVucNha.xeDaVaoNha == false)
        {
            HienLoi("Xe vẫn đang ở ngoài đường! Hãy đẩy xe vào nhà.");
            return;
        }

        if (cuaCuon != null && cuaCuon.isClosed == false)
        {
            HienLoi("Cửa cuốn chưa đóng! Hãy bấm E để đóng cửa.");
            return;
        }

        int soGheConLai = GameObject.FindGameObjectsWithTag(tagGhe).Length;
        if (soGheConLai > 0)
        {
            HienLoi($"Vẫn còn {soGheConLai} cái ghế ngoài đường! Hãy thu dọn hết.");
            return;
        }

        Debug.Log("NGÀY LÀM VIỆC HOÀN HẢO!");
        if (panelCanhBao != null) panelCanhBao.SetActive(false);
        if (panelDoanhThu != null) panelDoanhThu.SetActive(true);
    }

    void HienLoi(string noiDung)
    {
        Debug.LogWarning(noiDung);
        if (panelCanhBao != null)
        {
            panelCanhBao.SetActive(true);
            if (textLoi != null) textLoi.text = noiDung;
        }
    }
}