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

    [Header("--- THAM CHIẾU (TỰ ĐỘNG TÌM NẾU TRỐNG) ---")]
    public TextMeshProUGUI dongHoHienThi;
    public Light matTroi;

    private float gioGoiEnding = -1f;
    private bool daGoiEnding = false;

    void Update()
    {
        if (daHetGio) return;

        // =========================================================
        // 0. BỘ ĐỊNH VỊ TỰ ĐỘNG: TÌM LẠI ĐỒNG HỒ & MẶT TRỜI KHI RESTART
        // =========================================================
        if (dongHoHienThi == null)
        {
            // Tự động tìm object tên là "ClockText" trên màn hình
            GameObject txtObj = GameObject.Find("ClockText");
            if (txtObj != null)
            {
                dongHoHienThi = txtObj.GetComponent<TextMeshProUGUI>();
            }
        }

        if (matTroi == null)
        {
            // Tự động tìm ánh sáng mặt trời
            GameObject sunObj = GameObject.Find("Directional Light");
            if (sunObj != null)
            {
                matTroi = sunObj.GetComponent<Light>();
            }
        }
        // =========================================================

        // 1. TÍNH TOÁN THỜI GIAN
        gioHienTai += Time.deltaTime * tocDoThoiGian / 60.0f;

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
        if (gioHienTai >= 22.0f && gioHienTai < 22.02f)
        {
            Debug.Log("Đã 10 giờ tối! Dọn hàng thôi.");
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

        string buoi = (gio >= 12) ? "PM" : "AM";
        int gio12 = (gio > 12) ? gio - 12 : gio;
        if (gio12 == 0) gio12 = 12;

        dongHoHienThi.text = string.Format("{0:00}:{1:00} {2}", gio12, phut, buoi);
    }

    void XoayMatTroi()
    {
        if (matTroi == null) return;

        float gocQuay = (gioHienTai - 6.0f) * 15.0f;
        matTroi.transform.rotation = Quaternion.Euler(gocQuay, -30f, 0f);

        if (gioHienTai >= 18.0f || gioHienTai <= 5.0f)
        {
            matTroi.intensity = Mathf.MoveTowards(matTroi.intensity, 0.1f, Time.deltaTime);
        }
        else
        {
            matTroi.intensity = Mathf.MoveTowards(matTroi.intensity, 1.0f, Time.deltaTime);
        }
    }

    void BatDauNgayMoi()
    {
        Debug.Log("--- NGÀY MỚI BẮT ĐẦU ---");

        if (TiemTraGenZ.Manager.StoryManager.Instance != null)
        {
            TiemTraGenZ.Manager.StoryManager.Instance.AdvanceDay();

            if (TiemTraGenZ.Manager.StoryManager.Instance.currentDay == TiemTraGenZ.Manager.StoryManager.Instance.maxDays)
            {
                gioGoiEnding = Random.Range(10.0f, 17.0f);
                Debug.Log($"[TimeManager] Đã setup cuộc gọi Ending vào lúc {gioGoiEnding:F1}h hôm nay.");
            }
        }
    }
}