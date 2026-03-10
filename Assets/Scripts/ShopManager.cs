using TiemTraGenZ.Manager;
using TMPro;
using UnityEngine;

public class ShopManager : MonoBehaviour
{
    public static ShopManager Instance;

    [Header("THAM CHIẾU")]
    public QuanLyKho khoHang;

    [Header("CÁC BẢNG UI (KÉO TỪ CANVAS VÀO)")]
    public GameObject panelNguyenLieu; // UI NPC Tạp Hóa
    public GameObject panelCongThuc;   // UI NPC Sư Phụ
    public GameObject panelTuiDo;      // Kéo Khung_HienThiSoLuong vào đây

    [Header("=== TEXT: SHOP CÔNG THỨC (SƯ PHỤ) ===")]
    public TextMeshProUGUI txtTien_BiKip;

    [Header("=== TEXT: SHOP NGUYÊN LIỆU ===")]
    public TextMeshProUGUI txtTien;
    public TextMeshProUGUI txtSlTra;
    public TextMeshProUGUI txtSlTac;
    public TextMeshProUGUI txtSlDa;
    public TextMeshProUGUI txtSlLy;
    public TextMeshProUGUI txtSlChanh;
    public TextMeshProUGUI txtSlTraSua;
    public TextMeshProUGUI txtSlMatcha;
    public TextMeshProUGUI txtSlSua;
    public TextMeshProUGUI txtSlCaPhe;

    [Header("=== TEXT: TÚI ĐỒ BALO (PHÍM M) ===")]
    public TextMeshProUGUI txtTien_Balo;
    public TextMeshProUGUI txtSlTra_Balo;
    public TextMeshProUGUI txtSlTac_Balo;
    public TextMeshProUGUI txtSlDa_Balo;
    public TextMeshProUGUI txtSlLy_Balo;
    public TextMeshProUGUI txtSlChanh_Balo;
    public TextMeshProUGUI txtSlTraSua_Balo;
    public TextMeshProUGUI txtSlMatcha_Balo;
    public TextMeshProUGUI txtSlSua_Balo;
    public TextMeshProUGUI txtSlCaPhe_Balo;

    void Awake()
    {
        Instance = this;
    }

    void Start()
    {
        DongTatCaShop();
        CapNhatGiaoDien();
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            DongTatCaShop();
        }

        if (Input.GetKeyDown(KeyCode.M))
        {
            if (panelTuiDo != null && panelTuiDo.activeSelf)
            {
                DongTatCaShop();
            }
            else
            {
                MoTuiDo();
            }
        }

