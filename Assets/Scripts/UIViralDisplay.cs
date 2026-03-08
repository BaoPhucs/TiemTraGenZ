using UnityEngine;
using TMPro;

public class UIViralDisplay : MonoBehaviour
{
    private TextMeshProUGUI txtViral;

    void Start()
    {
        txtViral = GetComponent<TextMeshProUGUI>();
    }

    void Update()
    {
        if (txtViral != null && QuanLyKho.Instance != null)
        {
            // Hiển thị ra màn hình, bạn có thể trang trí thêm icon lửa 🔥 cho sinh động
            txtViral.text = "Độ Viral: " + QuanLyKho.Instance.DiemViral;
        }
    }
}