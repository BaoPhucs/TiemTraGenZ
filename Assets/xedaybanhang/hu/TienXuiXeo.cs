using UnityEngine;
using System.Collections;

public class TienXuiXeo : MonoBehaviour
{
    [Header("=== HIỆU ỨNG TÂM LINH ===")]
    public AudioSource audioMa;
    public AudioClip tiengTreConCuoi; // Kéo file mp3 tiếng quạ/tiếng cười vào đây
    public GameObject manHinhDo;      // Ảnh UI màu đỏ đục trên Canvas

    [Header("=== CÀI ĐẶT TRỪ TIỀN ===")]
    public int tienPhat = 50000;      // Tham thì thâm 50k

    private bool isPlayerNear = false;
    private bool daNhat = false;

    void Start()
    {
        // Mặc định giấu cái màn hình dọa ma đi
        if (manHinhDo != null) manHinhDo.SetActive(false);
    }

    void Update()
    {
        // Nhấn phím E khi đứng gần tờ tiền
        if (isPlayerNear && Input.GetKeyDown(KeyCode.E) && !daNhat)
        {
            StartCoroutine(BapBayCoHon());
        }
    }

    IEnumerator BapBayCoHon()
    {
        daNhat = true;

        // 1. Giấu tờ tiền ngay lập tức (làm như đã nhặt)
        if (GetComponent<MeshRenderer>() != null)
            GetComponent<MeshRenderer>().enabled = false;

        // 2. NỔ JUMPSCARE! (Bật tiếng + Nháy màn hình đỏ)
        // 2. NỔ JUMPSCARE! (Bật tiếng + Nháy màn hình đỏ)
        if (audioMa != null)
        {
            // Phát cái tiếng Flashbang cài sẵn trong Audio Source
            audioMa.Play();

            // Phát đè thêm tiếng trẻ con cười cùng lúc luôn cho rợn!
            if (tiengTreConCuoi != null) audioMa.PlayOneShot(tiengTreConCuoi);
        }

        if (manHinhDo != null)
        {
            manHinhDo.SetActive(true);
        }

        // 3. TRỪ TIỀN BÊN KHO TỔNG (Vừa đau tim vừa xót ví)
        if (QuanLyKho.Instance != null)
        {
            QuanLyKho.Instance.TienHienCo -= tienPhat;
            QuanLyKho.Instance.SaveGame();

            // Gọi ké hiệu ứng chữ trừ tiền bay lên của sếp (Nếu có)
            if (EffectManager.Instance != null)
            {
                EffectManager.Instance.HienThiTien(-tienPhat);
            }
            Debug.Log("👻 Tới công chuyện! Nhặt tiền cô hồn bị trừ " + tienPhat + "đ");
        }

        // 4. Đợi 0.2 giây (nháy chớp nhoáng) rồi tắt màn hình đỏ
        yield return new WaitForSeconds(2.0f);
        if (manHinhDo != null) manHinhDo.SetActive(false);

        // 5. Chờ âm thanh rùng rợn chạy hết rồi hủy luôn tờ tiền
        yield return new WaitForSeconds(3f);
        Destroy(gameObject);
    }

    // Nhận diện người chơi lại gần (Giống con Drone)
    void OnTriggerEnter(Collider other) { if (other.CompareTag("Player")) isPlayerNear = true; }
    void OnTriggerExit(Collider other) { if (other.CompareTag("Player")) isPlayerNear = false; }
}