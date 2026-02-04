using UnityEngine;
using TMPro;

public class ShopManager : MonoBehaviour
{
    [Header("THAM CHIẾU")]
    public QuanLyKho khoHang; // Kéo GameManager vào đây
    public GameObject shopPanel;
    public TextMeshProUGUI txtTien;

    [Header("TEXT SỐ LƯỢNG")]
    public TextMeshProUGUI txtSlTra;
    public TextMeshProUGUI txtSlTac;
    public TextMeshProUGUI txtSlDa;
    public TextMeshProUGUI txtSlLy;

    void Start()
    {
        if (shopPanel != null) shopPanel.SetActive(false);
        CapNhatGiaoDien();
        // Mới vào game thì khóa chuột để xoay camera
        KhoaChuot(true);
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.M)) ToggleShop();

        if (shopPanel != null && shopPanel.activeSelf)
        {
            CapNhatGiaoDien();
        }
    }

    public void ToggleShop()
    {
        if (shopPanel == null) return;

        bool isOpening = !shopPanel.activeSelf;
        shopPanel.SetActive(isOpening);

        // Mở shop -> Hiện chuột (False khóa). Đóng shop -> Khóa chuột (True khóa).
        KhoaChuot(!isOpening);
    }

    void KhoaChuot(bool khoa)
    {
        if (khoa)
        {
            Cursor.lockState = CursorLockMode.Locked; // Khóa giữa màn hình
            Cursor.visible = false; // Ẩn chuột
        }
        else
        {
            Cursor.lockState = CursorLockMode.None; // Thả tự do
            Cursor.visible = true; // Hiện chuột
        }
    }

    void CapNhatGiaoDien()
    {
        // Tự động tìm kho nếu lỡ quên kéo
        if (khoHang == null) khoHang = QuanLyKho.Instance;
        if (khoHang == null) return;

        if (txtTien != null) txtTien.text = "Vốn: " + khoHang.TienHienCo.ToString("n0") + " đ";

        if (txtSlTra != null) txtSlTra.text = "Trà: " + khoHang.Tra;
        if (txtSlTac != null) txtSlTac.text = "Tắc: " + khoHang.Tac;
        if (txtSlDa != null) txtSlDa.text = "Đá: " + khoHang.Da;
        if (txtSlLy != null) txtSlLy.text = "Ly: " + khoHang.LyNhua;
    }

    // --- CÁC HÀM MUA HÀNG (ĐÃ CẬP NHẬT GIÁ & SỐ LƯỢNG CHUẨN) ---
    // QuanLyKho sẽ tự tính toán giá thị trường dựa trên giá gốc này

    public void MuaTra()
    {
        if (khoHang != null) khoHang.MuaHang("Tra", 10, 7000); // 10 gói, gốc 2k
    }

    public void MuaTac()
    {
        if (khoHang != null) khoHang.MuaHang("Tac", 10, 10000); // 10 quả, gốc 2k
    }

    public void MuaDa()
    {
        if (khoHang != null) khoHang.MuaHang("Da", 20, 5000); // 20 bao, gốc 2k
    }

    public void MuaLy()
    {
        if (khoHang != null) khoHang.MuaHang("Ly", 50, 15000); // 50 cái, gốc 10k
    }

    // --- CÁC HÀM NÂNG CẤP (MỚI - GẮN VÀO NÚT MUA BÀN/GHẾ) ---

    public void MuaGheMoi()
    {
        // Mua thêm 1 cái ghế, giá 15.000đ
        if (khoHang != null) khoHang.NangCapBanGhe("Ghe", 30000);
    }

    public void MuaBanMoi()
    {
        // Mua thêm 1 cái bàn, giá 50.000đ
        if (khoHang != null) khoHang.NangCapBanGhe("Ban", 50000);
    }
}