using UnityEngine;

public class DonRac : MonoBehaviour
{
    // Biến này sẽ lưu trực tiếp cái ghế mà rác đang nằm lên
    public SeatPoint gheDangNgoi;

    private bool isPlayerNear = false;

    // --- CÁCH 1: Dùng chuột click thẳng vào rác ---
    private void OnMouseDown()
    {
        ThucHienDonRac();
    }

    // --- CÁCH 2: Nhân vật đi lại gần và bấm phím E ---
    void Update()
    {
        if (isPlayerNear && Input.GetKeyDown(KeyCode.E))
        {
            ThucHienDonRac();
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player")) isPlayerNear = true;
    }

    void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player")) isPlayerNear = false;
    }

    // =======================================================
    // HÀM XỬ LÝ CHUNG: TRẢ GHẾ VÀ CỘNG ĐIỂM
    // =======================================================
    private void ThucHienDonRac()
    {
        // 1. Mở khóa đúng cái ghế đó để đón khách mới (Logic cực chuẩn của bạn)
        if (gheDangNgoi != null)
        {
            gheDangNgoi.isOccupied = false;
        }
        else
        {
            Debug.LogWarning("Không tìm thấy dữ liệu ghế để mở khóa!");
        }

        // 2. Tăng điểm Tình Làng Nghĩa Xóm
        if (QuanLyKho.Instance != null)
        {
            // Tối đa 100 điểm, dọn rác được cộng 2 điểm
            if (QuanLyKho.Instance.DiemTinhLang < 100)
            {
                QuanLyKho.Instance.DiemTinhLang += 2;
            }
            QuanLyKho.Instance.SaveGame(); // Lưu lại vào ổ cứng ngay lập tức
        }

        Debug.Log("✨ Đã dọn rác! Ghế trống sẵn sàng và Tình Làng +2.");

        // 3. Tiêu hủy rác
        Destroy(gameObject);
    }
}