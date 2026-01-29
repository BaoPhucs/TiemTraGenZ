using UnityEngine;
using UnityEngine.AI; // Bắt buộc để dùng NavMesh

[RequireComponent(typeof(NavMeshAgent))]
public class NPC_DiBo_Pro : MonoBehaviour
{
    [Header("--- Cấu Hình Đường Đi ---")]
    public Transform[] waypoints; // Danh sách điểm P1 -> P7
    public bool loop = true;      // Đi vòng tròn vô tận

    [Header("--- Cấu Hình NPC ---")]
    public float moveSpeed = 1.5f; // Tốc độ đi bộ (chậm hơn xe)

    [Header("--- Cảm Biến Khẩn Cấp (Tùy chọn) ---")]
    // NavMeshAgent đã tự né rồi, nhưng thêm cái này để dừng hẳn nếu gặp xe tông
    public float rayDistance = 1.0f;
    public LayerMask layerNguyHiem; // Chọn layer Car/Obstacle
    public bool isStopping = false;

    private NavMeshAgent agent;
    private int currentPoint = 0;

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();

        // Cài đặt thông số cho Agent
        agent.speed = moveSpeed;
        agent.autoBraking = false; // Để đi qua các điểm mượt mà không khựng lại
        agent.radius = 0.25f;      // Thu nhỏ người lại để dễ lách

        // Bắt đầu đi tới điểm đầu tiên
        if (waypoints.Length > 0)
        {
            agent.SetDestination(waypoints[0].position);
        }
    }

    void Update()
    {
        if (waypoints.Length == 0) return;

        // 1. CẢM BIẾN (Dừng lại nếu có vật nguy hiểm sát mặt)
        CheckObstacle();

        if (isStopping)
        {
            agent.isStopped = true; // Dừng lại
            return;
        }
        else
        {
            agent.isStopped = false; // Đi tiếp
        }

        // 2. KIỂM TRA ĐÍCH ĐẾN
        // Nếu còn cách đích dưới 0.5m và không đang tính toán đường
        if (!agent.pathPending && agent.remainingDistance < 0.5f)
        {
            GoToNextPoint();
        }
    }

    void CheckObstacle()
    {
        // Bắn tia từ bụng NPC ra phía trước
        Vector3 sensorStart = transform.position + Vector3.up * 0.8f;

        // Vẽ tia đỏ trong Scene để bạn dễ nhìn
        Debug.DrawRay(sensorStart, transform.forward * rayDistance, Color.red);

        if (Physics.Raycast(sensorStart, transform.forward, out RaycastHit hit, rayDistance, layerNguyHiem))
        {
            isStopping = true;
            // (Tùy chọn) Có thể thêm animation sợ hãi tại đây
        }
        else
        {
            isStopping = false;
        }
    }

    void GoToNextPoint()
    {
        // Chuyển sang điểm tiếp theo
        currentPoint++;

        // Xử lý vòng lặp
        if (currentPoint >= waypoints.Length)
        {
            if (loop) currentPoint = 0; // Quay về P1
            else return; // Đứng im tại chỗ
        }

        // Ra lệnh đi
        agent.SetDestination(waypoints[currentPoint].position);
    }
}