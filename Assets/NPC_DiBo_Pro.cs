using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent))]
public class NPC_DiBo_Pro : MonoBehaviour
{
    [Header("--- Cấu Hình Đi Lại ---")]
    public float lookAheadDist = 10f;
    public float sideWander = 5f;

    [Header("--- Cấu Hình NPC ---")]
    public float moveSpeed = 1.5f;

    [Header("--- Cảm Biến (Mắt) ---")]
    public float rayDistance = 1.5f;   // Nhìn xa hơn chút
    public float rayHeight = 0.5f;     // Nâng cao tầm mắt lên bụng (đỡ nhìn xuống vỉa hè)
    public LayerMask layerNguyHiem;    // Chọn Everything

    // --- BIẾN XỬ LÝ KẸT ---
    private float stuckTimer = 0f;


    private NavMeshAgent agent;
    private Animator anim;
    private bool isStopping = false;

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        anim = GetComponent<Animator>();

        agent.speed = moveSpeed;
        agent.autoBraking = false;
        agent.obstacleAvoidanceType = ObstacleAvoidanceType.HighQualityObstacleAvoidance;

        DiTiep();
    }

    void Update()
    {
        UpdateAnimation();

        // 1. CẢM BIẾN THÔNG MINH
        CheckObstacle();

        // Xử lý dừng/chạy
        if (isStopping)
        {
            agent.isStopped = true;
            agent.velocity = Vector3.zero; // Phanh gấp
        }
        else
        {
            agent.isStopped = false;
        }

        // 2. TỰ ĐỘNG GỠ KẸT (NẾU ĐỨNG IM QUÁ LÂU)
        // Chỉ tính giờ kẹt khi KHÔNG bị lệnh dừng chủ động (tức là đang muốn đi mà không đi được)
        if (!isStopping && agent.velocity.magnitude < 0.1f)
        {
            stuckTimer += Time.deltaTime;
            if (stuckTimer > 2.0f) // Nếu đứng im 2 giây
            {
                GoKet(); // Quay đầu đi chỗ khác
            }
        }
        else
        {
            stuckTimer = 0;
        }

        // 3. LOGIC ĐẾN ĐÍCH
        if (!agent.pathPending && agent.remainingDistance < 1.0f)
        {
            DiTiep();
        }
    }

    void DiTiep()
    {
        // Chọn điểm ngẫu nhiên phía trước
        Vector3 duongDiThang = transform.position + transform.forward * lookAheadDist;
        float lechTraiPhai = Random.Range(-sideWander, sideWander);
        duongDiThang += transform.right * lechTraiPhai;

        SetDestinationSafe(duongDiThang);
    }

    void GoKet()
    {
        stuckTimer = 0;
        // Tìm điểm ngẫu nhiên xung quanh để thoát thân
        Vector3 randomPoint = transform.position + Random.insideUnitSphere * 6f;
        SetDestinationSafe(randomPoint);
    }

    void SetDestinationSafe(Vector3 target)
    {
        NavMeshHit hit;
        if (NavMesh.SamplePosition(target, out hit, 5.0f, NavMesh.AllAreas))
        {
            agent.SetDestination(hit.position);
        }
        else
        {
            // Nếu điểm đến bị lỗi, thử lại điểm khác ngay
            GoKet();
        }
    }

    void CheckObstacle()
    {
        Vector3 sensorStart = transform.position + Vector3.up * rayHeight;
        RaycastHit hit;

        // Vẽ tia đỏ debug
        Debug.DrawRay(sensorStart, transform.forward * rayDistance, Color.red);

        if (Physics.Raycast(sensorStart, transform.forward, out hit, rayDistance, layerNguyHiem))
        {
            // --- LOGIC QUAN TRỌNG: CHỈ DỪNG KHI GẶP XE ---
            // Nếu vật cản có Tag là "Car" hoặc "Vehicle" thì mới dừng
            if (hit.collider.CompareTag("Car") || hit.collider.CompareTag("Vehicle"))
            {
                isStopping = true;
            }
            else
            {
                // Gặp cây, tường, người... -> KHÔNG DỪNG.
                // Để NavMeshAgent tự lách qua.
                isStopping = false;

                // Nếu vật cản quá gần (dưới 0.3m) mà Agent vẫn đang đâm đầu vào -> Gọi gỡ kẹt ngay
                if (hit.distance < 0.3f)
                {
                    stuckTimer += Time.deltaTime * 5; // Tăng tốc độ đếm kẹt
                }
            }
        }
        else
        {
            isStopping = false;
        }
    }

    void UpdateAnimation()
    {
        if (anim == null) return;
        bool dangDiChuyen = agent.velocity.magnitude > 0.1f;
        anim.SetBool("IsMoving", dangDiChuyen);
    }
}