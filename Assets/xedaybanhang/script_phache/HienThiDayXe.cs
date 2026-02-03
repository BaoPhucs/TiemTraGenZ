using UnityEngine;

public class HienThiDayXe : MonoBehaviour, IInteractable
{
    // Script này chỉ có tác dụng HIỆN CHỮ để người chơi biết
    // Còn logic vật lý đẩy xe thì script PushableCart vẫn lo (phím F)

    public void Interact()
    {
        // Hàm này để trống, vì ta dùng phím F để đẩy chứ không dùng phím E
    }

    public string GetActionName()
    {
        // Mẹo: Vì hệ thống mặc định hiện chữ [E], ta ghi chú luôn phím F vào đây
        return "Đẩy Xe (F)";
    }
}