using System.Collections;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEngine.Video;
using UnityEngine.SceneManagement;

public class TraNoManager : MonoBehaviour
{
    [Header("=== UI BẢNG DOANH THU ===")]
    public GameObject bangKetToanPanel;
    public TextMeshProUGUI txtTienNo;
    public Button btnTraNo;

    [Header("=== GIAO DIỆN CẦN ẨN QUẤT ===")]
    public GameObject txtViral;      // Kéo Txt_Viral vào đây
    public GameObject txtTinhLang;   // Kéo Txt_TinhLang vào đây

    [Header("=== VIDEO HAPPY ENDING ===")]
    public VideoPlayer videoHappyEnding;

    void Start()
    {
        if (btnTraNo != null)
        {
            btnTraNo.onClick.AddListener(ThucHienTraNo);
        }
    }

    void Update()
    {
        if (QuanLyKho.Instance == null) return;

        if (txtTienNo != null)
        {
            txtTienNo.text = "So tien con no: " + QuanLyKho.Instance.TienNo.ToString("n0") + "đ";
        }

        if (btnTraNo != null)
        {
            bool duDieuKien = QuanLyKho.Instance.TienHienCo >= QuanLyKho.Instance.TienNo && QuanLyKho.Instance.TienNo > 0;
            btnTraNo.gameObject.SetActive(duDieuKien);
        }
    }

    public void ThucHienTraNo()
    {
        QuanLyKho.Instance.TienHienCo -= QuanLyKho.Instance.TienNo;
        QuanLyKho.Instance.TienNo = 0;
        QuanLyKho.Instance.SaveGame();

        Debug.Log("🎉 ĐÃ TRẢ HẾT NỢ! PHÁT VIDEO HAPPY ENDING!");

        // 1. Ẩn 2 dòng text Độ Viral và Tình Làng cho khỏi vướng video
        if (txtViral != null) txtViral.SetActive(false);
        if (txtTinhLang != null) txtTinhLang.SetActive(false);

        // 2. Bật và chiếu Video
        if (videoHappyEnding != null)
        {
            videoHappyEnding.gameObject.SetActive(true);
            videoHappyEnding.Play();

            // 3. SỬA LỖI ĐỎ: Nhờ QuanLyKho chạy dùm bộ đếm giờ (đảm bảo không bao giờ bị tắt giữa chừng)
            QuanLyKho.Instance.StartCoroutine(ChoVideoChieuXong((float)videoHappyEnding.length));
        }

        // 4. BÂY GIỜ mới tắt bảng doanh thu đi
        if (bangKetToanPanel != null) bangKetToanPanel.SetActive(false);
    }

    private IEnumerator ChoVideoChieuXong(float thoiGian)
    {
        yield return new WaitForSecondsRealtime(thoiGian + 0.5f);

        Debug.Log("Đã chiếu xong Video! Đang đưa về màn hình chính...");

        Time.timeScale = 1f;

        // DÙNG CÁCH CỦA BẠN: Load thẳng Scene số 0 (Khỏi sợ sai tên)
        //SceneManager.LoadScene(0);
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}