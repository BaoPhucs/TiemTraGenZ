using UnityEngine;

public class UINgayMoiConnector : MonoBehaviour
{
    [Header("Kéo BangKetToan_Panel vào đây:")]
    public GameObject panelCanAn; // Tự cái nút sẽ giữ quyền tắt cái bảng

    public void ClickGoiNgayMoi()
    {
        // 1. Vẫn gọi GameManager để rã đông game, giấu chuột, qua ngày
        GameLoopManager loopManager = FindObjectOfType<GameLoopManager>();

        if (loopManager != null)
        {
            loopManager.SangNgayMoi();
        }

        // 2. TỰ TAY TẮT BẢNG! (Bỏ qua GameManager)
        if (panelCanAn != null)
        {
            panelCanAn.SetActive(false);
        }
    }
}