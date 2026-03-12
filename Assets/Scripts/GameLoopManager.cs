using UnityEngine;
using UnityEngine.UI;
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
        Debug.Log("🚀 [GameLoopManager] Khởi động...");
        DonDepGiaoDienRac();
        TimBangKetToan();
        TimNutBam();

        if (bangKetToanPanel != null)
        {
            dangXemThongKe = false;
            bangKetToanPanel.SetActive(true);

            if (btnBatDauNgayMoi != null)
            {
                btnBatDauNgayMoi.SetActive(true);

                // LỚP BẢO VỆ 1: Ép cái nút phải nhớ chức năng của nó (Chống liệt nút)
                Button nutBam = btnBatDauNgayMoi.GetComponent<Button>();
                if (nutBam != null)
                {
                    nutBam.onClick.RemoveAllListeners(); // Xóa sạch bộ nhớ cũ bị lỗi
                    nutBam.onClick.AddListener(SangNgayMoi); // Gắn chặt lệnh mới vào
                }
            }

            CapNhatSoLieu();
            Time.timeScale = 0;
            MoKhoaChuot();
        }
    }

    void DonDepGiaoDienRac()
    {
        string[] danhSachRac = { "Intro_Panel", "PanelEnding", "Video_HappyEnding", "Video_PhaSan", "Video_BadEnding", "Video_VeSoEnding" };
        GameObject canvas = GameObject.Find("HUD_Canvas");

        if (canvas != null)
        {
            foreach (string ten in danhSachRac)
            {
                Transform panelRac = canvas.transform.Find(ten);
                if (panelRac != null) panelRac.gameObject.SetActive(false);
            }
        }
    }

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

    void TimNutBam()
    {
        if (btnBatDauNgayMoi == null && bangKetToanPanel != null)
        {
            Button nut = bangKetToanPanel.GetComponentInChildren<Button>(true);
            if (nut != null) btnBatDauNgayMoi = nut.gameObject;
        }
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Tab))
        {
            if (bangKetToanPanel != null && bangKetToanPanel.activeSelf && !dangXemThongKe)
            {
                // Cấm dùng Tab để tắt bảng nếu đây là màn hình Bắt buộc
            }
            else
            {
                ToggleSoKeToan();
            }
        }

        if (bangKetToanPanel != null && bangKetToanPanel.activeSelf && !dangXemThongKe)
        {
            if (Input.GetKeyDown(KeyCode.Return) || Input.GetKeyDown(KeyCode.KeypadEnter))
            {
                SangNgayMoi();
            }
        }
    }

    void LateUpdate()
    {
        if (bangKetToanPanel != null && bangKetToanPanel.activeSelf)
        {
            MoKhoaChuot();
        }
    }

    void ToggleSoKeToan()
    {
        TimBangKetToan();
        TimNutBam();
        if (bangKetToanPanel == null) return;

        if (!bangKetToanPanel.activeSelf && Time.timeScale == 0) return;

        bool dangMo = !bangKetToanPanel.activeSelf;
        dangXemThongKe = dangMo;
        bangKetToanPanel.SetActive(dangMo);

        if (dangMo)
        {
            CapNhatSoLieu();
            if (btnBatDauNgayMoi != null) btnBatDauNgayMoi.SetActive(false);

            Time.timeScale = 0;
            MoKhoaChuot();
        }
        else
        {
            Time.timeScale = 1;
            KhoaChuot();
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
        TimNutBam();
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

            foreach (DonRac rac in dongRacConSot) Destroy(rac.gameObject);
        }

        CapNhatSoLieu();
        Time.timeScale = 0;
        MoKhoaChuot();
    }

    public void SangNgayMoi()
    {
        Debug.Log("🎯 [GameLoopManager] ĐÃ KÍCH HOẠT LỆNH SANG NGÀY MỚI!");

        // LỚP BẢO VỆ 2: Bọc Try-Catch để chống game bị kẹt nếu có lỗi ngầm
        try
        {
            if (QuanLyKho.Instance != null)
            {
                QuanLyKho.Instance.DoanhThuNgay = 0;
                QuanLyKho.Instance.ChiPhiNgay = 0;
                QuanLyKho.Instance.RandomGiaThiTruong();
            }

            // Tắt bảng đi ngay lập tức
            if (bangKetToanPanel != null) bangKetToanPanel.SetActive(false);

            PoliceAI.ResetCongAnNgayMoi();

            if (TiemTraGenZ.Manager.StoryManager.Instance != null)
            {
                TiemTraGenZ.Manager.StoryManager.Instance.AdvanceDay();
            }

            Time.timeScale = 1;
            KhoaChuot();

            Debug.Log("✅ [GameLoopManager] Sang ngày mới HOÀN TẤT!");
        }
        catch (System.Exception e)
        {
            Debug.LogError("❌ [GameLoopManager] Có lỗi ngầm khi sang ngày, nhưng game đã được cứu! Lỗi: " + e.Message);
            // Dù có lỗi thì vẫn ép tắt bảng và mở thời gian để người chơi không bị kẹt
            if (bangKetToanPanel != null) bangKetToanPanel.SetActive(false);
            Time.timeScale = 1;
            KhoaChuot();
        }
    }

    void MoKhoaChuot()
    {
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }

    void KhoaChuot()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }
}