using UnityEngine;
using UnityEngine.Video;
using UnityEngine.UI;

public class VideoDialogueTrigger : MonoBehaviour
{
    [Header("UI & Video Settings")]
    public GameObject videoPanel;
    public VideoPlayer videoPlayer;
    public RawImage videoDisplay;   // THÊM MỚI: Màn hình chiếu video
    public VideoClip videoClip;

    [Header("Trigger Settings")]
    public float interactDistance = 2.5f;
    public string playerTag = "Player";

    private Transform playerTransform;
    private bool isPlayingVideo = false;
    private bool canSkip = false;

    void Start()
    {
        GameObject playerObj = GameObject.FindGameObjectWithTag(playerTag);
        if (playerObj != null) playerTransform = playerObj.transform;

        if (videoPanel != null) videoPanel.SetActive(false);

        if (videoPlayer != null)
        {
            videoPlayer.loopPointReached += EndVideo;

            // PHÉP MÀU Ở ĐÂY: Tự động truyền hình ảnh từ Video vào RawImage
            videoPlayer.renderMode = VideoRenderMode.APIOnly;
            videoPlayer.prepareCompleted += (vp) => {
                if (videoDisplay != null)
                {
                    videoDisplay.texture = vp.texture;
                    videoDisplay.color = Color.white;
                }
            };
        }
    }

    void Update()
    {
        if (playerTransform == null) return;

        if (isPlayingVideo)
        {
            if (canSkip && Input.anyKeyDown)
            {
                EndVideo(videoPlayer);
            }
            return;
        }

        float distance = Vector3.Distance(transform.position, playerTransform.position);
        if (distance <= interactDistance)
        {
            if (Input.GetKeyDown(KeyCode.E))
            {
                PlayVideo();
            }
        }
    }

    public void PlayVideo()
    {
        if (videoPanel == null || videoPlayer == null || videoClip == null) return;

        isPlayingVideo = true;
        canSkip = false;
        videoPanel.SetActive(true);

        videoPlayer.clip = videoClip;
        videoPlayer.Prepare(); // Ép Unity tải khung hình đầu tiên trước
        videoPlayer.Play();

        Time.timeScale = 0;
        Invoke(nameof(ChoPhepBoQua), 0.5f);
    }

    void ChoPhepBoQua()
    {
        canSkip = true;
    }

    public void EndVideo(VideoPlayer vp)
    {
        isPlayingVideo = false;
        canSkip = false;

        if (videoPlayer != null) videoPlayer.Stop();
        if (videoPanel != null) videoPanel.SetActive(false);

        Time.timeScale = 1;

        if (TutorialManager.DaHuongDanBaTu == false)
        {
            TutorialManager.DaHuongDanBaTu = true; // Lưu cờ vào RAM
            if (TutorialManager.Instance != null)
            {
                TutorialManager.Instance.HideTutorial();
            }
        }
    }
}