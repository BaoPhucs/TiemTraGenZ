using UnityEngine;

public class CuaHangVatLy : MonoBehaviour
{
    public enum LoaiCuaHang { CuaHangNguyenLieu, ThayDayPhaChe }

    [Header("Chức năng của cửa hàng này:")]
    public LoaiCuaHang loaiCuaHangCuaToi;

    [Header("Hiển thị chữ: Bấm E để mua hàng")]
    public GameObject huongDanUI;

    private bool isPlayerNear = false;

    void Start()
    {
        if (huongDanUI != null) huongDanUI.SetActive(false);
    }

    void Update()
    {
        // Khi người chơi lại gần và bấm phím E
        if (isPlayerNear && Input.GetKeyDown(KeyCode.E))
        {
            Debug.Log("🟢 Đã nhận lệnh bấm phím E từ người chơi!");

            if (ShopManager.Instance != null)
            {
                Debug.Log("🟢 Đã tìm thấy ShopManager! Chuẩn bị mở: " + loaiCuaHangCuaToi);

                if (loaiCuaHangCuaToi == LoaiCuaHang.CuaHangNguyenLieu)
                {
                    ShopManager.Instance.MoShopNguyenLieu();
                }
                else if (loaiCuaHangCuaToi == LoaiCuaHang.ThayDayPhaChe)
                {
                    ShopManager.Instance.MoShopCongThuc();
                }
            }
            else
            {
                Debug.LogError("🔴 LỖI NẶNG: ShopManager.Instance đang bị NULL! Ông NPC không tìm thấy ShopManager đâu cả!");
            }
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            isPlayerNear = true;
            if (huongDanUI != null) huongDanUI.SetActive(true);
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            isPlayerNear = false;
            if (huongDanUI != null) huongDanUI.SetActive(false);

            // Tự động đóng Shop nếu người chơi chạy ra xa
            if (ShopManager.Instance != null) ShopManager.Instance.DongTatCaShop();
        }
    }
}