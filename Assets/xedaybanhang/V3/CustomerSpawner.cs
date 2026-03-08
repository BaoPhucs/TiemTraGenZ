using UnityEngine;
using UnityEngine.AI; // Cần thiết để dùng NavMesh

public class CustomerSpawner : MonoBehaviour
{
    [Header("Cài đặt Khách hàng")]
    public GameObject customerPrefab;
    public float thoiGianSinhKhach = 30f;

    [Header("Các điểm xuất hiện (Đầu hẻm)")]
    public Transform[] spawnPoints;

    void Start()
    {
        InvokeRepeating("SpawnCustomer", 3f, thoiGianSinhKhach);
    }

    void SpawnCustomer()
    {
        if (spawnPoints.Length == 0 || customerPrefab == null) return;

        Transform diemSpawn = spawnPoints[Random.Range(0, spawnPoints.Length)];

        // ========================================================
        // SỬA LỖI ĐI BỘ TẠI CHỖ LÚC MỚI SINH RA
        // ========================================================
        // Quét bán kính 5m để tự động bắt dính khách vào mặt đường NavMesh
        if (NavMesh.SamplePosition(diemSpawn.position, out NavMeshHit hit, 5f, NavMesh.AllAreas))
        {
            Instantiate(customerPrefab, hit.position, diemSpawn.rotation);
        }
        else
        {
            // Nếu không tìm thấy, cứ sinh đại ra vị trí cũ
            Instantiate(customerPrefab, diemSpawn.position, diemSpawn.rotation);
        }

        Debug.Log("🚶 Đã sinh ra 1 khách hàng mới ở: " + diemSpawn.name);
    }
}