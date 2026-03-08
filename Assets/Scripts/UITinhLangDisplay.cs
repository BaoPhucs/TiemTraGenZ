using UnityEngine;
using TMPro;

public class UITinhLangDisplay : MonoBehaviour
{
    private TextMeshProUGUI txtTinhLang;

    void Start()
    {
        txtTinhLang = GetComponent<TextMeshProUGUI>();
    }

    void Update()
    {
        if (txtTinhLang != null && QuanLyKho.Instance != null)
        {
            // Hiển thị ra màn hình (Icon trái tim 💚)
            txtTinhLang.text = "Tình Làng: " + QuanLyKho.Instance.DiemTinhLang;
        }
    }
}