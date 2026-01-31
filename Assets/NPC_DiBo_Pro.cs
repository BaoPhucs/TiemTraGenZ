using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent))]
public class NPC_DiBo_Pro : MonoBehaviour
{
    [Header("--- Cấu Hình Đi Thẳng ---")]
    public float lookAheadDist = 8f;   // Giảm xuống chút để đỡ tìm điểm quá xa
    public float sideWander = 3f;      // Tăng lên để nó biết lách rộng hơn

    [Header("--- Cấu Hình NPC ---")]
    public float moveSpeed = 1.5f;

    [Header("--- Cảm Biến Khẩn Cấp ---")]
    public float rayDistance = 1.0f;
    public float rayHeight = 0.15f;
    public LayerMask layerNguyHiem;

    // --- BIẾN MỚI: XỬ LÝ KẸT ---
    private float stuckTimer = 0f;     // Đếm thời gian bị đứng im
    private bool isRecovering = false; // Đang trong chế độ "gỡ kẹt"

    private NavMeshAgent agent;
    private Animator anim;
    private bool isStopping = false;

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        anim = GetComponent<Animator>();

        agent.speed = moveSpeed;
        agent.autoBraking = false;
        agent.obstacleAvoidanceType = ObstacleAvoidanceType.HighQualityObstacleAvoidance; // Né nhau xịn hơn

        DiTiep();
    }

    void Update()
    {
        UpdateAnimation();

        // 1. CẢM BIẾN (Logic cũ)
        CheckObstacle();
        if (isStopping)
        {
            agent.isStopped = true;
            stuckTimer = 0; // Đang dừng chủ động thì không tính là kẹt
            return;
        }
        else
        {
            agent.isStopped = false;
        }

        // 2. KIỂM TRA CÓ BỊ KẸT KHÔNG? (Logic Mới)
        // Nếu vận tốc thực tế < 0.1m/s dù đang không bị lệnh dừng
        if (agent.velocity.magnitude < 0.1f && !agent.pathPending)
        {
            stuckTimer += Time.deltaTime;

            // Nếu đứng im quá 0.01 giây -> Kích hoạt gỡ kẹt
            if (stuckTimer > 0.01f)
            {
                GoKet();
            }
        }
        else
        {
            stuckTimer = 0; // Nếu đi được thì reset bộ đếm
        }

        // 3. LOGIC ĐI TIẾP
        // Nếu đã đến điểm gỡ kẹt hoặc điểm đến bình thường
        if (!agent.pathPending && agent.remainingDistance < 1.5f)
        {
            isRecovering = false; // Hết chế độ gỡ kẹt
            DiTiep();             // Quay lại đi thẳng
        }
    }

    void DiTiep()
    {
        // Tính điểm phía trước
        Vector3 duongDiThang = transform.position + transform.forward * lookAheadDist;

        // Random sang hai bên
        float lechTraiPhai = Random.Range(-sideWander, sideWander);
        duongDiThang += transform.right * lechTraiPhai;

        NavMeshHit hit;
        // Kiểm tra xem điểm đó có nằm trên NavMesh không (trong bán kính 5m)
        if (NavMesh.SamplePosition(duongDiThang, out hit, 5.0f, NavMesh.AllAreas))
        {
            agent.SetDestination(hit.position);
        }
        else
        {
            // Nếu điểm phía trước bị lỗi (do rơi xuống vực/hết đường), gọi hàm Gỡ Kẹt ngay
            GoKet();
        }
    }

    // Hàm mới: Tìm đại một điểm gần đó để đi (để thoát khỏi mép tường)
    void GoKet()
    {
        isRecovering = true;
        stuckTimer = 0;

        // Tìm một điểm ngẫu nhiên trong bán kính 5m (kể cả phía sau lưng)
        Vector3 randomPoint = transform.position + Random.insideUnitSphere * 5f;

        NavMeshHit hit;
        if (NavMesh.SamplePosition(randomPoint, out hit, 5f, NavMesh.AllAreas))
        {
            agent.SetDestination(hit.position);
            // Debug.Log("Đang gỡ kẹt..."); // Bật cái này nếu muốn test
        }
    }

    void UpdateAnimation()
    {
        if (anim == null) return;
        // Lấy vận tốc từ Agent cho chính xác
        bool dangDiChuyen = agent.velocity.magnitude > 0.1f;
        anim.SetBool("IsMoving", dangDiChuyen);
    }

    void CheckObstacle()
    {
        Vector3 sensorStart = transform.position + Vector3.up * rayHeight;
        if (Physics.Raycast(sensorStart, transform.forward, out RaycastHit hit, rayDistance, layerNguyHiem))
        {
            isStopping = true;
        }
        else
        {
            isStopping = false;
        }
    }
}