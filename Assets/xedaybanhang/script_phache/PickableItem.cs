using UnityEngine;

public class PickableItem : MonoBehaviour
{
    public GameObject prefabToSpawn;

    void OnMouseDown()
    {
        if (prefabToSpawn == null) return;

        Vector3 spawnPos = transform.position + transform.forward * 1.5f;
        Instantiate(prefabToSpawn, spawnPos, Quaternion.identity);
    }
}
