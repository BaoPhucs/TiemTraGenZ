using UnityEngine;
using UnityEngine.AI;

public class QuanLyKho : MonoBehaviour
{
    public static QuanLyKho Instance;

    [Header("=== TÀI CHÍNH & AUDIO ===")]
    public int TienHienCo = 50000;
    public int TienNo = 200000;
    public AudioSource amThanhTien;

    [Header("=== THỐNG KÊ NGÀY ===")]
    public int DoanhThuNgay = 0;
    public int ChiPhiNgay = 0;

    [Header("=== BIẾN ĐỘNG THỊ TRƯỜNG ===")]
    public float tiLeGiaHomNay = 1.0f;

    [Header("=== KHO NGUYÊN LIỆU ===")]
    public int Tra = 10;
    public int Tac = 10;
    public int Da = 20;
    public int LyNhua = 20;
    public int Chanh = 10;
    public int TraSua = 10;
    public int Matcha = 10;
    public int Sua = 10;
    public int CaPhe = 10;

    [Header("=== MENU ĐỒ UỐNG (MỞ KHÓA) ===")]
    public bool unlockTraDa = true;   // Mặc định cho không
    public bool unlockTraTac = false;
    public bool unlockTraChanh = false;
    public bool unlockTraSua = false;
    public bool unlockMatcha = false;
    public bool unlockCaPheDen = false;
    public bool unlockCaPheSua = false;

    [Header("=== NÂNG CẤP BÀN GHẾ ===")]
    public int maxGhe = 6;
    public int maxBan = 2;
    public int maxThungDa = 1;

    [Header("=== QUẢN LÝ VẬT LÝ ===")]
    public int soDoBenNgoai = 0;
    public int gheDaLay = 0;
    public int banDaLay = 0;
    public int thungDaDaLay = 0;
    public PushableCart boDayXe;

    [Header("=== HỆ THỐNG VIRAL ===")]
    public int DiemViral = 0;
    public int DiemTinhLang = 10;

    void Awake()
    {
        Instance = this;
        LoadGame();
        RandomGiaThiTruong();
    }

    void Update()
    {
        if (boDayXe != null) boDayXe.enabled = (soDoBenNgoai == 0);

        // --- CÁC PHÍM HACK TEST GAME ---
        if (Input.GetKeyDown(KeyCode.F12)) { TienHienCo += 500000; SaveGame(); Debug.Log("HACK TIỀN!"); }
        if (Input.GetKeyDown(KeyCode.F8)) { TienNo += 200000; SaveGame(); Debug.Log("HACK NỢ!"); }
        if (Input.GetKeyDown(KeyCode.F1)) { ResetGameToZero(); } // Đã chuyển thành F1
    }

    public void RandomGiaThiTruong() { tiLeGiaHomNay = Random.Range(0.8f, 1.2f); }

    public void NangCapBanGhe(string loai, int giaTien)
    {
        if (TienHienCo >= giaTien)
        {
            TienHienCo -= giaTien;
            ChiPhiNgay += giaTien;
            if (EffectManager.Instance != null) EffectManager.Instance.HienThiTien(-giaTien);
            if (amThanhTien != null) amThanhTien.Play();
            if (loai == "Ghe") maxGhe++;
            if (loai == "Ban") maxBan++;
            SaveGame();
        }
    }

    public void MuaHang(string tenMon, int soLuong, int giaGoc)
    {
        int giaThucTe = Mathf.RoundToInt(giaGoc * tiLeGiaHomNay);
        if (TienHienCo >= giaThucTe)
        {
            TienHienCo -= giaThucTe;
            ChiPhiNgay += giaThucTe;
            if (EffectManager.Instance != null) EffectManager.Instance.HienThiTien(-giaThucTe);
            if (amThanhTien != null) amThanhTien.Play();

            switch (tenMon)
            {
                case "Tra": Tra += soLuong; break;
                case "Tac": Tac += soLuong; break;
                case "Da": Da += soLuong; break;
                case "Ly": LyNhua += soLuong; break;
                case "Chanh": Chanh += soLuong; break;
                case "TraSua": TraSua += soLuong; break;
                case "Matcha": Matcha += soLuong; break;
                case "Sua": Sua += soLuong; break;
                case "CaPhe": CaPhe += soLuong; break;
            }
            SaveGame();
        }
    }

