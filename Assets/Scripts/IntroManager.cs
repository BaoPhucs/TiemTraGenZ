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
        if (introPanel != null) introPanel.SetActive(false);

        // 2. Bật lại giao diện game
        if (hudCanvas != null) hudCanvas.SetActive(true);

        // 3. Gọi bảng Ngày Mới
        if (gameLoopManager != null)
        {
            gameLoopManager.KetThucNgay();
        }
    }
}