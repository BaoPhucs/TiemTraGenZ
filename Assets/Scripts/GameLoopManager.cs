using UnityEngine;
using TMPro;

public class GameLoopManager : MonoBehaviour
{
    [Header("UI Kết Toán")]
    public GameObject bangKetToanPanel;
    public GameObject btnBatDauNgayMoi;
    public TextMeshProUGUI txtDoanhThu;
    public TextMeshProUGUI txtChiPhi;
    public TextMeshProUGUI txtLoiNhuan;

    // Biến này để chặn người chơi bấm Enter sang ngày mới khi chỉ đang mở ra xem
    private bool dangXemThongKe = false;

    void Start()
    {
        if (bangKetToanPanel == null)
            bangKetToanPanel = GameObject.Find("BangKetToan_Panel");

        // Bật bảng khi mới vào game
        if (bangKetToanPanel != null)
        {
            dangXemThongKe = false; // Chế độ chuẩn, cho phép sang ngày
            bangKetToanPanel.SetActive(true);

            if (btnBatDauNgayMoi != null) btnBatDauNgayMoi.SetActive(true); // Ép HIỆN nút

            CapNhatSoLieu(); // Cập nhật chữ

            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
        }
    }

    void Update()
    {
        // =========================================================
        // TÍNH NĂNG MỚI: BẤM TAB ĐỂ BẬT/TẮT SỔ KẾ TOÁN GIỮA CHỪNG
        // =========================================================
        if (Input.GetKeyDown(KeyCode.Tab))
        {
            ToggleSoKeToan();
        }

        // Logic bấm Enter chỉ hoạt động nếu bảng đang mở VÀ KHÔNG PHẢI đang ở chế độ xem tạm
        if (bangKetToanPanel != null && bangKetToanPanel.activeSelf && !dangXemThongKe)
        {
            if (Input.GetKeyDown(KeyCode.Return) || Input.GetKeyDown(KeyCode.KeypadEnter))
            {
                SangNgayMoi();
            }
        }
    }

    // --- HÀM BẬT/TẮT SỔ KẾ TOÁN (DÙNG PHÍM TAB) ---
    void ToggleSoKeToan()
    {
        if (bangKetToanPanel == null) return;

        bool dangMo = !bangKetToanPanel.activeSelf;
        dangXemThongKe = dangMo; // Đánh dấu là đang mở xem tạm
        bangKetToanPanel.SetActive(dangMo);

        if (dangMo)
        {
            CapNhatSoLieu();
            if (btnBatDauNgayMoi != null) btnBatDauNgayMoi.SetActive(false); // ẨN NÚT ĐI!

            Time.timeScale = 0; // Dừng game để xem
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
        }
        else
        {
            // Tắt bảng đi thì game chạy tiếp
            Time.timeScale = 1;
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
        }
    }

    // --- HÀM CẬP NHẬT TEXT CHUNG (Để không phải viết đi viết lại) ---
    void CapNhatSoLieu()
    {
        if (QuanLyKho.Instance != null)
        {
            int doanhThu = QuanLyKho.Instance.DoanhThuNgay;
            int chiPhi = QuanLyKho.Instance.ChiPhiNgay;
            int loiNhuan = doanhThu - chiPhi;

            if (txtDoanhThu) txtDoanhThu.text = "Doanh Thu: " + doanhThu.ToString("n0") + "đ";
            if (txtChiPhi) txtChiPhi.text = "Chi Phí: " + chiPhi.ToString("n0") + "đ";
            if (txtLoiNhuan) txtLoiNhuan.text = "Loi Nhuan: " + loiNhuan.ToString("n0") + "đ";
        }
    }

    [ContextMenu("TEST KET THUC")]
    public void KetThucNgay()
    {
        dangXemThongKe = false; // Đây là kết thúc thật, không phải xem tạm
        if (bangKetToanPanel != null) bangKetToanPanel.SetActive(true);
        if (btnBatDauNgayMoi != null) btnBatDauNgayMoi.SetActive(true); // HIỆN NÚT LÊN!

        CapNhatSoLieu();

        Time.timeScale = 0;
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }

    public void SangNgayMoi()
    {
        if (QuanLyKho.Instance != null)
        {
            QuanLyKho.Instance.DoanhThuNgay = 0;
            QuanLyKho.Instance.ChiPhiNgay = 0;
            QuanLyKho.Instance.RandomGiaThiTruong();
        }

        if (bangKetToanPanel != null) bangKetToanPanel.SetActive(false);

        PoliceAI.ResetCongAnNgayMoi();

        Time.timeScale = 1;

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        if (TiemTraGenZ.Manager.StoryManager.Instance != null)
        {
            TiemTraGenZ.Manager.StoryManager.Instance.AdvanceDay();
        }

        Debug.Log("Đã sang ngày mới!");
    }
}