    public bool MuaCongThuc(string tenMon, int giaTien)
    {
        if (TienHienCo >= giaTien)
        {
            TienHienCo -= giaTien;
            ChiPhiNgay += giaTien;
            if (EffectManager.Instance != null) EffectManager.Instance.HienThiTien(-giaTien);
            if (amThanhTien != null) amThanhTien.Play();

            if (tenMon == "TraTac") unlockTraTac = true;
            if (tenMon == "TraChanh") unlockTraChanh = true;
            if (tenMon == "TraSua") unlockTraSua = true;
            if (tenMon == "MatchaLatte") unlockMatcha = true;
            if (tenMon == "CaPheDen") unlockCaPheDen = true;
            if (tenMon == "CaPheSua") unlockCaPheSua = true;

            SaveGame();
            return true;
        }
        return false;
    }

    public void NhanTienBanNuoc(int soTien)
    {
        TienHienCo += soTien;
        DoanhThuNgay += soTien;
        if (EffectManager.Instance != null) EffectManager.Instance.HienThiTien(soTien);
        if (amThanhTien != null) amThanhTien.Play();
        SaveGame();
    }

    public void SaveGame()
    {
        PlayerPrefs.SetInt("Tien", TienHienCo);
        PlayerPrefs.SetInt("TienNo", TienNo);
        PlayerPrefs.SetInt("Tra", Tra);
        PlayerPrefs.SetInt("Tac", Tac);
        PlayerPrefs.SetInt("Da", Da);
        PlayerPrefs.SetInt("Ly", LyNhua);
        PlayerPrefs.SetInt("Chanh", Chanh);
        PlayerPrefs.SetInt("TraSua", TraSua);
        PlayerPrefs.SetInt("Matcha", Matcha);
        PlayerPrefs.SetInt("Sua", Sua);
        PlayerPrefs.SetInt("CaPhe", CaPhe);

        PlayerPrefs.SetInt("MaxGhe", maxGhe);
        PlayerPrefs.SetInt("MaxBan", maxBan);
        PlayerPrefs.SetInt("Viral", DiemViral);
        PlayerPrefs.SetInt("TinhLang", DiemTinhLang);

        PlayerPrefs.SetInt("UnlockTraTac", unlockTraTac ? 1 : 0);
        PlayerPrefs.SetInt("UnlockTraChanh", unlockTraChanh ? 1 : 0);
        PlayerPrefs.SetInt("UnlockTraSua", unlockTraSua ? 1 : 0);
        PlayerPrefs.SetInt("UnlockMatcha", unlockMatcha ? 1 : 0);
        PlayerPrefs.SetInt("UnlockCaPheDen", unlockCaPheDen ? 1 : 0);
        PlayerPrefs.SetInt("UnlockCaPheSua", unlockCaPheSua ? 1 : 0);

        PlayerPrefs.Save();
    }

    public void LoadGame()
    {
        if (PlayerPrefs.HasKey("Tien"))
        {
            TienHienCo = PlayerPrefs.GetInt("Tien");
            TienNo = PlayerPrefs.GetInt("TienNo", 200000);
            Tra = PlayerPrefs.GetInt("Tra");
            Tac = PlayerPrefs.GetInt("Tac");
            Da = PlayerPrefs.GetInt("Da");
            LyNhua = PlayerPrefs.GetInt("Ly");
            Chanh = PlayerPrefs.GetInt("Chanh", 0);
            TraSua = PlayerPrefs.GetInt("TraSua", 0);
            Matcha = PlayerPrefs.GetInt("Matcha", 0);
            Sua = PlayerPrefs.GetInt("Sua", 0);
            CaPhe = PlayerPrefs.GetInt("CaPhe", 0);

            maxGhe = PlayerPrefs.GetInt("MaxGhe", 6);
            maxBan = PlayerPrefs.GetInt("MaxBan", 2);
            DiemViral = PlayerPrefs.GetInt("Viral", 0);
            DiemTinhLang = PlayerPrefs.GetInt("TinhLang", 10);

            unlockTraTac = PlayerPrefs.GetInt("UnlockTraTac", 0) == 1;
            unlockTraChanh = PlayerPrefs.GetInt("UnlockTraChanh", 0) == 1;
            unlockTraSua = PlayerPrefs.GetInt("UnlockTraSua", 0) == 1;
            unlockMatcha = PlayerPrefs.GetInt("UnlockMatcha", 0) == 1;
            unlockCaPheDen = PlayerPrefs.GetInt("UnlockCaPheDen", 0) == 1;
            unlockCaPheSua = PlayerPrefs.GetInt("UnlockCaPheSua", 0) == 1;
        }
    }

