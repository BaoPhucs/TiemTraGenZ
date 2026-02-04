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
        if (prefabTextTien == null || viTriXuatHien == null) return;

        // Tạo chữ
        GameObject textObj = Instantiate(prefabTextTien, viTriXuatHien);

        // Đặt vị trí xuất hiện (Ví dụ: Giữa màn hình hoặc lệch sang phải xíu)
        // Bạn có thể chỉnh tọa độ này cho đẹp
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