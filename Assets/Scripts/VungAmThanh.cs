using UnityEngine;

public class VungAmThanh : MonoBehaviour
{
    public AudioSource mayPhatNhac;
    public string tagNhanVat = "Player";

    private Transform playerTransform;
    private Collider vungVaCham;
    private GameObject hudCanvas; // Dùng giao diện game làm mốc an toàn tuyệt đối

    void Start()
    {
        if (mayPhatNhac == null) mayPhatNhac = GetComponent<AudioSource>();

        // LỚP GIÁP 1: Bóp cổ cái loa ngay từ 1 phần nghìn giây đầu tiên
        if (mayPhatNhac != null) mayPhatNhac.Stop();

        vungVaCham = GetComponent<Collider>();

        GameObject player = GameObject.FindGameObjectWithTag(tagNhanVat);
        if (player != null) playerTransform = player.transform;

        // Tự động tìm giao diện game. 
        // IntroManager sẽ giấu cái này đi, ta dùng nó để biết lúc nào Intro kết thúc
        hudCanvas = GameObject.Find("HUD_Canvas");
    }

    void Update()
    {
        if (mayPhatNhac == null || playerTransform == null || vungVaCham == null) return;

        // LỚP GIÁP 2: KIỂM TRA ĐANG CHƠI HAY ĐANG CHIẾU INTRO/BẢNG KẾT TOÁN
        // Nếu HUD bị ẩn (đang Intro) HOẶC thời gian bị dừng -> dangTrongGame = false
        bool dangTrongGame = (hudCanvas != null && hudCanvas.activeInHierarchy) && (Time.timeScale > 0.1f);

        // LỚP GIÁP 3: TRỊ DỨT ĐIỂM LỖI BOX XOAY (AABB)
        // Lệnh ClosestPoint đo chính xác từng milimet Box Collider bị xoay, không bị phình to
        Vector3 diemGanNhat = vungVaCham.ClosestPoint(playerTransform.position);
        bool dangDungTrongVung = Vector3.Distance(playerTransform.position, diemGanNhat) < 0.1f;

        // QUYẾT ĐỊNH CUỐI CÙNG
        if (dangDungTrongVung && dangTrongGame)
        {
            if (!mayPhatNhac.isPlaying)
            {
                mayPhatNhac.Play();
            }
        }
        else
        {
            // Chỉ cần rớt 1 trong 2 điều kiện (chiếu Intro hoặc bước ra ngoài) là ÉP CÂM NGAY
            if (mayPhatNhac.isPlaying)
            {
                mayPhatNhac.Pause();
            }
        }
    }
}