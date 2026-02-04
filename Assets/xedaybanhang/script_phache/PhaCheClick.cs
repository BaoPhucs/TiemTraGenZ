using UnityEngine;

public class PhaCheClick : MonoBehaviour, IInteractable
{
    public enum ActionType
    {
        LayLy,
        DoTra,
        ThemTac,
        ThemDa,
        ThuHoi
    }

    public ActionType action;
    public PhaCheController phaChe;

    void Start()
    {
        // ƯU TIÊN 1: Tìm script ở ngay trên object cha hoặc chính nó (An toàn nhất)
        if (phaChe == null) phaChe = GetComponentInParent<PhaCheController>();

        // ƯU TIÊN 2: Nếu không thấy mới đi tìm lung tung
        if (phaChe == null) phaChe = FindObjectOfType<PhaCheController>();
    }

    public void Interact()
    {
        // Check lại lần nữa cho chắc
        if (phaChe == null) phaChe = GetComponentInParent<PhaCheController>();
        if (phaChe == null) phaChe = FindObjectOfType<PhaCheController>();

        if (phaChe != null)
        {
            switch (action)
            {
                case ActionType.LayLy: phaChe.LayLy(); break;
                case ActionType.DoTra: phaChe.DoTra(); break;
                case ActionType.ThemTac: phaChe.ThemTac(); break;
                case ActionType.ThemDa: phaChe.ThemDa(); break;
                case ActionType.ThuHoi: phaChe.ThuHoiLy(); break;
            }
        }
        else
        {
            Debug.LogError("Lỗi: Nút " + gameObject.name + " không tìm thấy Máy Pha Chế!");
        }
    }

    public string GetActionName()
    {
        // Hiển thị tên hành động kèm phím tắt
        switch (action)
        {
            case ActionType.LayLy: return "Lấy Ly (E)";
            case ActionType.DoTra: return "Đổ Trà (E)";
            case ActionType.ThemTac: return "Thêm Tắc (E)";
            case ActionType.ThemDa: return "Thêm Đá (E)";
            case ActionType.ThuHoi: return "Thu Hồi (E)";
            default: return "...";
        }
    }
}