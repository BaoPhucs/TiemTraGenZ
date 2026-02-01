using UnityEngine;

public class SpawnOnInteract : MonoBehaviour
{
    public GameObject prefabToSpawn;
    public Transform spawnPoint;

    public void Spawn()
    {
        if (prefabToSpawn == null) return;

        Vector3 pos = spawnPoint != null
            ? spawnPoint.position
            : transform.position + transform.forward * 1.5f;

        Instantiate(prefabToSpawn, pos, Quaternion.identity);
    }
}
