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

    private bool dangXemThongKe = false;

    void Start()
    {
        TimBangKetToan();

        // Bật bảng khi mới vào game
        if (bangKetToanPanel != null)
        {
            dangXemThongKe = false;
            bangKetToanPanel.SetActive(true);

            if (btnBatDauNgayMoi != null) btnBatDauNgayMoi.SetActive(true);

            CapNhatSoLieu();

            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
        }
    }

    // ========================================================
    // HÀM BẤT TỬ: Tìm bảng kết toán xuyên qua cả trạng thái TẮT
    // ========================================================
    void TimBangKetToan()
    {
        if (bangKetToanPanel == null)
        {
            GameObject canvas = GameObject.Find("HUD_Canvas");
            if (canvas != null)
            {
                Transform panel = canvas.transform.Find("BangKetToan_Panel");
                if (panel != null) bangKetToanPanel = panel.gameObject;
            }
        }
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Tab))
        {
            ToggleSoKeToan();
        }

        if (bangKetToanPanel != null && bangKetToanPanel.activeSelf && !dangXemThongKe)
        {
            if (Input.GetKeyDown(KeyCode.Return) || Input.GetKeyDown(KeyCode.KeypadEnter))
            {
                SangNgayMoi();
            }
        }
    }

    void ToggleSoKeToan()
    {
        TimBangKetToan(); // Khóa mục tiêu, không bao giờ lo mất kết nối
        if (bangKetToanPanel == null) return;

        // ========================================================
        // BẢO VỆ GAME: Nếu game đang bị dừng (do Phá sản, Bị bắt...) 
        // thì KHÔNG CHO BẤM TAB ĐỂ TRÁNH XUNG ĐỘT THỜI GIAN
        // ========================================================
        if (!bangKetToanPanel.activeSelf && Time.timeScale == 0) return;

        bool dangMo = !bangKetToanPanel.activeSelf;
        dangXemThongKe = dangMo;
        bangKetToanPanel.SetActive(dangMo);

        if (dangMo)
        {
            CapNhatSoLieu();
            if (btnBatDauNgayMoi != null) btnBatDauNgayMoi.SetActive(false);

            Time.timeScale = 0;
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
        }
        else
        {
            Time.timeScale = 1;
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
        }
    }

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
        TimBangKetToan();
        dangXemThongKe = false;
        if (bangKetToanPanel != null) bangKetToanPanel.SetActive(true);
        if (btnBatDauNgayMoi != null) btnBatDauNgayMoi.SetActive(true);

        DonRac[] dongRacConSot = FindObjectsOfType<DonRac>();
        if (dongRacConSot.Length > 0 && QuanLyKho.Instance != null)
        {
            int diemBiTru = dongRacConSot.Length * 5;
            QuanLyKho.Instance.DiemTinhLang -= diemBiTru;

            if (QuanLyKho.Instance.DiemTinhLang < 0) QuanLyKho.Instance.DiemTinhLang = 0;

            QuanLyKho.Instance.SaveGame();
            Debug.Log($"<color=red>🤬 Hàng xóm: Bán xong xả rác đầy đường hả? Bị trừ {diemBiTru} Tình Làng Nghĩa Xóm!</color>");

            foreach (DonRac rac in dongRacConSot)
            {
                Destroy(rac.gameObject);
            }
        }

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