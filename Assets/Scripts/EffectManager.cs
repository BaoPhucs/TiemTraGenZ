using UnityEngine;

public class EffectManager : MonoBehaviour
{
    public static EffectManager Instance;
    public GameObject prefabTextTien; // Kéo Prefab vừa tạo vào đây
    public Transform viTriXuatHien;   // Kéo HUD_Canvas vào đây

    void Awake()
    {
        Instance = this;
    }

    public void HienThiTien(int soTien)
    {
        // =========================================================
        // BỘ ĐỊNH VỊ: Tự động tìm lại HUD_Canvas nếu bị đứt kết nối (do Restart)
        // =========================================================
        if (viTriXuatHien == null)
        {
            // Dựa theo các ảnh trước của bạn, Canvas giao diện tên chính xác là "HUD_Canvas"
            GameObject canvasObj = GameObject.Find("HUD_Canvas");
            if (canvasObj != null)
            {
                viTriXuatHien = canvasObj.transform;
            }
        }

        // Kiểm tra chốt chặn: Nếu vẫn không tìm thấy thì chặn lỗi luôn
        if (prefabTextTien == null || viTriXuatHien == null) return;

        // Tạo chữ
        GameObject textObj = Instantiate(prefabTextTien, viTriXuatHien);

        // Đặt vị trí xuất hiện (Ví dụ: Giữa màn hình hoặc lệch sang phải xíu)
        textObj.transform.localPosition = new Vector3(300, 200, 0);

        FloatingText script = textObj.GetComponent<FloatingText>();
        if (script != null)
        {
            bool laLoi = soTien >= 0;
            string dau = laLoi ? "+" : "";
            script.KhoiTao(dau + soTien.ToString("n0") + "đ", laLoi);
        }
    }
}