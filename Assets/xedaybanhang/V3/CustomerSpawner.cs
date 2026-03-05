using UnityEngine;

public class CustomerSpawner : MonoBehaviour
{
    [Header("Cài đặt Khách hàng")]
    public GameObject customerPrefab; // Kéo Prefab viên nhộng vào đây
    public float thoiGianSinhKhach = 30f; // Cứ 10 giây ra 1 khách

    [Header("Các điểm xuất hiện (Đầu hẻm)")]
    public Transform[] spawnPoints;

    void Start()
    {
        // Gọi hàm SpawnCustomer lặp đi lặp lại
        InvokeRepeating("SpawnCustomer", 3f, thoiGianSinhKhach);
    }

    void SpawnCustomer()
    {
        if (spawnPoints.Length == 0 || customerPrefab == null) return;

        // Bốc ngẫu nhiên 1 trong các điểm Spawn (Ví dụ có 2 điểm ở 2 đầu hẻm)
        Transform diemSpawn = spawnPoints[Random.Range(0, spawnPoints.Length)];

        // Sinh ra khách
        Instantiate(customerPrefab, diemSpawn.position, diemSpawn.rotation);

        Debug.Log("🚶 Đã sinh ra 1 khách hàng mới ở: " + diemSpawn.name);
    }
}