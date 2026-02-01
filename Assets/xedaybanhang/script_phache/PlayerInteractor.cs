using UnityEngine;

public class PlayerInteractor : MonoBehaviour
{
    public float interactDistance = 2f;
    public LayerMask interactLayer;
    public Camera fpsCamera;

    public Interactable currentTarget;

    void Update()
    {
        DetectTarget();

        if (currentTarget != null && Input.GetKeyDown(KeyCode.E))
        {
            currentTarget.Interact();
        }
    }

    void DetectTarget()
    {
        currentTarget = null;

        Ray ray = new Ray(fpsCamera.transform.position, fpsCamera.transform.forward);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, interactDistance, interactLayer))
        {
            Interactable interactable = hit.collider.GetComponent<Interactable>();
            if (interactable != null)
            {
                currentTarget = interactable;
            }
        }
    }
}
