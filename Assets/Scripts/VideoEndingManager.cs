using UnityEngine;
using UnityEngine.Video;
using UnityEngine.SceneManagement; // Dùng để load lại màn chơi

public class VideoEndingManager : MonoBehaviour
{
    public VideoPlayer videoPlayer;
    public GameObject nutChoiMoi; // Kéo Btn_ChoiMoi vào đây

    void Start()
    {
        // Tắt nút chơi mới lúc video mới bắt đầu chiếu
        if (nutChoiMoi != null) nutChoiMoi.SetActive(false);

        // Đăng ký sự kiện: Khi video chiếu đến điểm cuối cùng -> Gọi hàm HienThiNut
        if (videoPlayer != null)
        {
            videoPlayer.loopPointReached += HienThiNut;
        }
    }

    void HienThiNut(VideoPlayer vp)
    {
        // Hiện nút bấm lên
        if (nutChoiMoi != null) nutChoiMoi.SetActive(true);
    }

    // Hàm này sẽ được gọi khi bạn bấm nút "Chơi Lại Từ Đầu"
    public void ChoiLaiGame()
    {
        // Rất quan trọng: Mở lại thời gian của game (vì trước đó lúc bị bắt đã set = 0)
        Time.timeScale = 1f;

        // Load lại chính màn chơi hiện tại (Reset game)
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}