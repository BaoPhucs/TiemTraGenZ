using System.Collections.Generic;
using UnityEngine;

public class PlayerInteraction : MonoBehaviour
{
    [Header("Cấu hình Tương tác")]
    public KeyCode interactKey = KeyCode.F;
    public float interactRange = 1.5f;
    public LayerMask pushableMask = ~0;

    [Header("Cấu hình Đẩy")]
    public float pushSpeed = 4.0f; // Tốc độ đẩy
    public float pushTurnSpeed = 120f; // Tốc độ xoay
    public string pushParam = "IsPushing";
    public bool ignoreCartCollision = true;

    [Header("Chống Xuyên Tường")]
    public LayerMask wallLayer; // Chọn layer Default, Building... (những thứ muốn chặn xe)
    public float collisionCheckDist = 0.5f; // Khoảng cách check va chạm phía trước xe

    private PlayerMovement movement;
    private Animator animator;
    private CharacterController characterController;
    private PushableCart currentCart;
    private bool isPushing;

    private void Awake()
    {
        movement = GetComponent<PlayerMovement>();
        animator = GetComponent<Animator>();
        if (animator == null) animator = GetComponentInChildren<Animator>();
        characterController = GetComponent<CharacterController>();
        if (animator != null) animator.applyRootMotion = false;
    }

    private void Update()
    {
        if (Input.GetKeyDown(interactKey))
        {
            if (isPushing) StopPush();
            else TryStartPush();
        }

        if (isPushing && currentCart != null)
        {
            UpdatePushingPhysics();
        }
    }

    private void UpdatePushingPhysics()
    {
        float hor = Input.GetAxis("Horizontal");
        float ver = Input.GetAxis("Vertical");

        // 1. XỬ LÝ DI CHUYỂN
        if (Mathf.Abs(ver) > 0.01f || Mathf.Abs(hor) > 0.01f)
        {
            // Hướng muốn di chuyển (Tiến/Lùi theo hướng xe)
            Vector3 moveDir = currentCart.handle.forward * ver;

            // --- CHECK XUYÊN TƯỜNG Ở ĐÂY ---
            if (CanMove(moveDir))
            {
                // Nếu không vướng tường thì mới cho đi
                currentCart.MoveBy(moveDir * pushSpeed * Time.deltaTime);
            }

            // Xoay xe
            if (Mathf.Abs(hor) > 0.01f)
            {
                float turnAmount = hor * pushTurnSpeed * Time.deltaTime;
                if (ver < -0.01f) turnAmount *= -1;
                currentCart.transform.Rotate(0, turnAmount, 0);
            }
        }

        SnapPlayerToHandle();
    }

    // Hàm kiểm tra va chạm phía trước
    bool CanMove(Vector3 direction)
    {
        if (currentCart == null) return false;

        // Chỉ check khi đi tới (ver > 0) hoặc lùi (ver < 0)
        if (direction.magnitude < 0.01f) return true;

        // Bắn tia từ tâm xe hoặc từ BoxCollider của xe
        // Lấy BoxCollider của xe để tính toán kích thước
        BoxCollider cartCol = currentCart.GetComponentInChildren<BoxCollider>();
        if (cartCol == null) return true; // Không có collider thì cứ đi

        // Tâm check: Chính giữa xe
        Vector3 center = currentCart.transform.TransformPoint(cartCol.center);
        // Kích thước check: Bằng 1/2 kích thước xe (trừ đi 1 xíu để không dính sàn)
        Vector3 size = Vector3.Scale(cartCol.size, currentCart.transform.localScale) / 2;
        size.x -= 0.05f; size.y -= 0.05f; size.z -= 0.05f; // Thu nhỏ tí xíu

        // Bắn hộp (BoxCast) về phía muốn đi
        // Nếu chạm vào WallLayer -> Trả về False (Không được đi)
        if (Physics.BoxCast(center, size, direction, Quaternion.identity, collisionCheckDist, wallLayer))
        {
            return false; // Đụng tường!
        }

        return true; // Đường thoáng
    }

