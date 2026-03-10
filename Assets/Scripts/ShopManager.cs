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
    public GameObject panelTuiDo;      // UI Balo (Bấm phím M) - THÊM MỚI Ở ĐÂY

    [Header("TEXT HIỂN THỊ SỐ LƯỢNG")]
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
        // Bấm phím ESC để đóng mọi UI đang mở
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            DongTatCaShop();
        }

        // --- CHỨC NĂNG MỚI CỦA PHÍM M (MỞ BALO) ---
        if (Input.GetKeyDown(KeyCode.M))
        {
            if (panelTuiDo != null && panelTuiDo.activeSelf)
            {
                DongTatCaShop(); // Nếu Balo đang mở thì đóng lại
            }
            else
            {
                MoTuiDo(); // Nếu đang đóng thì mở Balo ra xem
            }
        }

        // Cập nhật số lượng liên tục nếu 1 trong 3 bảng đang được mở
        if ((panelNguyenLieu != null && panelNguyenLieu.activeSelf) ||
            (panelCongThuc != null && panelCongThuc.activeSelf) ||
            (panelTuiDo != null && panelTuiDo.activeSelf))
        {
            CapNhatGiaoDien();
        }
    }

    public void MoShopNguyenLieu()
    {
        DongTatCaShop(); // Đóng các bảng khác trước
        if (panelNguyenLieu != null) panelNguyenLieu.SetActive(true);
        KhoaChuot(false);
    }

    public void MoShopCongThuc()
    {
        DongTatCaShop(); // Đóng các bảng khác trước
        if (panelCongThuc != null) panelCongThuc.SetActive(true);
        KhoaChuot(false);
    }

    public void MoTuiDo()
    {
        DongTatCaShop(); // Đóng các bảng khác trước
        if (panelTuiDo != null) panelTuiDo.SetActive(true);
        KhoaChuot(false); // Hiện chuột (hoặc bạn có thể để true nếu chỉ muốn nhìn chứ ko cần chuột)
    }

    public void DongTatCaShop()
    {
        if (panelNguyenLieu != null) panelNguyenLieu.SetActive(false);
        if (panelCongThuc != null) panelCongThuc.SetActive(false);
        if (panelTuiDo != null) panelTuiDo.SetActive(false); // Đóng luôn Balo
        KhoaChuot(true); // Khóa chuột lại để chơi game
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

        if (txtTien != null) txtTien.text = "Vốn: " + khoHang.TienHienCo.ToString("n0") + " đ";
        if (txtSlTra != null) txtSlTra.text = "Trà: " + khoHang.Tra;
        if (txtSlTac != null) txtSlTac.text = "Tắc: " + khoHang.Tac;
        if (txtSlDa != null) txtSlDa.text = "Đá: " + khoHang.Da;
        if (txtSlLy != null) txtSlLy.text = "Ly: " + khoHang.LyNhua;
        if (txtSlChanh != null) txtSlChanh.text = "Chanh: " + khoHang.Chanh;
        if (txtSlTraSua != null) txtSlTraSua.text = "Trà Sữa: " + khoHang.TraSua;
        if (txtSlMatcha != null) txtSlMatcha.text = "Matcha: " + khoHang.Matcha;
        if (txtSlSua != null) txtSlSua.text = "Sữa Tươi: " + khoHang.Sua;
        if (txtSlCaPhe != null) txtSlCaPhe.text = "Cà Phê: " + khoHang.CaPhe;
    }

    void CapNhatTienSangStory() { if (khoHang != null && StoryManager.Instance != null) StoryManager.Instance.capital = khoHang.TienHienCo; }

    // --- CÁC HÀM MUA HÀNG & MUA CÔNG THỨC (GIỮ NGUYÊN) ---
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