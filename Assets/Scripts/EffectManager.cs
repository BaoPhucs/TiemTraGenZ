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
        Debug.Log($"<color=yellow>[EffectManager] Đã nhận lệnh văng chữ: {soTien}</color>");

        if (viTriXuatHien == null)
        {
            Debug.Log("[EffectManager] viTriXuatHien bị mất, tự động quét tìm lại HUD_Canvas...");
            GameObject canvasObj = GameObject.Find("HUD_Canvas");
            if (canvasObj != null)
            {
                viTriXuatHien = canvasObj.transform;
                Debug.Log("[EffectManager] TÌM THẤY HUD_Canvas thành công!");
            }
            else Debug.LogError("[EffectManager] LỖI CHÍ MẠNG: Không tìm thấy HUD_Canvas trên màn hình!");
        }

        if (prefabTextTien == null)
        {
            Debug.LogError("[EffectManager] LỖI: Chưa kéo PrefabTextTien vào Inspector!");
            return;
        }

        if (viTriXuatHien == null) return;

        GameObject textObj = Instantiate(prefabTextTien, viTriXuatHien);

        // Đưa tọa độ về GIỮA MÀN HÌNH (nhích lên trên một chút) để 100% lọt vào tầm mắt
        textObj.transform.localPosition = new Vector3(0, 150, 0);
        Debug.Log("[EffectManager] Đã đẻ ra Prefab chữ ở tọa độ (0, 150)!");

        FloatingText script = textObj.GetComponent<FloatingText>();
        if (script != null)
        {
            bool laLoi = soTien >= 0;
            string dau = laLoi ? "+" : "";
            script.KhoiTao(dau + soTien.ToString("n0") + "đ", laLoi);
            Debug.Log($"<color=green>[EffectManager] KHỞI TẠO CHỮ HOÀN TẤT: {dau}{soTien:n0}đ</color>");
        }
        else Debug.LogError("[EffectManager] LỖI: PrefabTextTien không chứa script FloatingText!");
    }
}