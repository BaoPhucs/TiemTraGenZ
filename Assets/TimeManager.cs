using UnityEngine;
using TMPro;

public class TimeManager : MonoBehaviour
{
    public float gioHienTai = 6.0f; 
    public float tocDoThoiGian = 1.0f; 
    public TextMeshProUGUI dongHoHienThi;
    public bool daHetGio = false;

    void Update()
    {
        if (daHetGio) return;

        gioHienTai += Time.deltaTime * tocDoThoiGian / 60.0f; 

        int gio = Mathf.FloorToInt(gioHienTai);
        int phut = Mathf.FloorToInt((gioHienTai - gio) * 60);
        if (dongHoHienThi != null)
            dongHoHienThi.text = string.Format("{0:00}:{1:00}", gio, phut);

        if (gioHienTai >= 22.0f)
        {
            Debug.Log("Đã 10 giờ tối! Dọn hàng thôi.");
        }
    }
}