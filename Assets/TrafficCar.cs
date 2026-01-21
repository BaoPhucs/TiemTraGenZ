using UnityEngine;
using System.Collections;

public class TrafficCar : MonoBehaviour
{
    [Header("--- Cấu Hình Đường Đi ---")]
    public Transform[] waypoints; // Giữ nguyên Array (không sửa thành List)
    public bool loop = true;      // Mặc định True để xe cũ chạy vòng quanh mãi mãi

    [Header("--- Cấu Hình Xe ---")]
    public float speed = 5.0f;
    public float rotationSpeed = 10.0f;
    public float reachDist = 1.0f;

    [Header("--- Cảm Biến Va Chạm ---")]
    public float rayDistance = 4.0f;
    public LayerMask obstacleLayer;   // Nhớ chọn Layer Traffic
    private bool isStopping = false;

    [Header("--- Âm Thanh Động Cơ ---")]
    public AudioSource audioSource;
    public float minPitch = 0.8f;     // Tiếng máy nổ êm (khi dừng)
    public float maxPitch = 1.5f;     // Tiếng máy gầm (khi chạy)

    private int currentPoint = 0;

    void Start()
    {
        // Tự động tìm AudioSource trên xe
        if (audioSource == null)
        {
            audioSource = GetComponent<AudioSource>();
        }

        // Nếu người dùng quên chưa gắn AudioSource, code tự tạo luôn cho đỡ lỗi
        if (audioSource == null)
        {
            audioSource = gameObject.AddComponent<AudioSource>();
            audioSource.spatialBlend = 1.0f; // Tự chuyển sang 3D
            audioSource.playOnAwake = true;
            audioSource.loop = true;
        }
    }

    void Update()
    {
        if (waypoints == null || waypoints.Length == 0) return;

        // 1. MẮT THẦN (RAYCAST)
        CheckObstacle();

        // 2. XỬ LÝ ÂM THANH (PITCH)
        HandleEngineSound();

        // Nếu đang dừng thì không di chuyển
        if (isStopping) return;

        // 3. DI CHUYỂN
        MoveCar();
    }

    void CheckObstacle()
    {
        Vector3 sensorStart = transform.position + transform.up * 0.5f;
        RaycastHit hit;

        // Vẽ tia đỏ để debug
        Debug.DrawRay(sensorStart, transform.forward * rayDistance, Color.red);

        if (Physics.Raycast(sensorStart, transform.forward, out hit, rayDistance, obstacleLayer))
        {
            isStopping = true;
        }
        else
        {
            isStopping = false;
        }
    }

    void HandleEngineSound()
    {
        if (audioSource == null) return;

        // Nếu xe dừng -> Pitch thấp. Xe chạy -> Pitch cao.
        float targetPitch = isStopping ? minPitch : maxPitch;
        audioSource.pitch = Mathf.Lerp(audioSource.pitch, targetPitch, Time.deltaTime * 2.0f);
    }

    void MoveCar()
    {
        Transform target = waypoints[currentPoint];
        if (target == null) return;

        Vector3 direction = target.position - transform.position;
        direction.y = 0;

        if (direction != Vector3.zero)
        {
            Quaternion lookRot = Quaternion.LookRotation(direction);
            transform.rotation = Quaternion.Slerp(transform.rotation, lookRot, Time.deltaTime * rotationSpeed);
        }

        transform.Translate(Vector3.forward * speed * Time.deltaTime);

        float dist = Vector3.Distance(transform.position, target.position);
        if (dist <= reachDist)
        {
            currentPoint++;
            if (currentPoint >= waypoints.Length)
            {
                if (loop) currentPoint = 0;
                else Destroy(gameObject);
            }
        }
    }
}