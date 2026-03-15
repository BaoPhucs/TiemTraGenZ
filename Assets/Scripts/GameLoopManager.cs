using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class GameLoopManager : MonoBehaviour
{
    [Header("UI Kết Toán")]
    public GameObject bangKetToanPanel;
    public TextMeshProUGUI txtDoanhThu;
    public TextMeshProUGUI txtChiPhi;
    public TextMeshProUGUI txtLoiNhuan;

    private bool dangXemThongKe = false;

    void Start()
    {
        DonDepGiaoDienRac();
        TimBangKetToan();

        // Giấu bảng ngay từ đầu
        if (bangKetToanPanel != null) bangKetToanPanel.SetActive(false);
    }

    void DonDepGiaoDienRac()
    {
        string[] danhSachRac = { "PanelEnding", "Video_HappyEnding", "Video_PhaSan", "Video_BadEnding", "Video_VeSoEnding" };
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

    void Update()
    {
        // CHỨC NĂNG BẤM TAB ĐỂ XEM DOANH THU GIỮ NGUYÊN
        if (Input.GetKeyDown(KeyCode.Tab))
        {
            ToggleSoKeToan();
        }
    }

    void ToggleSoKeToan()
    {
        TimBangKetToan();
        if (bangKetToanPanel == null) return;

        bool dangMo = !bangKetToanPanel.activeSelf;
        dangXemThongKe = dangMo;
        bangKetToanPanel.SetActive(dangMo);

        if (dangMo)
        {
            CapNhatSoLieu();
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

            if (txtDoanhThu) txtDoanhThu.text = doanhThu.ToString("n0") + "đ";
            if (txtChiPhi) txtChiPhi.text = chiPhi.ToString("n0") + "đ";
            if (txtLoiNhuan) txtLoiNhuan.text = loiNhuan.ToString("n0") + "đ";
        }
    }

    [ContextMenu("TEST KET THUC")]
    public void KetThucNgay()
    {
        DonRac[] dongRacConSot = FindObjectsOfType<DonRac>();
        if (dongRacConSot.Length > 0 && QuanLyKho.Instance != null)
        {
            int diemBiTru = dongRacConSot.Length * 5;
            QuanLyKho.Instance.DiemTinhLang -= diemBiTru;
            if (QuanLyKho.Instance.DiemTinhLang < 0) QuanLyKho.Instance.DiemTinhLang = 0;
            QuanLyKho.Instance.SaveGame();

            foreach (DonRac rac in dongRacConSot) Destroy(rac.gameObject);
        }

        // TỰ ĐỘNG CHUYỂN NGÀY MỚI MÀ KHÔNG CẦN HIỆN BẢNG
        SangNgayMoi();
    }

    public void SangNgayMoi()
    {
        Debug.Log("🎯 [GameLoop] TỰ ĐỘNG SANG NGÀY MỚI (TĂNG NGÀY)");
        try
        {
            if (QuanLyKho.Instance != null)
            {
                QuanLyKho.Instance.DoanhThuNgay = 0;
                QuanLyKho.Instance.ChiPhiNgay = 0;
                QuanLyKho.Instance.RandomGiaThiTruong();
            }

            if (bangKetToanPanel != null) bangKetToanPanel.SetActive(false);

            PoliceAI.ResetCongAnNgayMoi();

            if (TiemTraGenZ.Manager.StoryManager.Instance != null)
            {
                TiemTraGenZ.Manager.StoryManager.Instance.AdvanceDay();
            }

            Time.timeScale = 1;
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
        }
        catch (System.Exception e)
        {
            Debug.LogError("Lỗi ngầm nhưng đã cứu: " + e.Message);
            Time.timeScale = 1;
        }
    }
}