using UnityEngine;

public class GarageDoor : MonoBehaviour
{
    [Header("Cấu hình")]
    public bool isClosed = true;
    public float openHeight = 3.0f;
    public float speed = 2.0f;

    [Header("Cảm biến An toàn")]
    public Transform sensorPoint;
    public Vector3 sensorSize = new Vector3(3, 1, 1);
    public LayerMask obstacleLayer;

    private Vector3 viTriDong;
    private Vector3 viTriMo;

    void Start()
    {
        viTriDong = transform.localPosition;
        viTriMo = viTriDong + new Vector3(0, openHeight, 0);
    }

    void Update()
    {
        // Chỉ làm đúng 1 nhiệm vụ: Di chuyển cửa
        Vector3 dichDen = isClosed ? viTriDong : viTriMo;
        transform.localPosition = Vector3.Lerp(transform.localPosition, dichDen, Time.deltaTime * speed);
    }

    // Hàm này sẽ được "Công Tắc" gọi
    public void ToggleDoor()
    {
        if (isClosed)
        {
            isClosed = false; // Mở ra
        }
        else
        {
            if (CheckObstacle())
            {
                Debug.Log("⛔ Có vật cản! Không đóng được!");
            }
            else
            {
                isClosed = true; // Đóng lại
            }
        }
    }

    bool CheckObstacle()
    {
        if (sensorPoint == null) return false;
        Collider[] hits = Physics.OverlapBox(sensorPoint.position, sensorSize / 2, sensorPoint.rotation, obstacleLayer);
        foreach (var hit in hits)
        {
            if (!hit.CompareTag("Ground") && hit.gameObject != this.gameObject)
            {
                return true;
            }
        }
        return false;
    }

    private void OnDrawGizmos()
    {
        if (sensorPoint != null)
        {
            Gizmos.color = Color.red;
            Gizmos.matrix = Matrix4x4.TRS(sensorPoint.position, sensorPoint.rotation, Vector3.one);
            Gizmos.DrawWireCube(Vector3.zero, sensorSize);
        }
    }
}