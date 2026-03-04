using UnityEngine;

public class DonRac : MonoBehaviour
{
    // Biến này sẽ lưu trực tiếp cái ghế mà rác đang nằm lên
    public SeatPoint gheDangNgoi;

    private void OnMouseDown()
    {
        // 1. Mở khóa đúng cái ghế đó (không cần quét tìm nữa)
        if (gheDangNgoi != null)
        {
            gheDangNgoi.isOccupied = false;
            Debug.Log("✨ Đã dọn rác! Ghế đã trống cho khách mới.");
        }
        else
        {
            Debug.LogWarning("Không tìm thấy dữ liệu ghế để mở khóa!");
        }

        // 2. Tiêu hủy rác
        Destroy(gameObject);
    }
}