using UnityEngine;
using TMPro;

public class GarageDoor : MonoBehaviour
{
    [Header("Cấu hình")]
    public bool isClosed = true;
    public float openHeight = 3.0f;
    public float speed = 2.0f;
    public GameObject textHienThi;

    [Header("Cảm biến An toàn")]
    public Transform sensorPoint; // Điểm giữa chân cửa (để bắn tia xuống)
    public Vector3 sensorSize = new Vector3(3, 1, 1); // Kích thước vùng quét (Dài, Cao, Rộng)
    public LayerMask obstacleLayer; // Layer của Xe và Người (thường là Default, Player, Interact)

    private Vector3 viTriDong;
    private Vector3 viTriMo;
    private bool nguoiChoiOgan = false;

    void Start()
    {
        viTriDong = transform.localPosition;
        viTriMo = viTriDong + new Vector3(0, openHeight, 0);
        if (textHienThi != null) textHienThi.SetActive(false);
    }

    void Update()
    {
        // Di chuyển cửa
        Vector3 dichDen = isClosed ? viTriDong : viTriMo;
        transform.localPosition = Vector3.Lerp(transform.localPosition, dichDen, Time.deltaTime * speed);

        // Bấm E để Đóng/Mở
        if (nguoiChoiOgan && Input.GetKeyDown(KeyCode.E))
        {
            ToggleDoor();
        }
    }

    public void ToggleDoor()
    {
        if (isClosed)
        {
            // Đang đóng -> Mở thì thoải mái
            isClosed = false;
        }
        else
        {
            // Đang mở -> Muốn đóng -> Phải quét xem có vướng gì không
            if (CheckObstacle())
            {
                Debug.Log("⛔ Có vật cản! Không đóng được!");
                // (Tùy chọn) Hiện thông báo UI ở đây
            }
            else
            {
                isClosed = true;
            }
        }
    }

    // Hàm quét vật cản
    bool CheckObstacle()
    {
        if (sensorPoint == null) return false;

        // Bắn một cái hộp vô hình ngay tại vị trí cửa để xem có va trúng ai không
        Collider[] hits = Physics.OverlapBox(sensorPoint.position, sensorSize / 2, sensorPoint.rotation, obstacleLayer);

        foreach (var hit in hits)
        {
            // Nếu va trúng bất cứ cái gì KHÔNG PHẢI LÀ SÀN NHÀ hay CỬA
            if (!hit.CompareTag("Ground") && hit.gameObject != this.gameObject)
            {
                Debug.Log("Vướng: " + hit.name);
                return true; // Có vật cản
            }
        }
        return false; // An toàn
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            nguoiChoiOgan = true;
            if (textHienThi != null) textHienThi.SetActive(true);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            nguoiChoiOgan = false;
            if (textHienThi != null) textHienThi.SetActive(false);
        }
    }

    // Vẽ vùng quét để dễ chỉnh trong Scene
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