using UnityEngine;
using UnityEngine.AI;

public class CustomerSpawner : MonoBehaviour
{
    [Header("Cài đặt Khách hàng")]
    public GameObject[] customerPrefabs;
    public float thoiGianSinhKhach = 30f;

    [Header("Các điểm xuất hiện")]
    public Transform[] spawnPoints;

    // --- BIẾN GHI NHỚ KHÁCH VỪA SPAWN ---
    private int lastSpawnIndex = -1;

    void Start()
    {
        InvokeRepeating("SpawnCustomer", 3f, thoiGianSinhKhach);
    }

    void SpawnCustomer()
    {
        if (spawnPoints.Length == 0 || customerPrefabs.Length == 0) return;

        // 1. Random vị trí xuất hiện
        Transform diemSpawn = spawnPoints[Random.Range(0, spawnPoints.Length)];

        // ========================================================
        // 2. LOGIC CHỐNG TRÙNG LẶP KHÁCH HÀNG
        // ========================================================
        int randomIndex = Random.Range(0, customerPrefabs.Length);

        // Nếu có nhiều hơn 1 loại khách, thì mới dùng vòng lặp chống trùng
        if (customerPrefabs.Length > 1)
        {
            // Chừng nào còn bốc trúng người cũ, thì bốc lại!
            while (randomIndex == lastSpawnIndex)
            {
                randomIndex = Random.Range(0, customerPrefabs.Length);
            }
        }

        // Lưu lại người vừa bốc để lần sau không bốc trúng nữa
        lastSpawnIndex = randomIndex;

        // Chốt đơn khách hàng
        GameObject khachDuocChon = customerPrefabs[randomIndex];

        // 3. Spawn khách ra map
        if (NavMesh.SamplePosition(diemSpawn.position, out NavMeshHit hit, 5f, NavMesh.AllAreas))
        {
            Instantiate(khachDuocChon, hit.position, diemSpawn.rotation);
        }
        else
        {
            Instantiate(khachDuocChon, diemSpawn.position, diemSpawn.rotation);
        }

        Debug.Log("🚶 Đã sinh ra khách: [" + khachDuocChon.name + "]. Đảm bảo không trùng khách trước!");
    }
}