    public bool SuDungNguyenLieu(string tenMon)
    {
        bool ck = false;
        switch (tenMon)
        {
            case "Ly": if (LyNhua > 0) { LyNhua--; ck = true; } break;
            case "Tra": if (Tra > 0) { Tra--; ck = true; } break;
            case "Tac": if (Tac > 0) { Tac--; ck = true; } break;
            case "Da": if (Da > 0) { Da--; ck = true; } break;
            case "Chanh": if (Chanh > 0) { Chanh--; ck = true; } break;
            case "TraSua": if (TraSua > 0) { TraSua--; ck = true; } break;
            case "Matcha": if (Matcha > 0) { Matcha--; ck = true; } break;
            case "Sua": if (Sua > 0) { Sua--; ck = true; } break;
            case "CaPhe": if (CaPhe > 0) { CaPhe--; ck = true; } break;
        }
        if (ck) SaveGame();
        return ck;
    }

    public System.Collections.Generic.List<string> LayMenuHienTai()
    {
        System.Collections.Generic.List<string> menu = new System.Collections.Generic.List<string>();
        if (unlockTraDa) menu.Add("TraDa");
        if (unlockTraTac) menu.Add("TraTac");
        if (unlockTraChanh) menu.Add("TraChanh");
        if (unlockTraSua) menu.Add("TraSua");
        if (unlockMatcha) menu.Add("MatchaLatte");
        if (unlockCaPheDen) menu.Add("CaPheDen");
        if (unlockCaPheSua) menu.Add("CaPheSua");
        return menu;
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
    public bool ConDoBenNgoai()
    {
        return soDoBenNgoai > 0;
    }

    // ==========================================
    // CÁC HÀM MỚI THÊM CHO LOGIC MUA VÉ SỐ
    // ==========================================
    public bool MuaVeSo(int giaVe)
    {
        if (TienHienCo >= giaVe)
        {
            TienHienCo -= giaVe;
            ChiPhiNgay += giaVe;
            if (EffectManager.Instance != null) EffectManager.Instance.HienThiTien(-giaVe);
            if (amThanhTien != null) amThanhTien.Play();
            SaveGame();
            return true; // Mua thành công
        }
        return false; // Nghèo quá không mua được
    }

    public void TrungDocDac(int tienThuong)
    {
        // ÉP KIỂU SANG LONG ĐỂ KIỂM TRA CHỐNG TRÀN BỘ NHỚ
        long kiemTraTien = (long)TienHienCo + (long)tienThuong;

        if (kiemTraTien > 2000000000)
        {
            TienHienCo = 2000000000; // Khóa trần ở mức 2 Tỷ
        }
        else
        {
            TienHienCo += tienThuong;
        }

        DoanhThuNgay += tienThuong; // Tính luôn vào doanh thu ngày
        if (EffectManager.Instance != null) EffectManager.Instance.HienThiTien(tienThuong);
        if (amThanhTien != null) amThanhTien.Play();
        SaveGame();
    }
    // ==========================================

    public void ResetGameToZero()
    {
        PlayerPrefs.DeleteAll();

        TienHienCo = 50000;
        TienNo = 200000;
        Tra = 10; Tac = 10; Da = 20; LyNhua = 20;
        Chanh = 10; TraSua = 10; Matcha = 10; Sua = 10; CaPhe = 10;

        maxGhe = 6; maxBan = 2; DiemViral = 0; DiemTinhLang = 10;

        unlockTraDa = true;
        unlockTraTac = false;
        unlockTraChanh = false;
        unlockTraSua = false;
        unlockMatcha = false;
        unlockCaPheDen = false;
        unlockCaPheSua = false;

        SaveGame();
        Debug.Log("⚠️ ĐÃ RESET GAME VỀ SỐ 0! TẤT CẢ CÔNG THỨC ĐÃ BỊ KHÓA!");
    }
}