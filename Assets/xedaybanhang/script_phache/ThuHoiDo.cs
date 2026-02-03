using UnityEngine;

public class ThuHoiDo : MonoBehaviour, IInteractable
{
    public string loaiDo; // "Ghe", "Ban", "ThungDa"

    public void Interact()
    {
        // Báo cho thủ kho biết là đã trả đồ
        if (QuanLyKho.Instance != null)
        {
            QuanLyKho.Instance.ThuDoVe(loaiDo);
        }

        // Tự hủy
        Destroy(gameObject);
    }

    public string GetActionName()
    {
        return "Cất " + loaiDo;
    }
}