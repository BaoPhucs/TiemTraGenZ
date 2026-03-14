using UnityEngine;

public class DroneThanhPho : MonoBehaviour
{
    [Header("=== Cài Đặt Đường Bay ===")]
    public Transform[] diemBay; // Danh sách các điểm trên trời
    public float tocDoBay = 8f;
    public float tocDoXoay = 5f;

    private Transform mucTieuHienTai;

    void Start()
    {
        ChonMucTieuMoi();
    }

    void Update()
    {
        if (mucTieuHienTai == null || diemBay.Length == 0) return;

        // 1. Tịnh tiến về phía mục tiêu
        transform.position = Vector3.MoveTowards(transform.position, mucTieuHienTai.position, tocDoBay * Time.deltaTime);

        // 2. Xoay mặt về hướng bay (Đã chống lỗi đứng hình)
        Vector3 direction = (mucTieuHienTai.position - transform.position).normalized;
        direction.y = 0; // Cân bằng không cho chúi nhủi
        if (direction.sqrMagnitude > 0.001f)
        {
            Quaternion lookRotation = Quaternion.LookRotation(direction);
            transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, tocDoXoay * Time.deltaTime);
        }

        // 3. Nếu đã bay đến nơi -> Tìm điểm tiếp theo
        if (Vector3.Distance(transform.position, mucTieuHienTai.position) < 0.5f)
        {
            ChonMucTieuMoi();
        }
    }

    void ChonMucTieuMoi()
    {
        if (diemBay.Length <= 1) return;

        Transform mucTieuMoi = mucTieuHienTai;

        // Random điểm mới, đảm bảo không bốc trúng điểm đang đứng
        while (mucTieuMoi == mucTieuHienTai)
        {
            mucTieuMoi = diemBay[Random.Range(0, diemBay.Length)];
        }

        mucTieuHienTai = mucTieuMoi;
    }
}