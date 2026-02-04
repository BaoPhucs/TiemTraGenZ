using UnityEngine;

public class CartController : MonoBehaviour
{
    public enum CartState
    {
        DiChuyen,
        BanHang
    }

    public CartState currentState;

    public GameObject[] cacMonBanGhe;
    public GameObject phaChe;

    void Start()
    {
        // Mặc định vào game là Bày Hàng
        SetState(CartState.BanHang);
    }

    public void SetState(CartState newState)
    {
        currentState = newState;
        bool isBanHang = (newState == CartState.BanHang);

        // Bật/Tắt bàn ghế
        if (cacMonBanGhe != null)
        {
            foreach (GameObject mon in cacMonBanGhe)
            {
                if (mon != null) mon.SetActive(isBanHang);
            }
        }

        // Bật/Tắt nhóm pha chế (bao gồm cả ly trà đang pha dở)
        if (phaChe != null) phaChe.SetActive(isBanHang);
    }

    void Update()
    {
        // Bấm B để Bán Hàng (Bày ra)
        if (Input.GetKeyDown(KeyCode.B))
        {
            SetState(CartState.BanHang);
        }

        // --- SỬA LỖI TẠI ĐÂY ---
        // Đổi KeyCode.M thành KeyCode.C (hoặc phím khác tùy bạn)
        // Để tránh xung đột với phím M mở Shop
        if (Input.GetKeyDown(KeyCode.C))
        {
            SetState(CartState.DiChuyen);
        }
    }
}