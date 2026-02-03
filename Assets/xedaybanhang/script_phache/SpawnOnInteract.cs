using UnityEngine;

// Thêm cái IInteractable vào dòng này
public class SpawnOnInteract : MonoBehaviour, IInteractable
{
    public GameObject prefabToSpawn;
    public Transform spawnPoint; // Điểm sinh ra ghế (đặt cạnh xe)
    public string tenHanhDong = "Lấy Ghế";

    // Hàm bắt buộc của Interface
    public void Interact()
    {
        Spawn();
    }

    // Hàm hiển thị tên hành động
    public string GetActionName()
    {
        return tenHanhDong;
    }

    public void Spawn()
    {
        if (prefabToSpawn == null) return;

        // Sinh ra tại vị trí spawnPoint
        Vector3 pos = spawnPoint != null ? spawnPoint.position : transform.position + transform.forward;
        Instantiate(prefabToSpawn, pos, Quaternion.identity);

        Debug.Log("Đã lấy một cái ghế!");
    }
}