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
        // SỬA LỖI XUNG ĐỘT PHÍM TẠI ĐÂY

        // Đổi phím B thành phím O (Open - Mở quán) để không trùng với nút Nghe điện thoại
        if (Input.GetKeyDown(KeyCode.O))
        {
            SetState(CartState.BanHang);
        }

        // Đổi phím C thành phím P (Pack - Dọn hàng) để không trùng với nút Cúp điện thoại
        if (Input.GetKeyDown(KeyCode.P))
        {
            SetState(CartState.DiChuyen);
        }
    }
}