    private void SnapPlayerToHandle()
    {
        if (currentCart == null || currentCart.handle == null) return;
        Vector3 targetPos = currentCart.handle.position;
        if (characterController != null) characterController.enabled = false;
        transform.position = targetPos;
        transform.rotation = currentCart.handle.rotation;
        if (characterController != null) characterController.enabled = true;
    }

    private void TryStartPush()
    {
        // 1. Logic tìm xe gần nhất (GIỮ NGUYÊN KHÔNG ĐỔI)
        Collider[] hits = Physics.OverlapSphere(transform.position, interactRange, pushableMask, QueryTriggerInteraction.Ignore);
        float closestDist = Mathf.Infinity;
        PushableCart bestCart = null;

        foreach (var hit in hits)
        {
            PushableCart cart = hit.GetComponentInParent<PushableCart>();
            if (cart != null)
            {
                float d = Vector3.Distance(transform.position, cart.GetHandlePosition());
                if (d < closestDist)
                {
                    closestDist = d;
                    bestCart = cart;
                }
            }
        }

        // 2. Logic Kiểm tra điều kiện Dọn Hàng (MỚI THÊM VÀO)
        if (bestCart != null)
        {
            // Lấy script QuanLyKho trên cái xe tìm được
            QuanLyKho kho = bestCart.GetComponent<QuanLyKho>();

            // Nếu xe có gắn script quản lý kho, thì phải kiểm tra
            if (kho != null)
            {
                if (kho.ConDoBenNgoai())
                {
                    Debug.Log("⛔ Cất hết bàn ghế, thùng đá vào xe rồi mới được đẩy về!");
                    // Nếu bạn có UI thông báo, gọi hàm hiện thông báo ở đây
                    return; // NGẮT LỆNH: Không cho thực hiện StartPush
                }
            }

            // 3. Nếu mọi thứ ok (hoặc xe không có kho) -> Bắt đầu đẩy
            StartPush(bestCart);
        }
    }
    private void StartPush(PushableCart cart)
    {
        currentCart = cart;
        isPushing = true;
        cart.BeginPush();
        if (ignoreCartCollision) IgnoreCollisions(cart, true);
        if (movement != null) movement.enabled = false;
        if (animator != null) animator.SetBool(pushParam, true);
        SnapPlayerToHandle();
    }

    private void StopPush()
    {
        isPushing = false;
        if (currentCart != null && ignoreCartCollision) IgnoreCollisions(currentCart, false);
        if (currentCart != null) currentCart.EndPush();
        if (movement != null) movement.enabled = true;
        if (animator != null) animator.SetBool(pushParam, false);
        currentCart = null;
    }

    private void IgnoreCollisions(PushableCart cart, bool ignore)
    {
        if (characterController == null) return;
        Collider[] cartColliders = cart.GetComponentsInChildren<Collider>();
        foreach (var col in cartColliders)
        {
            if (col != null && !col.isTrigger)
                Physics.IgnoreCollision(characterController, col, ignore);
        }
    }

    private void OnDrawGizmos()
    {
        // 1. Vẽ vùng tương tác F (Màu vàng)
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, interactRange);

        // 2. Vẽ hộp check va chạm tường (Màu Đỏ) - CHỈ HIỆN KHI ĐANG ĐẨY XE
        if (isPushing && currentCart != null)
        {
            Gizmos.color = Color.red;
            BoxCollider cartCol = currentCart.GetComponentInChildren<BoxCollider>();
            if (cartCol != null)
            {
                // Mô phỏng vị trí cái hộp check tường
                Vector3 center = currentCart.transform.TransformPoint(cartCol.center);
                Vector3 size = Vector3.Scale(cartCol.size, currentCart.transform.localScale);
                size.x -= 0.1f; size.y -= 0.1f; size.z -= 0.1f;

                // Vẽ cái hộp phía trước xe
                Vector3 forwardPos = center + currentCart.transform.forward * collisionCheckDist;
                Gizmos.matrix = Matrix4x4.TRS(forwardPos, currentCart.transform.rotation, size);
                Gizmos.DrawWireCube(Vector3.zero, Vector3.one);
            }
        }
    }
}