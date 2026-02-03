using UnityEngine;

public class PhaCheClick : MonoBehaviour, IInteractable
{
    public enum ActionType
    {
        LayLy,
        DoTra,
        ThemTac,
        ThemDa
    }

    public ActionType action;
    public PhaCheController phaChe; // Kéo script PhaCheController của xe vào

    void Start()
    {
        if (phaChe == null)
        {
            // Tìm cái xe đẩy đang có trong màn hình
            phaChe = FindObjectOfType<PhaCheController>();
        }
    }

    public void Interact()
    {
        // Kiểm tra lại lần nữa cho chắc
        if (phaChe == null) phaChe = FindObjectOfType<PhaCheController>();

        if (phaChe == null) return; // Nếu vẫn không tìm thấy thì thôi

        switch (action)
        {
            case ActionType.LayLy: phaChe.LayLy(); break;
            case ActionType.DoTra: phaChe.DoTra(); break;
            case ActionType.ThemTac: phaChe.ThemTac(); break;
            case ActionType.ThemDa: phaChe.ThemDa(); break;
        }
    }

    public string GetActionName()
    {
        // Trả về tên hành động để hiện lên màn hình
        switch (action)
        {
            case ActionType.LayLy: return "Lấy Ly";
            case ActionType.DoTra: return "Đổ Trà";
            case ActionType.ThemTac: return "Thêm Tắc";
            case ActionType.ThemDa: return "Thêm Đá";
            default: return "...";
        }
    }
}