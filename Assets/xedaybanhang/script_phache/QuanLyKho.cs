using UnityEngine;

public class QuanLyKho : MonoBehaviour
{
    public static QuanLyKho Instance; // Singleton để gọi từ bất cứ đâu

    [Header("=== TÀI CHÍNH ===")]
    public int TienHienCo = 50000; // Vốn khởi nghiệp

    [Header("=== KHO NGUYÊN LIỆU ===")]
    public int Tra = 10;      // Đơn vị: Gói
    public int Duong = 10;    // Đơn vị: Hũ
    public int Tac = 10;      // Đơn vị: Quả
    public int Da = 20;       // Đơn vị: Bao
    public int LyNhua = 20;   // Đơn vị: Cái

    [Header("=== QUẢN LÝ BÀN GHẾ (Vật lý) ===")]
    public int soDoBenNgoai = 0; // Biến đếm tổng

    [Header("Giới hạn số lượng bàn ghế")]
    public int maxGhe = 6;
    public int maxBan = 2;
    public int maxThungDa = 1;

    [Header("Đếm số lượng đã lấy ra")]
    public int gheDaLay = 0;
    public int banDaLay = 0;
    public int thungDaDaLay = 0;

    // Tham chiếu (Không bắt buộc dùng trong Update nữa vì PlayerInteraction đã lo)
    public PushableCart boDayXe;

    void Awake()
    {
        Instance = this;
    }

    void Update()
    {
        if (boDayXe != null) boDayXe.enabled = (soDoBenNgoai == 0);
    }

    public bool SuDungNguyenLieu(string tenMon)
    {
        switch (tenMon)
        {
            case "Ly":
                if (LyNhua > 0) { LyNhua--; return true; }
                break;
            case "Tra":
                if (Tra > 0) { Tra--; return true; }
                break;
            case "Duong": // Nếu công thức có đường
                if (Duong > 0) { Duong--; return true; }
                break;
            case "Tac":
                if (Tac > 0) { Tac--; return true; }
                break;
            case "Da":
                if (Da > 0) { Da--; return true; }
                break;
        }

        Debug.Log("HẾT HÀNG: " + tenMon);
        return false; // Hết hàng
    }
    // --- PHẦN 1: QUẢN LÝ BÀN GHẾ (Logic Cũ) ---

    // Hàm gọi khi Lấy đồ ra
    public bool LayDoRa(string loaiDo)
    {
        if (loaiDo == "Ghe" && gheDaLay < maxGhe)
        {
            gheDaLay++;
            soDoBenNgoai++;
            return true;
        }
        if (loaiDo == "Ban" && banDaLay < maxBan)
        {
            banDaLay++;
            soDoBenNgoai++;
            return true;
        }
        if (loaiDo == "ThungDa" && thungDaDaLay < maxThungDa)
        {
            thungDaDaLay++;
            soDoBenNgoai++;
            return true;
        }

        Debug.Log("Hết " + loaiDo + " trong kho rồi!");
        return false;
    }

    // Hàm gọi khi Thu hồi đồ về
    public void ThuDoVe(string loaiDo)
    {
        if (soDoBenNgoai > 0) soDoBenNgoai--;

        if (loaiDo == "Ghe" && gheDaLay > 0) gheDaLay--;
        if (loaiDo == "Ban" && banDaLay > 0) banDaLay--;
        if (loaiDo == "ThungDa" && thungDaDaLay > 0) thungDaDaLay--;
    }

    // Hàm kiểm tra để chặn đẩy xe (Script PlayerInteraction sẽ gọi hàm này)
    public bool ConDoBenNgoai()
    {
        if (banDaLay > 0) return true;
        if (gheDaLay > 0) return true;
        if (thungDaDaLay > 0) return true;

        return false; // Sạch sẽ
    }

    // --- PHẦN 2: QUẢN LÝ KINH TẾ (Logic Mới) ---

    // Hàm kiểm tra xem còn đủ nguyên liệu pha chế không
    public bool KiemTraNguyenLieu(int canTra, int canDuong, int canTac, int canDa)
    {
        if (Tra < canTra) return false;
        if (Duong < canDuong) return false;
        if (Tac < canTac) return false;
        if (Da < canDa) return false;
        if (LyNhua < 1) return false; // Luôn tốn 1 ly
        return true;
    }

    // Hàm trừ nguyên liệu sau khi pha xong
    public void TieuThuNguyenLieu(int canTra, int canDuong, int canTac, int canDa)
    {
        Tra -= canTra;
        Duong -= canDuong;
        Tac -= canTac;
        Da -= canDa;
        LyNhua -= 1;
        // Debug.Log("Đã pha chế! Ly nhựa còn: " + LyNhua);
    }

    // Hàm mua hàng (Gọi từ ShopManager)
    public void MuaHang(string tenMon, int soLuong, int giaTien)
    {
        if (TienHienCo >= giaTien)
        {
            TienHienCo -= giaTien;
            switch (tenMon)
            {
                case "Tra": Tra += soLuong; break;
                case "Duong": Duong += soLuong; break;
                case "Tac": Tac += soLuong; break;
                case "Da": Da += soLuong; break;
                case "Ly": LyNhua += soLuong; break;
            }
            Debug.Log($"Mua thành công {soLuong} {tenMon}. Tiền còn: {TienHienCo}");
        }
        else
        {
            Debug.Log("Không đủ tiền!");
        }
    }

    // Hàm cộng tiền (Khi bán được nước)
    public void CongTien(int soTien)
    {
        TienHienCo += soTien;
    }
}