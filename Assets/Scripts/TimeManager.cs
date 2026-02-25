using UnityEngine;
using TMPro; // Để hiển thị đồng hồ

public class TimeManager : MonoBehaviour
{
    [Header("--- CẤU HÌNH THỜI GIAN ---")]
    [Tooltip("Giờ bắt đầu game (ví dụ 6.0 là 6 giờ sáng)")]
    public float gioHienTai = 6.0f;

    [Tooltip("Tốc độ trôi (1.0 = 1 giây thực là 1 phút game)")]
    public float tocDoThoiGian = 1.0f;

    public bool daHetGio = false;

    [Header("--- THAM CHIẾU (KÉO VÀO) ---")]
    public TextMeshProUGUI dongHoHienThi; // Kéo UI Text vào đây
    public Light matTroi;                 // Kéo Directional Light vào đây (MỚI)

    // Biến để lưu giờ sẽ diễn ra cuộc gọi Ending ở Ngày Cuối
    private float gioGoiEnding = -1f;
    private bool daGoiEnding = false;

    void Update()
    {
        if (daHetGio) return;

        // 1. TÍNH TOÁN THỜI GIAN
        // Công thức: Tăng thời gian theo frame
        gioHienTai += Time.deltaTime * tocDoThoiGian / 60.0f;

        // Nếu qua 24h đêm thì reset về 0h sáng (Ngày mới)
        if (gioHienTai >= 24.0f)
        {
            gioHienTai = 0.0f;
            BatDauNgayMoi();
        }

        // 2. CẬP NHẬT ĐỒNG HỒ (UI)
        CapNhatGiaoDien();

        // 3. XOAY MẶT TRỜI (Hiệu ứng Ngày/Đêm)
        XoayMatTroi();

        // 4. KIỂM TRA SỰ KIỆN (10h tối)
        // Dùng khoảng nhỏ (22.0 đến 22.05) để tránh Log bị spam liên tục
        if (gioHienTai >= 22.0f && gioHienTai < 22.02f)
        {
            Debug.Log("Đã 10 giờ tối! Dọn hàng thôi.");
            // Có thể gọi GameManager.Instance.HienCanhBao("Sắp đóng cửa!") tại đây
        }

        // 5. KIỂM TRA SỰ KIỆN ENDING Ở NGÀY 90
        if (TiemTraGenZ.Manager.StoryManager.Instance != null && TiemTraGenZ.Manager.StoryManager.Instance.currentDay == TiemTraGenZ.Manager.StoryManager.Instance.maxDays)
        {
            if (!daGoiEnding && gioGoiEnding > 0f && gioHienTai >= gioGoiEnding)
            {
                daGoiEnding = true;
                var ending = TiemTraGenZ.Manager.StoryManager.Instance.CheckEnding();
                TiemTraGenZ.Manager.StoryManager.Instance.TriggerEnding(ending);
            }
        }
    }

    void CapNhatGiaoDien()
    {
        if (dongHoHienThi == null) return;

        int gio = Mathf.FloorToInt(gioHienTai);
        int phut = Mathf.FloorToInt((gioHienTai - gio) * 60);

        // -- TÍNH NĂNG MỚI: Đổi sang AM/PM cho đẹp --
        string buoi = (gio >= 12) ? "PM" : "AM";

        // Đổi từ 24h sang 12h (ví dụ 13h thành 1h)
        int gio12 = (gio > 12) ? gio - 12 : gio;
        if (gio12 == 0) gio12 = 12; // 0h thì hiện là 12h đêm

        // Hiển thị dạng: 08:30 PM
        dongHoHienThi.text = string.Format("{0:00}:{1:00} {2}", gio12, phut, buoi);
    }

    void XoayMatTroi()
    {
        if (matTroi == null) return;

        // -- LOGIC XOAY MẶT TRỜI --
        // 6h sáng = 0 độ (Mọc), 12h trưa = 90 độ (Đỉnh đầu), 18h tối = 180 độ (Lặn)
        // Công thức: (Giờ - 6) * 15 độ
        float gocQuay = (gioHienTai - 6.0f) * 15.0f;

        // Xoay trục X để mặt trời mọc/lặn. Trục Y để -30 cho bóng đổ nghiêng đẹp hơn
        matTroi.transform.rotation = Quaternion.Euler(gocQuay, -30f, 0f);

        // -- LOGIC TẮT/BẬT ĐÈN --
        // Nếu là đêm (sau 18h hoặc trước 6h sáng), giảm cường độ sáng xuống
        if (gioHienTai >= 18.0f || gioHienTai <= 5.0f)
        {
            // Tối dần đi (Mô phỏng chập choạng tối)
            matTroi.intensity = Mathf.MoveTowards(matTroi.intensity, 0.1f, Time.deltaTime);
        }
        else
        {
            // Sáng dần lên
            matTroi.intensity = Mathf.MoveTowards(matTroi.intensity, 1.0f, Time.deltaTime);
        }
    }

    void BatDauNgayMoi()
    {
        Debug.Log("--- NGÀY MỚI BẮT ĐẦU ---");
        // Chỗ này để reset doanh thu sau này
        
        // Gọi StoryManager để tăng currentDay lên 1 và tự động check các logic sự kiện Phone
        if (TiemTraGenZ.Manager.StoryManager.Instance != null)
        {
            TiemTraGenZ.Manager.StoryManager.Instance.AdvanceDay();

            // Nếu ngày mới nhảy sang đúng ngày 90 (ngày cuối), thì Random một giờ để gọi kết thúc
            if (TiemTraGenZ.Manager.StoryManager.Instance.currentDay == TiemTraGenZ.Manager.StoryManager.Instance.maxDays)
            {
                gioGoiEnding = Random.Range(10.0f, 17.0f); // Random từ 10h sáng tới 5h chiều
                Debug.Log($"[TimeManager] Đã setup cuộc gọi Ending vào lúc {gioGoiEnding:F1}h hôm nay.");
            }
        }
    }
}