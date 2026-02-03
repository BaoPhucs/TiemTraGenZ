using UnityEngine;

public class QuanLyKho : MonoBehaviour
{
    public static QuanLyKho Instance; // Để gọi từ bất cứ đâu

    [Header("Số lượng đồ đang ở bên ngoài")]
    public int soDoBenNgoai = 0;

    [Header("Giới hạn số lượng (Trong kho có bao nhiêu cái)")]
    public int maxGhe = 6;
    public int maxBan = 2;
    public int maxThungDa = 1;

    [Header("Đếm số lượng đã lấy ra")]
    public int gheDaLay = 0;
    public int banDaLay = 0;
    public int thungDaDaLay = 0;

    public PushableCart boDayXe; // Kéo script đẩy xe vào đây

    void Awake()
    {
        Instance = this;
    }

    void Update()
    {
        // Kiểm tra để Khóa/Mở chức năng đẩy xe
        if (boDayXe != null)
        {
            // Nếu đồ bên ngoài > 0 thì KHÓA (không cho đẩy)
            // Ngược lại thì MỞ (cho đẩy)
            boDayXe.enabled = (soDoBenNgoai == 0);
        }
    }

    // Hàm gọi khi Lấy đồ ra
    public bool LayDoRa(string loaiDo)
    {
        if (loaiDo == "Ghe" && gheDaLay < maxGhe)
        {
            gheDaLay++;
            soDoBenNgoai++;
            return true; // Cho phép lấy
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

        Debug.Log("Hết hàng trong kho rồi!");
        return false; // Hết hàng
    }

    // Hàm gọi khi Thu hồi đồ về
    public void ThuDoVe(string loaiDo)
    {
        soDoBenNgoai--;
        if (soDoBenNgoai < 0) soDoBenNgoai = 0;

        if (loaiDo == "Ghe") gheDaLay--;
        if (loaiDo == "Ban") banDaLay--;
        if (loaiDo == "ThungDa") thungDaDaLay--;
    }

    public bool ConDoBenNgoai()
    {
        // Kiểm tra tất cả các biến đếm số lượng "đã lấy ra"
        // (Lưu ý: Hãy sửa tên biến bên dưới cho khớp CHÍNH XÁC với code của bạn nếu tôi viết sai)

        if (banDaLay > 0) return true;      // Còn bàn ở ngoài
        if (gheDaLay > 0) return true;      // Còn ghế (nếu có biến này)
        if (thungDaDaLay > 0) return true;  // Còn thùng đá

        // Nếu tất cả bằng 0 hết -> Trả về false (Đã dọn sạch)
        return false;
    }
}