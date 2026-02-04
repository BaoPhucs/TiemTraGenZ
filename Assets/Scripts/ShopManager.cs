using UnityEngine;
using TMPro;

public class ShopManager : MonoBehaviour
{
    [Header("THAM CHIẾU")]
    public QuanLyKho khoHang;
    public GameObject shopPanel;
    public TextMeshProUGUI txtTien;

    [Header("TEXT SỐ LƯỢNG")]
    public TextMeshProUGUI txtSlTra;
    public TextMeshProUGUI txtSlDuong;
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
        if (khoHang == null) return;

        if (txtTien != null) txtTien.text = "Vốn: " + khoHang.TienHienCo.ToString("n0") + " đ";

        if (txtSlTra != null) txtSlTra.text = "Trà: " + khoHang.Tra;
        if (txtSlDuong != null) txtSlDuong.text = "Đường: " + khoHang.Duong;
        if (txtSlTac != null) txtSlTac.text = "Tắc: " + khoHang.Tac;
        if (txtSlDa != null) txtSlDa.text = "Đá: " + khoHang.Da;
        if (txtSlLy != null) txtSlLy.text = "Ly: " + khoHang.LyNhua;
    }

    // Các hàm mua hàng giữ nguyên
    public void MuaTra() { if (khoHang != null) khoHang.MuaHang("Tra", 5, 2000); }
    public void MuaDuong() { if (khoHang != null) khoHang.MuaHang("Duong", 2, 1000); }
    public void MuaTac() { if (khoHang != null) khoHang.MuaHang("Tac", 5, 500); }
    public void MuaDa() { if (khoHang != null) khoHang.MuaHang("Da", 1, 1000); }
    public void MuaLy() { if (khoHang != null) khoHang.MuaHang("Ly", 50, 10000); }
}