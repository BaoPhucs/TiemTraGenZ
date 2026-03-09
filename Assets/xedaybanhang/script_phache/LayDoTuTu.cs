using UnityEngine;

public class LayDoTuTu : MonoBehaviour, IInteractable
{
    public enum LoaiDo { Ghe, Ban, ThungDa }
    public LoaiDo loaiVatPham;

    public GameObject prefabDoVat;   // Prefab đồ thật (có Collider, có Script)
    public Transform viTriCoDinh;    // Chỉ dùng cho Thùng Đá

    // --- BIẾN CHO CHẾ ĐỘ ĐẶT ĐỒ ---
    private GameObject previewObject; // Cái "bóng ma" để ngắm vị trí
    private bool dangCamDo = false;   // Đang cầm đồ trên tay hay không?
    private GameObject thungDaDangHienHuu; // Biến riêng để quản lý thùng đá cố định

    void Update()
    {
        // Logic này chỉ chạy khi bạn ĐANG CẦM BÀN/GHẾ (đang ngắm nghía)
        if (dangCamDo && previewObject != null)
        {
            XuLyDiChuyenBongMa();
            XuLyDatXuong();
        }
    }

    // 1. Hàm di chuyển cái bóng theo chấm đỏ
    void XuLyDiChuyenBongMa()
    {
        // Bắn tia từ GIỮA MÀN HÌNH (Chấm đỏ)
        Ray ray = Camera.main.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0));
        RaycastHit hit;

        // LayerMask: Chỉ bắn vào những vật "Default" hoặc "Ground" (tránh bắn vào Player hay Trigger)
        // Dấu ~ nghĩa là loại trừ layer Interact và Player ra
        int layerMask = ~LayerMask.GetMask("Player", "Interact", "Ignore Raycast");

        if (Physics.Raycast(ray, out hit, 10f, layerMask)) // Tầm xa 10 mét
        {
            // Đặt cái bóng tại điểm tia chạm đất
            previewObject.transform.position = hit.point;

            // Xoay cái bóng cho nó luôn quay mặt về phía người chơi (để dễ nhìn)
            Vector3 huongQuay = transform.position - Camera.main.transform.position;
            huongQuay.y = 0; // Giữ thăng bằng, không chúi đầu xuống đất
            if (huongQuay != Vector3.zero)
                previewObject.transform.rotation = Quaternion.LookRotation(huongQuay);
        }
    }

    // 2. Hàm bắt sự kiện Click chuột để đặt
    void XuLyDatXuong()
    {
        // Bấm CHUỘT TRÁI (0) để Đặt
        if (Input.GetMouseButtonDown(0))
        {
            // Sinh ra đồ thật ngay tại vị trí cái bóng
            GameObject doThat = Instantiate(prefabDoVat, previewObject.transform.position, previewObject.transform.rotation);

            // Xóa cái bóng đi
            Destroy(previewObject);
            dangCamDo = false;
            previewObject = null;
        }

        // Bấm CHUỘT PHẢI (1) để HỦY (Trả về kho)
        if (Input.GetMouseButtonDown(1))
        {
            QuanLyKho.Instance.ThuDoVe(loaiVatPham.ToString());
            Destroy(previewObject);
            dangCamDo = false;
            previewObject = null;
        }
    }

    // 3. Hàm Interact (Khi bấm E vào tủ xe)
    public void Interact()
    {
        // A. NẾU LÀ THÙNG ĐÁ (Logic cũ: Bật/Tắt tại chỗ cố định)
        if (loaiVatPham == LoaiDo.ThungDa)
        {
            XuLyThungDaCoDinh();
            return;
        }

        // B. NẾU LÀ BÀN/GHẾ (Logic mới: Bắt đầu chế độ ngắm nghía)
        if (dangCamDo) return; // Đang cầm rồi thì không lấy thêm

        // Kiểm tra kho
        if (!QuanLyKho.Instance.LayDoRa(loaiVatPham.ToString())) return;

        // Tạo ra cái bóng ma (Preview)
        previewObject = Instantiate(prefabDoVat);

        // --- QUAN TRỌNG: LỘT BỎ HẾT CHỨC NĂNG CỦA CÁI BÓNG ---
        // Xóa Collider (để tia Raycast không bắn trúng chính nó -> gây giật lag)
        Destroy(previewObject.GetComponent<BoxCollider>());
        // Xóa Script Thu Hồi (để không hiện chữ "Cất Bàn" khi đang cầm)
        Destroy(previewObject.GetComponent<ThuHoiDo>());

        // (Mẹo: Nếu muốn làm màu, bạn có thể chỉnh Material của previewObject thành trong suốt ở đây)

        dangCamDo = true; // Bật cờ "Đang cầm đồ"
    }

    // Logic riêng cho Thùng Đá (Giữ nguyên như cũ)
    void XuLyThungDaCoDinh()
    {
        if (thungDaDangHienHuu != null)
        {
            Destroy(thungDaDangHienHuu);
            QuanLyKho.Instance.ThuDoVe("ThungDa");
        }
        else
        {
            if (QuanLyKho.Instance.LayDoRa("ThungDa"))
            {
                thungDaDangHienHuu = Instantiate(prefabDoVat, viTriCoDinh.position, viTriCoDinh.rotation);
            }
        }
    }

    public string GetActionName()
    {
        if (loaiVatPham == LoaiDo.ThungDa && thungDaDangHienHuu != null) return "E - Cất Thùng Đá";
        if (dangCamDo) return "Đang cầm... (Chuột Trái: Đặt)";
        return "E - Lấy " + loaiVatPham.ToString();
    }
}