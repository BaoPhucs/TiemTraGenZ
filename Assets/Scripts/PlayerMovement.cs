using UnityEngine;

[RequireComponent(typeof(CharacterController))]
public class PlayerMovement : MonoBehaviour
{
    [Header("--- Cấu Hình Di Chuyển ---")]
    public float moveSpeed = 3.5f;   // Tốc độ đi bộ
    public float runSpeed = 7.0f;    // Tốc độ chạy (Shift)
    public float rotationSpeed = 12f;

    [Header("--- Cấu Hình Nhảy & Trọng Lực ---")]
    public float jumpHeight = 1.2f;  // Nhảy cao 1.2 mét
    public float gravity = -20f;     // Trọng lực (kéo xuống đất)

    [Header("--- Tham Chiếu ---")]
    public Transform cameraTransform; // Kéo Main Camera vào đây

    [Header("--- Animator Params ---")]
    public string speedParam = "Speed";
    public string groundedParam = "IsGrounded";
    public string isMovingParam = "IsMoving";

    private CharacterController controller;
    private Animator animator;
    private Vector3 velocity; // Dùng để tính toán rơi tự do và nhảy
    private bool hasSpeedParam;
    private bool hasGroundedParam;
    private bool hasIsMovingParam;

    // Các biến hỗ trợ xe đẩy (giữ nguyên logic cũ)
    private bool lockToForward;
    private Transform forwardLock;
    private bool hasSpeedOverride;
    private float savedSpeed;
    private bool movementBlocked;

    private void Awake()
    {
        controller = GetComponent<CharacterController>();
        animator = GetComponent<Animator>();

        // Kiểm tra xem Animator có các biến này không để tránh lỗi
        if (animator != null)
        {
            foreach (var param in animator.parameters)
            {
                if (param.name == speedParam) hasSpeedParam = true;
                else if (param.name == groundedParam) hasGroundedParam = true;
                else if (param.name == isMovingParam) hasIsMovingParam = true;
            }
        }
    }

    private void Update()
    {
        // Nếu đang bị chặn (ví dụ đang đẩy xe) thì đứng im
        if (movementBlocked)
        {
            StopMovement();
            return;
        }

        // 1. KIỂM TRA MẶT ĐẤT
        bool isGrounded = controller.isGrounded;
        if (isGrounded && velocity.y < 0)
        {
            velocity.y = -2f; // Reset lực rơi để nhân vật bám sát đất
        }

        // 2. XỬ LÝ DI CHUYỂN (WASD)
        Vector2 input = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));
        Vector3 move = Vector3.zero;

        // Xác định tốc độ hiện tại (Đi bộ hay Chạy?)
        // Nếu giữ Shift thì dùng runSpeed, không thì dùng moveSpeed
        float currentSpeed = Input.GetKey(KeyCode.LeftShift) ? runSpeed : moveSpeed;

        // Nếu có override (ví dụ đang đẩy xe) thì dùng tốc độ override
        if (hasSpeedOverride) currentSpeed = moveSpeed;

        if (input.sqrMagnitude > 0.0001f)
        {
            input.Normalize(); // Chuẩn hóa để đi chéo không bị nhanh hơn

            if (cameraTransform != null)
            {
                // Tính hướng đi theo Camera
                Vector3 camForward = cameraTransform.forward;
                Vector3 camRight = cameraTransform.right;
                camForward.y = 0f;
                camRight.y = 0f;
                camForward.Normalize();
                camRight.Normalize();

                move = camForward * input.y + camRight * input.x;
            }
            else
            {
                move = new Vector3(input.x, 0f, input.y);
            }

            // Xoay nhân vật theo hướng đi
            if (move != Vector3.zero)
            {
                Quaternion targetRot = Quaternion.LookRotation(move, Vector3.up);
                transform.rotation = Quaternion.Slerp(transform.rotation, targetRot, rotationSpeed * Time.deltaTime);
            }
        }

        // Thực hiện di chuyển ngang
        controller.Move(move * currentSpeed * Time.deltaTime);

        // 3. XỬ LÝ NHẢY (SPACE)
        if (Input.GetButtonDown("Jump") && isGrounded)
        {
            // Công thức vật lý: Vận tốc = Căn bậc 2 của (Độ cao * -2 * Trọng lực)
            velocity.y = Mathf.Sqrt(jumpHeight * -2f * gravity);

            // (Tùy chọn) Trigger animation nhảy nếu có
            // if (animator) animator.SetTrigger("Jump"); 
        }

        // 4. ÁP DỤNG TRỌNG LỰC (RƠI)
        velocity.y += gravity * Time.deltaTime;
        controller.Move(velocity * Time.deltaTime);

        // 5. CẬP NHẬT ANIMATION
        UpdateAnimator(move.magnitude * currentSpeed, isGrounded);
    }

    void StopMovement()
    {
        if (controller != null) controller.Move(Vector3.zero);
        velocity = Vector3.zero;
        UpdateAnimator(0, true);
    }

    void UpdateAnimator(float speed, bool grounded)
    {
        if (animator == null) return;

        bool isMoving = speed > 0.1f;

        if (hasSpeedParam) animator.SetFloat(speedParam, speed);
        if (hasGroundedParam) animator.SetBool(groundedParam, grounded);
        if (hasIsMovingParam) animator.SetBool(isMovingParam, isMoving);
    }

    // --- CÁC HÀM HỖ TRỢ XE ĐẨY (GIỮ NGUYÊN) ---
    public void SetForwardLock(Transform lockTransform, float speedOverride = -1f)
    {
        lockToForward = true;
        forwardLock = lockTransform;
        if (speedOverride > 0f)
        {
            if (!hasSpeedOverride)
            {
                savedSpeed = moveSpeed;
                hasSpeedOverride = true;
            }
            moveSpeed = speedOverride;
        }
    }

    public void ClearForwardLock()
    {
        lockToForward = false;
        forwardLock = null;
        if (hasSpeedOverride)
        {
            moveSpeed = savedSpeed;
            hasSpeedOverride = false;
        }
    }

    public void SetMovementBlocked(bool blocked)
    {
        movementBlocked = blocked;
    }

    public void Teleport(Vector3 position)
    {
        if (controller != null) controller.enabled = false;
        transform.position = position;
        velocity = Vector3.zero;
        if (controller != null) controller.enabled = true;
    }
}