using UnityEngine;
using TMPro; // Để hiển thị thông báo
using UnityEngine.SceneManagement; // Cần để chuyển cảnh (Load Scene)
using UnityEngine.UI; // Cần để dùng Slider âm thanh

public class GameManager : MonoBehaviour
{
    // ==========================================
    // PHẦN 1: LOGIC CŨ (KIỂM TRA CUỐI NGÀY)
    // ==========================================
    [Header("--- THAM CHIẾU GAMEPLAY ---")]
    public HomeZone khuVucNha;
    public GarageDoor cuaCuon;
    public string tagGhe = "Chair";

    [Header("--- GIAO DIỆN KẾT THÚC NGÀY ---")]
    public GameObject panelDoanhThu;
    public GameObject panelCanhBao;
    public TextMeshProUGUI textLoi;

    // ==========================================
    // PHẦN 2: LOGIC MỚI (HỆ THỐNG PAUSE & MENU)
    // ==========================================
    [Header("--- HỆ THỐNG PAUSE ---")]
    public GameObject pausePanel; // Kéo cái Panel Pause vào đây
    public Slider volumeSlider;   // Kéo thanh trượt âm thanh vào đây

    private bool isPaused = false;

    void Start()
    {
        // Đảm bảo khi vào game thì thời gian chạy bình thường
        Time.timeScale = 1;

        // Ẩn bảng Pause và bảng Báo lỗi lúc đầu
        if (pausePanel != null) pausePanel.SetActive(false);
        if (panelCanhBao != null) panelCanhBao.SetActive(false);
        if (panelDoanhThu != null) panelDoanhThu.SetActive(false);

        // Cài đặt thanh âm lượng theo âm lượng hiện tại của game
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

    // --- CÁC HÀM HỆ THỐNG ---

    public void PauseGame()
    {
        isPaused = true;
        if (pausePanel != null) pausePanel.SetActive(true); // Hiện bảng Pause
        Time.timeScale = 0; // ĐÓNG BĂNG THỜI GIAN
    }

    public void ResumeGame()
    {
        isPaused = false;
        if (pausePanel != null) pausePanel.SetActive(false); // Ẩn bảng Pause
        Time.timeScale = 1; // Thời gian chạy lại
    }

    public void RestartLevel()
    {
        Time.timeScale = 1; // Mở lại thời gian trước khi load
        // Load lại màn chơi hiện tại
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void GoToMainMenu()
    {
        Time.timeScale = 1;
        // Chuyển về Scene Menu (Bạn phải tạo Scene tên là "MenuScene")
        SceneManager.LoadScene("MenuScene");
    }

    public void SetVolume(float volume)
    {
        AudioListener.volume = volume; // Chỉnh âm lượng tổng của cả game
    }

    // ==========================================
    // PHẦN 3: LOGIC CŨ GIỮ NGUYÊN
    // ==========================================

    public void KiemTraKetThucNgay()
    {
        // 1. Kiểm tra Xe vào nhà chưa?
        if (khuVucNha != null && khuVucNha.xeDaVaoNha == false)
        {
            HienLoi("Xe vẫn đang ở ngoài đường! Hãy đẩy xe vào nhà.");
            return;
        }

        // 2. Kiểm tra Cửa đóng chưa?
        if (cuaCuon != null && cuaCuon.isClosed == false)
        {
            HienLoi("Cửa cuốn chưa đóng! Hãy bấm E để đóng cửa.");
            return;
        }

        // 3. KIỂM TRA GHẾ
        int soGheConLai = GameObject.FindGameObjectsWithTag(tagGhe).Length;
        if (soGheConLai > 0)
        {
            HienLoi($"Vẫn còn {soGheConLai} cái ghế ngoài đường! Hãy thu dọn hết.");
            return;
        }

        // THÀNH CÔNG
        Debug.Log("NGÀY LÀM VIỆC HOÀN HẢO!");
        if (panelCanhBao != null) panelCanhBao.SetActive(false);
        if (panelDoanhThu != null) panelDoanhThu.SetActive(true);

        // Có thể thêm: Dừng game khi thắng
        // Time.timeScale = 0; 
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