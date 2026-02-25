using UnityEngine;
using UnityEngine.Video;
using UnityEngine.SceneManagement;
using System.Collections;

public class IntroManager : MonoBehaviour
{
    [Header("UI References")]
    public VideoPlayer videoPlayer;
    public UnityEngine.UI.RawImage rawImage; // Thêm biến RawImage để hiển thị video
    public string nextSceneName = "SampleScene"; // Tên scene tiếp theo (Menu chính hoặc Game)

    [Header("Settings")]
    public bool allowSkip = true;
    public float fadeDuration = 1.0f;

    private bool hasSkipped = false;

    [Header("Video Settings")]
    public string videoFileName = "intro_final_v6.mp4"; // Tên file video trong thư mục StreamingAssets

    void Start()
    {
        // Tự động tìm VideoPlayer nếu chưa gán
        if (videoPlayer == null)
            videoPlayer = GetComponent<VideoPlayer>();

        // Tự động tìm RawImage nếu chưa gán (giả sử script gắn trên cùng object)
        if (rawImage == null)
            rawImage = GetComponent<UnityEngine.UI.RawImage>();

        // Tự động set đường dẫn (URL) cho video từ thư mục StreamingAssets
        string videoPath = System.IO.Path.Combine(Application.streamingAssetsPath, videoFileName);
        Debug.Log("Playing Video from: " + videoPath);
        
        videoPlayer.source = VideoSource.Url;
        videoPlayer.url = videoPath;

        // Cấu hình để Video hiển thị lên UI (RawImage)
        // Chế độ APIOnly giúp chúng ta lấy texture từ videoPlayer gán vào RawImage
        videoPlayer.renderMode = VideoRenderMode.APIOnly; 
        videoPlayer.prepareCompleted += (vp) => {
            rawImage.texture = vp.texture; // Gán texture của video vào RawImage
        };
        videoPlayer.Prepare(); // Bắt đầu chuẩn bị video (load file, decode...)

        // Đăng ký sự kiện khi video kết thúc
        videoPlayer.loopPointReached += OnVideoFinished;
        
        // Bắt đầu chạy video
        videoPlayer.Play();
    }

    void Update()
    {
        // Cho phép ấn phím bất kỳ để bỏ qua (nếu bật allowSkip)
        if (allowSkip && !hasSkipped && Input.anyKeyDown)
        {
            SkipIntro();
        }
    }

    void OnVideoFinished(VideoPlayer vp)
    {
        LoadNextScene();
    }

    public void SkipIntro()
    {
        if (hasSkipped) return;
        hasSkipped = true;
        
        Debug.Log("Skipping Intro...");
        LoadNextScene();
    }

    void LoadNextScene()
    {
        Debug.Log("Loading Next Scene: " + nextSceneName);
        // Lưu ý: Đảm bảo Scene "MainMenu" đã được add vào Build Settings
        SceneManager.LoadScene(nextSceneName);
    }
}
