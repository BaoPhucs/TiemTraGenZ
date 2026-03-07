using UnityEngine;
using UnityEngine.UI;

public class MinigamePhaChe : MonoBehaviour
{
    [Header("Kéo Slider_PhaChe vào đây:")]
    public Slider thanhTruot;

    [Header("Tốc độ chạy của vạch:")]
    public float tocDo = 1.5f;

    private bool dangPhaChe = false;
    private int huongChay = 1;

    void Start()
    {
        // Ẩn thanh trượt khi mới vào game
        if (thanhTruot != null) thanhTruot.gameObject.SetActive(false);
    }

    void Update()
    {
        if (!dangPhaChe) return;

        // Cho vạch chạy qua chạy lại
        thanhTruot.value += tocDo * huongChay * Time.deltaTime;
        if (thanhTruot.value >= 1f || thanhTruot.value <= 0f)
        {
            huongChay *= -1; // Đụng tường thì đảo chiều
        }

        // Bấm Space để dừng lại
        if (Input.GetKeyDown(KeyCode.Space))
        {
            ChotPhaChe();
        }
    }

    public void BatDauMinigame()
    {
        thanhTruot.gameObject.SetActive(true);
        thanhTruot.value = 0f;
        dangPhaChe = true;
        Debug.Log("🎮 ĐANG PHA CHẾ! Bấm Space để dừng!");
    }

    void ChotPhaChe()
    {
        dangPhaChe = false;
        thanhTruot.gameObject.SetActive(false); // Ẩn đi khi xong

        // Giả sử vùng Perfect màu xanh nằm ở giữa (từ 0.4 đến 0.6)
        if (thanhTruot.value >= 0.4f && thanhTruot.value <= 0.6f)
        {
            Debug.Log("🌟 PERFECT! Đã pha ra Ly Trà Hảo Hạng!");
            // Gọi lệnh sinh ra ly trà của TV2 ở đây sau
        }
        else
        {
            Debug.Log("🤢 BAD! Pha trượt rồi, ly trà dở tệ!");
        }
    }
}