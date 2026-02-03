using UnityEngine;

public class CartInteraction : MonoBehaviour
{
    public Transform playerTransform; // Kéo Transform của Player vào đây
    public Transform cartTransform;   // Kéo Transform của Xe vào đây
    public Transform cartHoldPoint;   // Tạo 1 GameObject rỗng trước bụng nhân vật, kéo vào đây

    private bool isPushing = false;
    private Rigidbody cartRb;
    private Collider playerCol;
    private Collider cartCol;
    public string carTag = "Default";

    void Start()
    {
        cartRb = cartTransform.GetComponent<Rigidbody>();
        playerCol = playerTransform.GetComponent<Collider>();
        cartCol = cartTransform.GetComponent<Collider>();
    }

    void Update()
    {
        // Giả sử phím E để bắt đầu/kết thúc đẩy
        if (Input.GetKeyDown(KeyCode.E))
        {
            TogglePush();
        }

        // Nếu đang đẩy, khóa cứng vị trí xe vào tay nhân vật
        if (isPushing)
        {
            // Cập nhật vị trí xe theo điểm cầm (Hold Point)
            cartTransform.position = cartHoldPoint.position;

            // Cập nhật hướng xoay của xe theo hướng người
            cartTransform.rotation = playerTransform.rotation;
        }
    }

    void TogglePush()
    {
        isPushing = !isPushing;

        if (isPushing)
        {
            // KHI BẮT ĐẦU ĐẨY:

            // 1. Tắt vật lý của xe để không bị xung đột (QUAN TRỌNG)
            cartRb.isKinematic = true;

            // 2. Bỏ qua va chạm giữa người và xe để không bị đẩy bay
            Physics.IgnoreCollision(playerCol, cartCol, true);

            // 3. Trigger Animation đẩy (nếu có)
            // playerAnimator.SetBool("isPushing", true);
        }
        else
        {
            // KHI THẢ XE RA:

            // 1. Bật lại vật lý
            cartRb.isKinematic = false;

            // 2. Bật lại va chạm
            Physics.IgnoreCollision(playerCol, cartCol, false);

            // 3. Tắt Animation
            // playerAnimator.SetBool("isPushing", false);
        }

        void OnCollisionEnter(Collision collision)
        {
            // Kiểm tra nếu vật va chạm là Xe Ô tô
            if (collision.gameObject.CompareTag(carTag))
            {
                // 1. Nếu đang đẩy xe thì BUÔNG RA NGAY LẬP TỨC
                if (isPushing)
                {
                    TogglePush(); // Gọi hàm buông xe (đã viết ở bài trước)
                }

                // 2. Xử lý lực va chạm để không bay lên trời
                Rigidbody rb = GetComponent<Rigidbody>();
                if (rb != null)
                {
                    // Hủy bỏ vận tốc hiện tại (để tránh cộng dồn lực)
                    rb.velocity = Vector3.zero;

                    // Tính toán lực đẩy lùi (Knockback) nhưng CHỈ theo phương ngang (bỏ trục Y)
                    Vector3 pushDir = collision.transform.position - transform.position;
                    pushDir.y = 0; // Quan trọng: Ép lực đẩy theo trục Y về 0 -> Không bay lên
                    pushDir = -pushDir.normalized; // Đẩy ngược lại hướng va chạm

                    // Đẩy nhân vật trượt đi
                    rb.AddForce(pushDir * 10f, ForceMode.Impulse);
                }
            }
        }

        // Nếu bạn dùng CharacterController thay vì Rigidbody thì dùng hàm này:
        void OnControllerColliderHit(ControllerColliderHit hit)
        {
            if (hit.gameObject.CompareTag(carTag))
            {
                if (isPushing)
                {
                    TogglePush(); // Buông xe ngay
                }
                // Với CharacterController, việc xử lý lực đẩy sẽ phức tạp hơn chút,
                // nhưng quan trọng nhất vẫn là phải Buông Xe ra.
            }
        }
    }
}