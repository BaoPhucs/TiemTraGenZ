using UnityEngine;
using TMPro;

public class FloatingText : MonoBehaviour
{
    public float tocDoBay = 50f;
    public float thoiGianBienMat = 1f;
    private TextMeshProUGUI textMesh;
    private float timer;
    private Color startColor;

    void Awake()
    {
        textMesh = GetComponent<TextMeshProUGUI>();
        startColor = textMesh.color;
    }

    public void KhoiTao(string noiDung, bool laTienLoi)
    {
        textMesh.text = noiDung;
        // Nếu lời (true) -> Xanh, Lỗ (false) -> Đỏ
        textMesh.color = laTienLoi ? Color.green : Color.red;
        startColor = textMesh.color;
        timer = 0;
    }

    void Update()
    {
        // 1. Bay lên
        transform.Translate(Vector3.up * tocDoBay * Time.deltaTime);

        // 2. Mờ dần
        timer += Time.deltaTime;
        float alpha = Mathf.Lerp(1f, 0f, timer / thoiGianBienMat);
        textMesh.color = new Color(startColor.r, startColor.g, startColor.b, alpha);

        // 3. Tự hủy
        if (timer >= thoiGianBienMat) Destroy(gameObject);
    }
}