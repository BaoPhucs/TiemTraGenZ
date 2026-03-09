using UnityEngine;
using TMPro;

public class PlayerInteract : MonoBehaviour
{
    [Header("--- Cấu Hình ---")]
    public float reachDistance = 10f; // Tăng lên 10m để với tới cửa từ xa
    public LayerMask interactLayer;   // Chọn Layer "Interact"
    public TextMeshProUGUI interactText; // Kéo cái Text hiển thị chữ "[E] Mở Cửa"

    void Update()
    {
        // Bắn tia từ CHÍNH GIỮA MÀN HÌNH (Góc nhìn của Camera)
        Ray ray = Camera.main.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0));
        RaycastHit hit;

        // Vẽ tia đỏ để bạn debug (nhìn trong Scene)
        Debug.DrawRay(ray.origin, ray.direction * reachDistance, Color.red);

        if (Physics.Raycast(ray, out hit, reachDistance, interactLayer))
        {
            // Kiểm tra xem vật đó có chức năng tương tác không (Cửa, Đèn...)
            IInteractable item = hit.collider.GetComponent<IInteractable>();

            if (item != null)
            {
                // Hiện thông báo (nếu có UI)
                if (interactText != null)
                {
                    interactText.text = item.GetActionName();
                    interactText.gameObject.SetActive(true);
                }

                // Bấm E để thực hiện
                if (Input.GetKeyDown(KeyCode.E))
                {
                    item.Interact();
                }
                return; // Kết thúc để không chạy đoạn ẩn text bên dưới
            }
        }

        // Nếu không nhìn thấy gì thì ẩn chữ đi
        if (interactText != null) interactText.gameObject.SetActive(false);
    }
}

// Interface này bắt buộc phải có để các vật thể (Cửa, Ghế) dùng chung
public interface IInteractable
{
    void Interact();
    string GetActionName();
}