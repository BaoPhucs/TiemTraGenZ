using UnityEngine;

public class TramNguyenLieu : MonoBehaviour
{
    public enum LoaiTram { Tac, Chanh, TraSua, Matcha, CaPhe, Sua }

    [Header("Trạm này chứa nguyên liệu gì?")]
    public LoaiTram loaiTramCuaToi;

    [Header("Kéo Model 3D (Thằng Con) vào đây")]
    public GameObject modelThucTe;

    void Update()
    {
        // Nếu chưa có Kho hoặc chưa gắn Model thì bỏ qua
        if (QuanLyKho.Instance == null || modelThucTe == null) return;

        bool daMoKhoa = false;

        // Hỏi Kho xem đã mua bí kíp chưa?
        switch (loaiTramCuaToi)
        {
            case LoaiTram.Tac: daMoKhoa = QuanLyKho.Instance.unlockTraTac; break;
            case LoaiTram.Chanh: daMoKhoa = QuanLyKho.Instance.unlockTraChanh; break;
            case LoaiTram.TraSua: daMoKhoa = QuanLyKho.Instance.unlockTraSua; break;
            case LoaiTram.Matcha: daMoKhoa = QuanLyKho.Instance.unlockMatcha; break;
            case LoaiTram.CaPhe: daMoKhoa = QuanLyKho.Instance.unlockCaPheDen || QuanLyKho.Instance.unlockCaPheSua; break;
            case LoaiTram.Sua: daMoKhoa = QuanLyKho.Instance.unlockMatcha || QuanLyKho.Instance.unlockCaPheSua; break;
        }

        // Tàng hình / Hiện hình Thằng Con
        if (modelThucTe.activeSelf != daMoKhoa)
        {
            modelThucTe.SetActive(daMoKhoa);
        }
    }
}