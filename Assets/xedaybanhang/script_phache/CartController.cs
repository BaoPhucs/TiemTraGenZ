using UnityEngine;

public class CartController : MonoBehaviour
{
    public enum CartState
    {
        DiChuyen,
        BanHang
    }

    public CartState currentState;

    // SỬA DÒNG NÀY: Thay vì 1 GameObject, ta dùng Mảng [] để chứa nhiều cái
    public GameObject[] cacMonBanGhe;

    // Giữ nguyên cái này hoặc đổi thành mảng luôn nếu muốn xé lẻ nhóm Pha Chế
    public GameObject phaChe;

    void Start()
    {
        // Mặc định vào game là Bày Hàng để test
        SetState(CartState.BanHang);
    }

    public void SetState(CartState newState)
    {
        currentState = newState;
        bool isBanHang = (newState == CartState.BanHang);

        // VÒNG LẶP: Tắt/Bật từng món trong danh sách
        if (cacMonBanGhe != null)
        {
            foreach (GameObject mon in cacMonBanGhe)
            {
                if (mon != null) mon.SetActive(isBanHang);
            }
        }

        if (phaChe != null) phaChe.SetActive(isBanHang);
    }

    // Giữ nguyên phần Update bên dưới...
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.B)) SetState(CartState.BanHang);
        if (Input.GetKeyDown(KeyCode.M)) SetState(CartState.DiChuyen);
    }


}