using UnityEngine;

public class ObjectPlacer : MonoBehaviour
{
    public KeyCode placeKey = KeyCode.F;
    public GameObject prefabToPlace;
    public Transform placePoint;

    private void Update()
    {
        if (prefabToPlace == null || placePoint == null)
        {
            return;
        }

        if (Input.GetKeyDown(placeKey))
        {
            Instantiate(prefabToPlace, placePoint.position, placePoint.rotation);
        }
    }
}