        if ((panelNguyenLieu != null && panelNguyenLieu.activeSelf) ||
            (panelCongThuc != null && panelCongThuc.activeSelf) ||
            (panelTuiDo != null && panelTuiDo.activeSelf))
        {
            CapNhatGiaoDien();
        }
    }

    // =========================================================
    // VỆ SĨ BẢO VỆ CHUỘT: LUÔN ÉP HIỆN CHUỘT KHI ĐANG MỞ UI
    // =========================================================
    void LateUpdate()
    {
        bool dangMoUI = (panelNguyenLieu != null && panelNguyenLieu.activeSelf) ||
                        (panelCongThuc != null && panelCongThuc.activeSelf) ||
                        (panelTuiDo != null && panelTuiDo.activeSelf);

        if (dangMoUI)
        {
            // Ép hệ thống thả chuột ra liên tục, không cho Player cướp khi Click
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
        }
    }

    public void MoShopNguyenLieu()
    {
        DongTatCaShop();
        if (panelNguyenLieu != null) panelNguyenLieu.SetActive(true);
        KhoaChuot(false);
    }

    public void MoShopCongThuc()
    {
        DongTatCaShop();
        if (panelCongThuc != null) panelCongThuc.SetActive(true);
        KhoaChuot(false);
    }

    public void MoTuiDo()
    {
        DongTatCaShop();
        if (panelTuiDo != null) panelTuiDo.SetActive(true);

        // ĐÃ SỬA LỖI: Báo hệ thống thả con chuột ra để người chơi xem đồ
        KhoaChuot(false);
    }

    public void DongTatCaShop()
    {
        if (panelNguyenLieu != null) panelNguyenLieu.SetActive(false);
        if (panelCongThuc != null) panelCongThuc.SetActive(false);
        if (panelTuiDo != null) panelTuiDo.SetActive(false);
        KhoaChuot(true);
    }

    void KhoaChuot(bool khoa)
    {
        Cursor.lockState = khoa ? CursorLockMode.Locked : CursorLockMode.None;
        Cursor.visible = !khoa;
    }

    void CapNhatGiaoDien()
    {
        if (khoHang == null) khoHang = QuanLyKho.Instance;
        if (khoHang == null) return;

        string chuoiTien = "Vốn: " + khoHang.TienHienCo.ToString("n0") + " đ";

        if (txtTien_BiKip != null) txtTien_BiKip.text = chuoiTien;

        if (txtTien != null) txtTien.text = chuoiTien;
        if (txtSlTra != null) txtSlTra.text = "Trà: " + khoHang.Tra;
        if (txtSlTac != null) txtSlTac.text = "Tắc: " + khoHang.Tac;
        if (txtSlDa != null) txtSlDa.text = "Đá: " + khoHang.Da;
        if (txtSlLy != null) txtSlLy.text = "Ly: " + khoHang.LyNhua;
        if (txtSlChanh != null) txtSlChanh.text = "Chanh: " + khoHang.Chanh;
        if (txtSlTraSua != null) txtSlTraSua.text = "Trà Sua: " + khoHang.TraSua;
        if (txtSlMatcha != null) txtSlMatcha.text = "Matcha: " + khoHang.Matcha;
        if (txtSlSua != null) txtSlSua.text = "Sua Tươi: " + khoHang.Sua;
        if (txtSlCaPhe != null) txtSlCaPhe.text = "Cà Phê: " + khoHang.CaPhe;

        if (txtTien_Balo != null) txtTien_Balo.text = chuoiTien;
        if (txtSlTra_Balo != null) txtSlTra_Balo.text = "Trà: " + khoHang.Tra;
        if (txtSlTac_Balo != null) txtSlTac_Balo.text = "Tắc: " + khoHang.Tac;
        if (txtSlDa_Balo != null) txtSlDa_Balo.text = "Đá: " + khoHang.Da;
        if (txtSlLy_Balo != null) txtSlLy_Balo.text = "Ly: " + khoHang.LyNhua;
        if (txtSlChanh_Balo != null) txtSlChanh_Balo.text = "Chanh: " + khoHang.Chanh;
        if (txtSlTraSua_Balo != null) txtSlTraSua_Balo.text = "Trà Sua: " + khoHang.TraSua;
        if (txtSlMatcha_Balo != null) txtSlMatcha_Balo.text = "Matcha: " + khoHang.Matcha;
        if (txtSlSua_Balo != null) txtSlSua_Balo.text = "Sua Tươi: " + khoHang.Sua;
        if (txtSlCaPhe_Balo != null) txtSlCaPhe_Balo.text = "Cà Phê: " + khoHang.CaPhe;
    }

    void CapNhatTienSangStory() { if (khoHang != null && StoryManager.Instance != null) StoryManager.Instance.capital = khoHang.TienHienCo; }

    public void MuaTra() { if (khoHang != null) khoHang.MuaHang("Tra", 10, 7000); CapNhatTienSangStory(); }
    public void MuaTac() { if (khoHang != null) khoHang.MuaHang("Tac", 10, 10000); CapNhatTienSangStory(); }
    public void MuaDa() { if (khoHang != null) khoHang.MuaHang("Da", 20, 5000); CapNhatTienSangStory(); }
    public void MuaLy() { if (khoHang != null) khoHang.MuaHang("Ly", 50, 15000); CapNhatTienSangStory(); }
    public void MuaChanh() { if (khoHang != null) khoHang.MuaHang("Chanh", 10, 12000); CapNhatTienSangStory(); }
    public void MuaTraSua() { if (khoHang != null) khoHang.MuaHang("TraSua", 10, 20000); CapNhatTienSangStory(); }
    public void MuaMatcha() { if (khoHang != null) khoHang.MuaHang("Matcha", 10, 30000); CapNhatTienSangStory(); }
    public void MuaSua() { if (khoHang != null) khoHang.MuaHang("Sua", 10, 15000); CapNhatTienSangStory(); }
    public void MuaCaPhe() { if (khoHang != null) khoHang.MuaHang("CaPhe", 10, 18000); CapNhatTienSangStory(); }

    public void MuaCongThucTraTac() { if (khoHang != null && !khoHang.unlockTraTac) khoHang.MuaCongThuc("TraTac", 50000); CapNhatTienSangStory(); }
    public void MuaCongThucTraChanh() { if (khoHang != null && !khoHang.unlockTraChanh) khoHang.MuaCongThuc("TraChanh", 150000); CapNhatTienSangStory(); }
    public void MuaCongThucTraSua() { if (khoHang != null && !khoHang.unlockTraSua) khoHang.MuaCongThuc("TraSua", 300000); CapNhatTienSangStory(); }
    public void MuaCongThucMatcha() { if (khoHang != null && !khoHang.unlockMatcha) khoHang.MuaCongThuc("MatchaLatte", 500000); CapNhatTienSangStory(); }
    public void MuaCongThucCaPheDen() { if (khoHang != null && !khoHang.unlockCaPheDen) khoHang.MuaCongThuc("CaPheDen", 100000); CapNhatTienSangStory(); }
    public void MuaCongThucCaPheSua() { if (khoHang != null && !khoHang.unlockCaPheSua) khoHang.MuaCongThuc("CaPheSua", 200000); CapNhatTienSangStory(); }
    public void MuaGheMoi() { if (khoHang != null) khoHang.NangCapBanGhe("Ghe", 30000); CapNhatTienSangStory(); }
    public void MuaBanMoi() { if (khoHang != null) khoHang.NangCapBanGhe("Ban", 50000); CapNhatTienSangStory(); }
}