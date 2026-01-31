using UnityEngine;

public class ThirdPersonCamera : MonoBehaviour
{
    public Transform target;
    public Vector3 offset = new Vector3(0f, 2f, -4f);
    public float followSpeed = 8f;
    public float mouseSensitivity = 3f;
    public float minPitch = 10f;
    public float maxPitch = 60f;
    public float lookAtHeight = 1.4f;
    public bool enableMouseLook = true;
    public bool lockCursorOnStart = true;
    public bool clickToRelock = true;
    public KeyCode unlockKey = KeyCode.Escape;

    private float yaw = 0f;
    private float pitch = 20f;
    private bool cursorLocked = false;

    private void Start()
    {
        if (enableMouseLook && lockCursorOnStart)
        {
            SetCursorLock(true);
        }
    }

    private void LateUpdate()
    {
        if (target == null)
        {
            return;
        }

        HandleCursorLock();

        if (enableMouseLook && cursorLocked)
        {
            yaw += Input.GetAxis("Mouse X") * mouseSensitivity;
            pitch -= Input.GetAxis("Mouse Y") * mouseSensitivity;
            pitch = Mathf.Clamp(pitch, minPitch, maxPitch);
        }

        Quaternion rot = Quaternion.Euler(pitch, yaw, 0f);
        Vector3 desiredPos = target.position + rot * offset;
        transform.position = Vector3.Lerp(transform.position, desiredPos, followSpeed * Time.deltaTime);

        Vector3 lookPos = target.position + Vector3.up * lookAtHeight;
        transform.rotation = Quaternion.LookRotation(lookPos - transform.position, Vector3.up);
    }

    private void HandleCursorLock()
    {
        if (!enableMouseLook)
        {
            if (cursorLocked)
            {
                SetCursorLock(false);
            }
            return;
        }

        if (Input.GetKeyDown(unlockKey))
        {
            SetCursorLock(false);
        }
        else if (clickToRelock && !cursorLocked && Input.GetMouseButtonDown(0))
        {
            SetCursorLock(true);
        }
    }

    private void SetCursorLock(bool locked)
    {
        cursorLocked = locked;
        Cursor.lockState = locked ? CursorLockMode.Locked : CursorLockMode.None;
        Cursor.visible = !locked;
    }

    private void OnDisable()
    {
        if (cursorLocked)
        {
            SetCursorLock(false);
        }
    }
}
