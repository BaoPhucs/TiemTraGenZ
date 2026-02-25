using UnityEngine;
using TMPro;

public class GameLoopManager : MonoBehaviour
{
    [Header("UI Kết Toán")]
    public GameObject bangKetToanPanel;
    public TextMeshProUGUI txtDoanhThu;
    public TextMeshProUGUI txtChiPhi;
    public TextMeshProUGUI txtLoiNhuan;

    void Start()
    {
        // Tự động tìm Panel nếu chưa gán (để tránh lỗi khi chuyển scene)
        if (bangKetToanPanel == null)
            bangKetToanPanel = GameObject.Find("BangKetToan_Panel");

        // Bật bảng khi mới vào game (theo yêu cầu của bạn để tiếp nối Intro)
        if (bangKetToanPanel != null) 
        {
            bangKetToanPanel.SetActive(true);
            
            // Hiện chuột để người chơi thao tác (nếu cần)
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
        }
    }

    void Update()
    {
        // Logic bấm Enter để bắt đầu ngày mới (chuyển từ bảng kết toán -> game)
        if (bangKetToanPanel != null && bangKetToanPanel.activeSelf)
        {
            if (Input.GetKeyDown(KeyCode.Return) || Input.GetKeyDown(KeyCode.KeypadEnter))
            {
                SangNgayMoi();
            }
        }
    }

    // --- DÒNG NÀY TẠO NÚT TEST TRONG MENU CHUỘT PHẢI ---
    [ContextMenu("TEST KET THUC")]
    public void KetThucNgay()
    {
        if (bangKetToanPanel != null) bangKetToanPanel.SetActive(true);

        // Lấy số liệu từ Kho (đảm bảo Kho không null)
        if (QuanLyKho.Instance != null)
        {
            int doanhThu = QuanLyKho.Instance.DoanhThuNgay;
            int chiPhi = QuanLyKho.Instance.ChiPhiNgay;
            int loiNhuan = doanhThu - chiPhi;

            // Cập nhật text
            if (txtDoanhThu) txtDoanhThu.text = "Doanh Thu: " + doanhThu.ToString("n0") + "đ";
            if (txtChiPhi) txtChiPhi.text = "Chi Phí: " + chiPhi.ToString("n0") + "đ";
            if (txtLoiNhuan) txtLoiNhuan.text = "Loi Nhuan: " + loiNhuan.ToString("n0") + "đ";
        }

        // Dừng game và hiện chuột
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

        Time.timeScale = 1;

        // Khóa chuột lại để chơi tiếp
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        Debug.Log("Đã sang ngày mới!");
    }
}