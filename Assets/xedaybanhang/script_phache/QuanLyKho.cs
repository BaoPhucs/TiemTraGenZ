using UnityEngine;

public class QuanLyKho : MonoBehaviour
{
    public static QuanLyKho Instance; // Singleton để gọi từ bất cứ đâu

    [Header("=== TÀI CHÍNH ===")]
    public int TienHienCo = 50000; // Vốn khởi nghiệp

    [Header("=== THỐNG KÊ TRONG NGÀY (MỚI) ===")]
    // Hai biến này dùng để hiện lên bảng kết toán cuối ngày
    public int DoanhThuNgay = 0;
    public int ChiPhiNgay = 0;

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

    // Tham chiếu
    public PushableCart boDayXe;

    void Awake()
    {
        Instance = this;
        LoadGame(); // <--- MỚI: Tự động tải dữ liệu cũ khi vào game
    }

    void Update()
    {
        if (boDayXe != null) boDayXe.enabled = (soDoBenNgoai == 0);
    }

    // --- HỆ THỐNG LƯU / TẢI GAME (MỚI) ---
    public void SaveGame()
    {
        PlayerPrefs.SetInt("Tien", TienHienCo);
        PlayerPrefs.SetInt("Tra", Tra);
        PlayerPrefs.SetInt("Duong", Duong);
        PlayerPrefs.SetInt("Tac", Tac);
        PlayerPrefs.SetInt("Da", Da);
        PlayerPrefs.SetInt("Ly", LyNhua);
        PlayerPrefs.Save();
        // Debug.Log("Đã lưu game!");
    }

    public void LoadGame()
    {
        // Chỉ tải nếu đã từng lưu trước đó
        if (PlayerPrefs.HasKey("Tien"))
        {
            TienHienCo = PlayerPrefs.GetInt("Tien");
            Tra = PlayerPrefs.GetInt("Tra");
            Duong = PlayerPrefs.GetInt("Duong");
            Tac = PlayerPrefs.GetInt("Tac");
            Da = PlayerPrefs.GetInt("Da");
            LyNhua = PlayerPrefs.GetInt("Ly");
        }
    }

    // --- LOGIC KHO & PHA CHẾ (Đã thêm Lưu Game) ---
    public bool SuDungNguyenLieu(string tenMon)
    {
        bool thanhCong = false;
        switch (tenMon)
        {
            case "Ly":
                if (LyNhua > 0) { LyNhua--; thanhCong = true; }
                break;
            case "Tra":
                if (Tra > 0) { Tra--; thanhCong = true; }
                break;
            case "Duong":
                if (Duong > 0) { Duong--; thanhCong = true; }
                break;
            case "Tac":
                if (Tac > 0) { Tac--; thanhCong = true; }
                break;
            case "Da":
                if (Da > 0) { Da--; thanhCong = true; }
                break;
        }

        if (thanhCong)
        {
            SaveGame(); // Dùng nguyên liệu xong là lưu lại ngay
            return true;
        }

        Debug.Log("HẾT HÀNG: " + tenMon);
        return false; // Hết hàng
    }

    // --- PHẦN 1: QUẢN LÝ BÀN GHẾ (Logic Cũ - Giữ nguyên 100%) ---

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

    public void ThuDoVe(string loaiDo)
    {
        if (soDoBenNgoai > 0) soDoBenNgoai--;

        if (loaiDo == "Ghe" && gheDaLay > 0) gheDaLay--;
        if (loaiDo == "Ban" && banDaLay > 0) banDaLay--;
        if (loaiDo == "ThungDa" && thungDaDaLay > 0) thungDaDaLay--;
    }

    public bool ConDoBenNgoai()
    {
        if (banDaLay > 0) return true;
        if (gheDaLay > 0) return true;
        if (thungDaDaLay > 0) return true;

        return false;
    }

    // --- PHẦN 2: QUẢN LÝ KINH TẾ (Logic Mới + Hiệu ứng) ---

    // Hàm cũ giữ lại để tương thích (nếu có script nào gọi)
    public bool KiemTraNguyenLieu(int canTra, int canDuong, int canTac, int canDa)
    {
        if (Tra < canTra) return false;
        if (Duong < canDuong) return false;
        if (Tac < canTac) return false;
        if (Da < canDa) return false;
        if (LyNhua < 1) return false;
        return true;
    }

    public void TieuThuNguyenLieu(int canTra, int canDuong, int canTac, int canDa)
    {
        Tra -= canTra;
        Duong -= canDuong;
        Tac -= canTac;
        Da -= canDa;
        LyNhua -= 1;
        SaveGame(); // Lưu game
    }

    // Hàm mua hàng (Đã cập nhật Hiệu ứng + Báo cáo ngày)
    public void MuaHang(string tenMon, int soLuong, int giaTien)
    {
        if (TienHienCo >= giaTien)
        {
            TienHienCo -= giaTien;

            // 1. Thống kê chi phí trong ngày
            ChiPhiNgay += giaTien;

            // 2. Hiệu ứng chữ bay (Nếu có EffectManager)
            if (EffectManager.Instance != null)
                EffectManager.Instance.HienThiTien(-giaTien);

            switch (tenMon)
            {
                case "Tra": Tra += soLuong; break;
                case "Duong": Duong += soLuong; break;
                case "Tac": Tac += soLuong; break;
                case "Da": Da += soLuong; break;
                case "Ly": LyNhua += soLuong; break;
            }

            Debug.Log($"Mua thành công {soLuong} {tenMon}. Tiền còn: {TienHienCo}");
            SaveGame(); // 3. Lưu game ngay lập tức
        }
        else
        {
            Debug.Log("Không đủ tiền!");
        }
    }

    // Hàm cộng tiền (Đã cập nhật Hiệu ứng + Báo cáo ngày)
    // Sau này khi bán được nước, bạn gọi hàm này là xong
    public void NhanTienBanNuoc(int soTien)
    {
        TienHienCo += soTien;

        // 1. Thống kê doanh thu
        DoanhThuNgay += soTien;

        // 2. Hiệu ứng chữ bay
        if (EffectManager.Instance != null)
            EffectManager.Instance.HienThiTien(soTien);

        SaveGame(); // 3. Lưu lại tiền
    }

    // Hàm cũ (giữ lại để tránh lỗi code cũ gọi nó)
    public void CongTien(int soTien)
    {
        NhanTienBanNuoc(soTien);
    }
}