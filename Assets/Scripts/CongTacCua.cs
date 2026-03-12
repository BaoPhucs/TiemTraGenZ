using UnityEngine;
using TMPro;

public class CongTacCua : MonoBehaviour
{
    [Header("Liên kết với Cửa")]
    public GarageDoor cuaCuonCuaToi;

    [Header("UI Hiển Thị")]
    public GameObject textHienThi;
    public TextMeshProUGUI txtNoiDung;

    [Header("Cài đặt khoảng cách")]
    public float khoangCachBamNut = 1f; // Đứng cách công tắc 2.5m là bấm được

    private Transform playerTransform;
    private bool daHienUI = false;

    void Start()
    {
        if (textHienThi != null) textHienThi.SetActive(false);

        // Tự động tìm nhân vật Minh (Phải có Tag là "Player")
        GameObject playerObj = GameObject.FindGameObjectWithTag("Player");
        if (playerObj != null)
        {
            playerTransform = playerObj.transform;
        }
        else
        {
            Debug.LogError("🔴 Công tắc không tìm thấy Player!");
        }
    }

    void Update()
    {
        if (playerTransform == null) return;

        // Dùng thước dây toán học đo khoảng cách từ công tắc đến người chơi
        float khoangCach = Vector3.Distance(transform.position, playerTransform.position);

        // NẾU ĐỨNG GẦN (Dưới 2.5 mét)
        if (khoangCach <= khoangCachBamNut)
        {
            if (!daHienUI)
            {
                daHienUI = true;
                CapNhatChu();
                if (textHienThi != null) textHienThi.SetActive(true);
            }

            // Lắng nghe phím E
            if (Input.GetKeyDown(KeyCode.E))
            {
                if (cuaCuonCuaToi != null)
                {
                    cuaCuonCuaToi.ToggleDoor(); // Ra lệnh mở/đóng cửa
                    CapNhatChu(); // Cập nhật lại chữ E Đóng/Mở
                }
            }
        }
        // NẾU ĐI RA XA
        else
        {
            if (daHienUI)
            {
                daHienUI = false;
                if (textHienThi != null) textHienThi.SetActive(false);
            }
        }
    }

    void CapNhatChu()
    {
        if (txtNoiDung != null && cuaCuonCuaToi != null)
        {
            txtNoiDung.text = cuaCuonCuaToi.isClosed ? "E": "E";
        }
    }
}