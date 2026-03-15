using UnityEngine;
using UnityEngine.AI;

public class KidAI : MonoBehaviour
{
    [Header("Cài đặt khu vui chơi")]
    public float playRadius = 15f;    // Bán kính khu vực cho phép chạy nhảy
    public float minWaitTime = 0.5f;  // Thời gian nghỉ ít nhất
    public float maxWaitTime = 2.5f;  // Thời gian nghỉ lâu nhất

    private NavMeshAgent agent;
    private Animator anim;
    private Vector3 centerPoint;      // Tâm của khu vui chơi (chính là chỗ sếp đặt nó lúc đầu)
    private float waitCounter;
    private bool isWaiting;

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        anim = GetComponent<Animator>();

        // Lấy vị trí sếp đặt nó trên Scene làm tâm của "Sân chơi"
        centerPoint = transform.position;

        // Trẻ con thì phải chạy nhanh hơn người lớn đi bộ (Sếp có thể tự chỉnh)
        agent.speed = 4.5f;
        agent.acceleration = 8f; // Gia tốc nhanh để rẽ gắt

        PickNewDestination();
    }

    void Update()
    {
        // Nếu đã chạy đến đích (hoặc bị kẹt rớt lại rất gần đích)
        if (!agent.pathPending && agent.remainingDistance <= 0.5f)
        {
            if (!isWaiting)
            {
                // Bắt đầu đứng nghỉ thở
                isWaiting = true;
                waitCounter = Random.Range(minWaitTime, maxWaitTime);
                if (anim != null) anim.SetBool("isRunning", false); // Tắt animation chạy
            }
            else
            {
                // Đếm ngược thời gian nghỉ
                waitCounter -= Time.deltaTime;
                if (waitCounter <= 0)
                {
                    PickNewDestination(); // Nghỉ xong lại chạy tiếp
                }
            }
        }
    }

    void PickNewDestination()
    {
        // Random 1 điểm bất kỳ trong vòng tròn bán kính playRadius
        Vector3 randomDirection = Random.insideUnitSphere * playRadius;
        randomDirection += centerPoint; // Cộng với tọa độ tâm để không chạy ra ngoài đường lớn

        NavMeshHit hit;
        // Dò xem điểm vừa random có nằm trên mặt đường (NavMesh) không
        if (NavMesh.SamplePosition(randomDirection, out hit, playRadius, NavMesh.AllAreas))
        {
            agent.SetDestination(hit.position); // Ra lệnh chạy tới đó
            isWaiting = false;

            if (anim != null) anim.SetBool("isRunning", true); // Bật animation chạy
        }
    }
}