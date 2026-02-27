using UnityEngine;

public class CameraController : MonoBehaviour
{
    [Header("Target")]
    public Transform player;       // Kéo nhân vật Minh vào đây
    public Transform lookTarget;   // Kéo cái cổ hoặc đầu nhân vật vào đây (để nhìn cho chuẩn)

    [Header("Settings")]
    public float mouseSensitivity = 2.0f;
    public float pitchMin = -40f;  // Góc ngẩng tối đa
    public float pitchMax = 80f;   // Góc cúi tối đa

    [Header("View Modes")]
    public float thirdPersonDist = 4.0f; // Khoảng cách góc nhìn thứ 3
    public Vector3 thirdPersonOffset = new Vector3(0, 1.5f, 0); // Độ cao camera góc nhìn 3
    public Vector3 firstPersonOffset = new Vector3(0, 1.6f, 0.2f); // Vị trí mắt (trước mặt chút xíu)

    private bool isFirstPerson = false;
    private float currentX = 0f;
    private float currentY = 0f;

    void Start()
    {
        // Ẩn chuột khi chơi
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    void LateUpdate()
    {
        if (player == null) return;

        if (Time.timeScale == 0f) return;

        // 1. Nhận input chuột
        currentX += Input.GetAxis("Mouse X") * mouseSensitivity;
        currentY -= Input.GetAxis("Mouse Y") * mouseSensitivity;
        currentY = Mathf.Clamp(currentY, pitchMin, pitchMax);

        // 2. Chuyển đổi góc nhìn khi bấm V
        if (Input.GetKeyDown(KeyCode.V))
        {
            isFirstPerson = !isFirstPerson;
        }

        // 3. Tính toán vị trí & Góc xoay
        Vector3 direction = new Vector3(0, 0, -thirdPersonDist);
        Quaternion rotation = Quaternion.Euler(currentY, currentX, 0);

        if (isFirstPerson)
        {
            // Góc nhìn thứ 1: Gắn chặt vào vị trí mắt, xoay theo chuột
            transform.position = player.position + (player.rotation * firstPersonOffset);
            transform.rotation = rotation;

            // Xoay nhân vật theo hướng camera ngay lập tức
            player.rotation = Quaternion.Euler(0, currentX, 0);
        }
        else
        {
            // Góc nhìn thứ 3: Camera xoay quanh nhân vật
            // Tính vị trí mong muốn
            Vector3 targetPos = player.position + thirdPersonOffset + rotation * direction;

            // (Nâng cao) Raycast chống xuyên tường nếu cần, tạm thời dùng targetPos
            transform.position = targetPos;
            transform.LookAt(player.position + thirdPersonOffset);
        }
    }
}