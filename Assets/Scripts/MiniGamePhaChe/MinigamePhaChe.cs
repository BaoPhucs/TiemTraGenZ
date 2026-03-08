using UnityEngine;
using UnityEngine.UI;

public class MinigamePhaChe : MonoBehaviour
{
    public static MinigamePhaChe Instance;

    [Header("Kéo Slider_PhaChe vào đây:")]
    public Slider thanhTruot;
    public float tocDo = 1.5f;

    private bool dangPhaChe = false;
    private int huongChay = 1;

    private string monDangPhaTam = "";
    private GameObject lyNuocGoc;

    void Awake()
    {
        Instance = this;
    }

    void Start()
    {
        // --- ĐÃ SỬA: Tắt luôn toàn bộ cục UI Minigame đi cho khuất mắt ---
        gameObject.SetActive(false);
    }

    void Update()
    {
        if (!dangPhaChe) return;

        thanhTruot.value += tocDo * huongChay * Time.deltaTime;
        if (thanhTruot.value >= 1f || thanhTruot.value <= 0f) huongChay *= -1;

        if (Input.GetKeyDown(KeyCode.Space))
        {
            ChotPhaChe();
        }
    }

    public void BatDauMinigame(string tenMon, GameObject lyTrenBan)
    {
        // --- ĐÃ SỬA: Khi được gọi thì mới hiển thị toàn bộ UI lên ---
        gameObject.SetActive(true);

        monDangPhaTam = tenMon;
        lyNuocGoc = lyTrenBan;

        if (thanhTruot != null)
        {
            thanhTruot.gameObject.SetActive(true);
            thanhTruot.value = 0f;
        }

        dangPhaChe = true;
        Debug.Log("🎮 ĐANG PHA CHẾ! Bấm Space để dừng!");
    }

    void ChotPhaChe()
    {
        dangPhaChe = false;

        // --- ĐÃ SỬA: Chơi xong thì tắt toàn bộ UI đi ---
        gameObject.SetActive(false);

        bool isPerfect = (thanhTruot.value >= 0.4f && thanhTruot.value <= 0.6f);

        if (isPerfect) Debug.Log("🌟 PERFECT! Đã pha ra Ly Trà Hảo Hạng!");
        else Debug.Log("🤢 BAD! Pha trượt rồi, ly trà dở tệ!");

        if (PlayerHand.Instance != null)
        {
            PlayerHand.Instance.monDangCam = monDangPhaTam;
            PlayerHand.Instance.isPerfectDrink = isPerfect;
        }

        if (lyNuocGoc != null) Destroy(lyNuocGoc);
    }
}