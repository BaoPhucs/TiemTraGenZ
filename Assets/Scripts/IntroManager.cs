using UnityEngine;
using UnityEngine.Video;

public class IntroManager : MonoBehaviour
{
    [Header("UI References")]
    public VideoPlayer videoPlayer;
    public UnityEngine.UI.RawImage rawImage;
    public GameObject introPanel;
    public GameLoopManager gameLoopManager;

    [Header("THÊM MỚI: ẨN GIAO DIỆN GAME")]
    public GameObject hudCanvas; // Kéo object GameUI hoặc HUD_Canvas vào đây

    [Header("Settings")]
    public bool allowSkip = true;
    public float fadeDuration = 1.0f;
    private bool hasSkipped = false;

    [Header("Video Settings")]
    public string videoFileName = "intro_final_v6.mp4";

    void Start()
    {
        AudioListener.pause = true;
        // 1. Tạm tắt giao diện game (Đồng hồ, tiền, chữ...)
        if (hudCanvas != null) hudCanvas.SetActive(false);

        Time.timeScale = 0;

        if (videoPlayer == null) videoPlayer = GetComponent<VideoPlayer>();
        if (rawImage == null) rawImage = GetComponent<UnityEngine.UI.RawImage>();

        string videoPath = System.IO.Path.Combine(Application.streamingAssetsPath, videoFileName);

        videoPlayer.source = VideoSource.Url;
        videoPlayer.url = videoPath;
        videoPlayer.renderMode = VideoRenderMode.APIOnly;

        videoPlayer.prepareCompleted += (vp) => {
            rawImage.texture = vp.texture;
            rawImage.color = Color.white;
        };

        videoPlayer.Prepare();
        videoPlayer.loopPointReached += OnVideoFinished;
        videoPlayer.Play();
    }

    void Update()
    {
        if (allowSkip && !hasSkipped && Input.anyKeyDown)
        {
            SkipIntro();
        }
    }

    void OnVideoFinished(VideoPlayer vp)
    {
        SkipIntro();
    }

    public void SkipIntro()
    {
        if (hasSkipped) return;
        hasSkipped = true;

        videoPlayer.Stop();
        AudioListener.pause = false;
        if (introPanel != null) introPanel.SetActive(false);

        // Bật lại giao diện game
        if (hudCanvas != null) hudCanvas.SetActive(true);

        // THAY ĐỔI: Chạy thẳng vào game (Ép thời gian chạy, khóa chuột)
        Time.timeScale = 1;
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;

        // Gọi thẳng SangNgayMoi để tăng ngày (đáp ứng yêu cầu khi chơi lại thì tăng ngày lên)
        if (gameLoopManager != null)
        {
            gameLoopManager.SangNgayMoi();
        }
    }
}