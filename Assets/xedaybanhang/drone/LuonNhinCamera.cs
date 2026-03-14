using UnityEngine;

public class LuonNhinCamera : MonoBehaviour
{
    private Camera camMain;

    void Start()
    {
        camMain = Camera.main; // Tự động tìm Camera của người chơi
    }

    void LateUpdate()
    {
        if (camMain != null)
        {
            // Bắt cái UI luôn xoay mặt về phía Camera
            transform.LookAt(transform.position + camMain.transform.rotation * Vector3.forward,
                             camMain.transform.rotation * Vector3.up);
        }
    }
}