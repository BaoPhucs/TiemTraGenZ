using UnityEngine;

public class QuanLyKho : MonoBehaviour
{
    public static QuanLyKho Instance;

    [Header("=== TÀI CHÍNH & AUDIO ===")]
    public int TienHienCo = 50000;
    public AudioSource amThanhTien; // Kéo AudioSource vào đây

    [Header("=== THỐNG KÊ NGÀY ===")]
    public int DoanhThuNgay = 0;
    public int ChiPhiNgay = 0;

    [Header("=== BIẾN ĐỘNG THỊ TRƯỜNG (TASK 3) ===")]
    public float tiLeGiaHomNay = 1.0f; // 1.0 là bình thường, 1.2 là đắt, 0.8 là rẻ

    [Header("=== KHO NGUYÊN LIỆU ===")]
    public int Tra = 10;
    public int Tac = 10;
    public int Da = 20;
    public int LyNhua = 20;

    [Header("=== NÂNG CẤP (TASK 1) ===")]
    public int maxGhe = 6;     // Số ghế tối đa
    public int maxBan = 2;     // Số bàn tối đa
    public int maxThungDa = 1;

    [Header("=== QUẢN LÝ VẬT LÝ (Giữ nguyên) ===")]
    public int soDoBenNgoai = 0;
    public int gheDaLay = 0;
    public int banDaLay = 0;
    public int thungDaDaLay = 0;
    public PushableCart boDayXe;

    void Awake()
    {
        Instance = this;
        LoadGame();
        RandomGiaThiTruong(); // Đầu ngày random giá luôn
    }

    void Update()
    {
        if (boDayXe != null) boDayXe.enabled = (soDoBenNgoai == 0);
    }

    // --- HỆ THỐNG BIẾN ĐỘNG GIÁ ---
    public void RandomGiaThiTruong()
    {
        // Random từ 80% đến 120% giá gốc
        tiLeGiaHomNay = Random.Range(0.8f, 1.2f);
        Debug.Log($"Thị trường hôm nay: {tiLeGiaHomNay * 100}% giá gốc");
    }

    // --- HỆ THỐNG NÂNG CẤP BÀN GHẾ ---
    public void NangCapBanGhe(string loai, int giaTien)
    {
        if (TienHienCo >= giaTien)
        {
            TienHienCo -= giaTien;
            ChiPhiNgay += giaTien;

            // Hiệu ứng trừ tiền
            if (EffectManager.Instance != null) EffectManager.Instance.HienThiTien(-giaTien);
            if (amThanhTien != null) amThanhTien.Play();

            if (loai == "Ghe") maxGhe++;
            if (loai == "Ban") maxBan++;

            Debug.Log($"Đã nâng cấp {loai}. Giới hạn mới: Bàn {maxBan} - Ghế {maxGhe}");
            SaveGame();
        }
        else Debug.Log("Không đủ tiền nâng cấp!");
    }

    // --- HỆ THỐNG MUA HÀNG (Đã cập nhật Biến Động Giá) ---
    public void MuaHang(string tenMon, int soLuong, int giaGoc)
    {
        // Tính giá thực tế theo thị trường
        int giaThucTe = Mathf.RoundToInt(giaGoc * tiLeGiaHomNay);

        if (TienHienCo >= giaThucTe)
        {
            TienHienCo -= giaThucTe;
            ChiPhiNgay += giaThucTe;

            // Hiệu ứng & Âm thanh
            if (EffectManager.Instance != null) EffectManager.Instance.HienThiTien(-giaThucTe);
            if (amThanhTien != null) amThanhTien.Play();

            switch (tenMon)
            {
                case "Tra": Tra += soLuong; break;
                case "Tac": Tac += soLuong; break;
                case "Da": Da += soLuong; break;
                case "Ly": LyNhua += soLuong; break;
            }
            SaveGame();
        }
        else Debug.Log("Không đủ tiền (Giá hôm nay cao quá)!");
    }

    public void NhanTienBanNuoc(int soTien)
    {
        TienHienCo += soTien;
        DoanhThuNgay += soTien;

        // Hiệu ứng & Âm thanh
        if (EffectManager.Instance != null) EffectManager.Instance.HienThiTien(soTien);
        if (amThanhTien != null) amThanhTien.Play();

        SaveGame();
    }

    // --- SAVE / LOAD (Cập nhật lưu maxBan, maxGhe) ---
    public void SaveGame()
    {
        PlayerPrefs.SetInt("Tien", TienHienCo);
        PlayerPrefs.SetInt("Tra", Tra);
        PlayerPrefs.SetInt("Tac", Tac);
        PlayerPrefs.SetInt("Da", Da);
        PlayerPrefs.SetInt("Ly", LyNhua);
        // Lưu thêm cấp độ bàn ghế
        PlayerPrefs.SetInt("MaxGhe", maxGhe);
        PlayerPrefs.SetInt("MaxBan", maxBan);
        PlayerPrefs.Save();
    }

    public void LoadGame()
    {
        if (PlayerPrefs.HasKey("Tien"))
        {
            TienHienCo = PlayerPrefs.GetInt("Tien");
            Tra = PlayerPrefs.GetInt("Tra");
            Tac = PlayerPrefs.GetInt("Tac");
            Da = PlayerPrefs.GetInt("Da");
            LyNhua = PlayerPrefs.GetInt("Ly");
            // Tải cấp độ bàn ghế (Nếu chưa có thì lấy số mặc định 6 và 2)
            maxGhe = PlayerPrefs.GetInt("MaxGhe", 6);
            maxBan = PlayerPrefs.GetInt("MaxBan", 2);
        }
    }

    // ... (Giữ nguyên các hàm SuDungNguyenLieu, LayDoRa, ThuDoVe, KiemTraNguyenLieu, TieuThuNguyenLieu cũ) ...
    public bool SuDungNguyenLieu(string tenMon)
    {
        bool ck = false;
        switch (tenMon)
        {
            case "Ly": if (LyNhua > 0) { LyNhua--; ck = true; } break;
            case "Tra": if (Tra > 0) { Tra--; ck = true; } break;
            case "Tac": if (Tac > 0) { Tac--; ck = true; } break;
            case "Da": if (Da > 0) { Da--; ck = true; } break;
        }
        if (ck) SaveGame();
        return ck;
    }

    public bool LayDoRa(string loaiDo)
    {
        if (loaiDo == "Ghe" && gheDaLay < maxGhe) { gheDaLay++; soDoBenNgoai++; return true; }
        if (loaiDo == "Ban" && banDaLay < maxBan) { banDaLay++; soDoBenNgoai++; return true; }
        if (loaiDo == "ThungDa" && thungDaDaLay < maxThungDa) { thungDaDaLay++; soDoBenNgoai++; return true; }
        return false;
    }

    public void ThuDoVe(string loaiDo)
    {
        if (soDoBenNgoai > 0) soDoBenNgoai--;
        if (loaiDo == "Ghe" && gheDaLay > 0) gheDaLay--;
        if (loaiDo == "Ban" && banDaLay > 0) banDaLay--;
        if (loaiDo == "ThungDa" && thungDaDaLay > 0) thungDaDaLay--;
    }
    public bool ConDoBenNgoai() { return soDoBenNgoai > 0; }
}