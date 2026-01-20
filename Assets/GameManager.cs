using UnityEngine;
using TMPro; // Để hiển thị thông báo

public class GameManager : MonoBehaviour
{
    [Header("THAM CHIẾU")]
    public HomeZone khuVucNha;
    public GarageDoor cuaCuon;
    public string tagGhe = "Chair"; // Tag của cái ghế

    [Header("GIAO DIỆN (UI)")]
    public GameObject panelDoanhThu;  // Bảng thành công
    public GameObject panelCanhBao;   // Bảng báo lỗi
    public TextMeshProUGUI textLoi;   // Dòng chữ ghi lỗi cụ thể

    public void KiemTraKetThucNgay()
    {
        // 1. Kiểm tra Xe vào nhà chưa?
        if (khuVucNha.xeDaVaoNha == false)
        {
            HienLoi("Xe vẫn đang ở ngoài đường! Hãy đẩy xe vào nhà.");
            return;
        }

        // 2. Kiểm tra Cửa đóng chưa?
        if (cuaCuon.isClosed == false)
        {
            HienLoi("Cửa cuốn chưa đóng! Hãy bấm E để đóng cửa.");
            return;
        }

        // 3. KIỂM TRA GHẾ (MỚI)
        // Tìm tất cả vật thể có Tag là "Chair" đang tồn tại trong game
        int soGheConLai = GameObject.FindGameObjectsWithTag(tagGhe).Length;

        if (soGheConLai > 0)
        {
            HienLoi($"Vẫn còn {soGheConLai} cái ghế ngoài đường! Hãy thu dọn hết.");
            return;
        }

        // NẾU TẤT CẢ OK -> THÀNH CÔNG
        Debug.Log("NGÀY LÀM VIỆC HOÀN HẢO!");
        if (panelCanhBao != null) panelCanhBao.SetActive(false); // Tắt bảng lỗi nếu đang hiện
        if (panelDoanhThu != null) panelDoanhThu.SetActive(true); // Hiện bảng lương
    }

    void HienLoi(string noiDung)
    {
        Debug.LogWarning(noiDung);
        if (panelCanhBao != null)
        {
            panelCanhBao.SetActive(true); // Bật bảng lỗi lên
            if (textLoi != null) textLoi.text = noiDung; // Ghi nội dung lỗi
        }
    }
}