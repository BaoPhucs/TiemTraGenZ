using UnityEngine;
using TMPro;
using System.Collections; // Cần thiết để dùng Coroutine nếu cần sau này

public class DroneGiaoHang : MonoBehaviour
{
    public enum DroneState { NghiNgoi, ChoGiaoNuoc, DangBayDi, DangBayVe }

    [Header("=== Trạng thái Drone ===")]
    public DroneState currentState = DroneState.NghiNgoi;
    public string donHangHienTai = "";

    [Header("=== Cài đặt Đơn hàng App ===")]
    public float thoiGianChoDonMoi = 20f;
    private float demNguocDon;

    [Header("=== Giao diện UI (Gắn trên Drone) ===")]
    public GameObject bangThongBao;
    public TextMeshProUGUI txtDonHang;

    [Header("=== Tọa độ bay ===")]
    public Transform diemDap;
    public Transform diemBayDi;
    public float tocDoBay = 5f;
    // --- CẢI TIẾN 3: Tốc độ xoay ---
    public float tocDoXoay = 5f;

    [Header("=== Hiệu ứng Visual/Audio (MỚI THÊM) ===")]
    // --- CẢI TIẾN 1: Kéo cái ly nước/hộp hàng con dưới bụng Drone vào đây ---
    public GameObject moHinhLyNuocTrenDrone;
    // --- CẢI TIẾN 2: Gắn 1 AudioSource vào Drone, kéo tiếng TingTing và tiếng Bay vào ---
    public AudioSource audioSource;
    public AudioClip soundTingTing;
    public AudioClip soundLoopFlying;

    private bool isPlayerNear = false;

    void Start()
    {
        demNguocDon = thoiGianChoDonMoi;
        if (bangThongBao != null) bangThongBao.SetActive(false);
        transform.position = diemDap.position; // Đậu sẵn ở trạm

        // --- CẢI TIẾN 1: Mới vào game, Drone không chở gì ---
        if (moHinhLyNuocTrenDrone != null) moHinhLyNuocTrenDrone.SetActive(false);
    }

    // ========================================================
    // --- CẢI TIẾN 4: THÊM HÀM UPDATE ĐỂ DI CHUYỂN + XOAY + NHẬN INPUT ---
    // ========================================================
    void Update()
    {
        switch (currentState)
        {
            case DroneState.NghiNgoi:
                demNguocDon -= Time.deltaTime;
                if (demNguocDon <= 0) NhanDonHangMoi();
                break;

            case DroneState.ChoGiaoNuoc:
                // --- CẢI TIẾN 4: Kiểm tra người chơi lại gần bấm phím E để giao nước ---
                if (isPlayerNear && Input.GetKeyDown(KeyCode.E))
                {
                    if (PlayerHand.Instance != null && PlayerHand.Instance.monDangCam != "")
                    {
                        GiaoNuocChoDrone(PlayerHand.Instance.monDangCam, PlayerHand.Instance.isPerfectDrink);
                    }
                }
                break;

            case DroneState.DangBayDi:
                // 1. Di chuyển tới điểm bay đi
                DiChuyenToiMucTieu(diemBayDi.position);

                // 2. Kiểm tra đã đến nơi chưa
                if (Vector3.Distance(transform.position, diemBayDi.position) < 0.2f)
                {
                    HoanThanhDon();
                }
                break;

            case DroneState.DangBayVe:
                // 1. Di chuyển về bãi đáp
                DiChuyenToiMucTieu(diemDap.position);

                // 2. Kiểm tra đã đến nơi chưa
                if (Vector3.Distance(transform.position, diemDap.position) < 0.2f)
                {
                    currentState = DroneState.NghiNgoi;
                    demNguocDon = thoiGianChoDonMoi;
                    // --- CẢI TIẾN 2: Tắt tiếng bay rè rè ---
                    if (audioSource != null) audioSource.Stop();
                }
                break;
        }
    }

    // Hàm phụ trợ để xử lý Di chuyển + Xoay (CẢI TIẾN 3)
    void DiChuyenToiMucTieu(Vector3 viTriMucTieu)
    {
        // 1. Tịnh tiến bay đi
        transform.position = Vector3.MoveTowards(transform.position, viTriMucTieu, tocDoBay * Time.deltaTime);

        // 2. Xoay mặt (Đã sửa lỗi đứng hình)
        Vector3 direction = (viTriMucTieu - transform.position).normalized;
        direction.y = 0; // Ép trục Y = 0 để Drone không chúi nhủi

        // KIỂM TRA: Nếu vector hướng đi đủ lớn (không phải bay thẳng đứng 90 độ) thì mới xoay
        if (direction.sqrMagnitude > 0.001f)
        {
            Quaternion lookRotation = Quaternion.LookRotation(direction);
            transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, tocDoXoay * Time.deltaTime);
        }
    }

    // --- CODE CỦA BẠN (ẢNH 1 + ẢNH 2) CÓ SỬA LỖI orderMon -> donHangHienTai ---
    void NhanDonHangMoi()
    {
        currentState = DroneState.ChoGiaoNuoc;

        // Random 1 món trong Menu đã mở khóa
        if (QuanLyKho.Instance != null)
        {
            var menuHienTai = QuanLyKho.Instance.LayMenuHienTai();
            donHangHienTai = menuHienTai[Random.Range(0, menuHienTai.Count)];
        }
        else donHangHienTai = "TraDa";

        if (bangThongBao != null)
        {
            bangThongBao.SetActive(true);
            txtDonHang.text = "<color=#00FF00>[App GrapTrà]</color>\nGiao đi:\n" + donHangHienTai;
        }

        // --- CẢI TIẾN 2: Kêu Ting Ting báo đơn hàng mới ---
        if (audioSource != null && soundTingTing != null) audioSource.PlayOneShot(soundTingTing);

        Debug.Log("🚁 TING TING! Có đơn online mới: " + donHangHienTai);
    }

    void GiaoNuocChoDrone(string monPhaChe, bool isPerfect)
    {
        if (monPhaChe == donHangHienTai)
        {
            // 1. Nhận nước thành công
            PlayerHand.Instance.monDangCam = "";
            PlayerHand.Instance.isPerfectDrink = false;

            if (bangThongBao != null) bangThongBao.SetActive(false);
            currentState = DroneState.DangBayDi;

            // --- CẢI TIẾN 1: Hiện mô hình ly nước dưới bụng Drone ---
            if (moHinhLyNuocTrenDrone != null) moHinhLyNuocTrenDrone.SetActive(true);

            // --- CẢI TIẾN 2: Bật tiếng quạt bay rè rè (Loop) ---
            if (audioSource != null && soundLoopFlying != null)
            {
                audioSource.clip = soundLoopFlying;
                audioSource.loop = true;
                audioSource.Play();
            }

            // 2. Tính tiền (Tiền gốc + 15k phí Ship Online)
            if (QuanLyKho.Instance != null)
            {
                int tienCoBan = 10000;
                // === LỖI SỐNG CÒN: Copy-paste bị nhầm tên biến orderMon, đã sửa thành donHangHienTai ===
                switch (donHangHienTai)
                {
                    case "TraDa": tienCoBan = 10000; break;
                    case "CaPheDen": tienCoBan = 20000; break;
                    case "TraTac": tienCoBan = 25000; break;
                    case "CaPheSua": tienCoBan = 30000; break;
                    case "TraChanh": tienCoBan = 45000; break;
                    case "TraSua": tienCoBan = 60000; break;
                    case "MatchaLatte": tienCoBan = 85000; break;
                }

                // Trả tiền liền tay + Tiền Ship
                QuanLyKho.Instance.NhanTienBanNuoc(tienCoBan + 15000);
                QuanLyKho.Instance.DiemViral += 10; // App đánh giá 5 sao
            }
            Debug.Log("🚁 Drone cất cánh đi giao hàng: " + donHangHienTai);
        }
        else
        {
            Debug.Log("❌ Bỏ nhầm món rồi! Drone không nhận! Cần " + donHangHienTai + " nhưng sếp đưa " + monPhaChe);
        }
    }

    // --- CODE CỦA BẠN (ẢNH 3) ---
    void HoanThanhDon()
    {
        currentState = DroneState.DangBayVe;

        // --- CẢI TIẾN 1: Ẩn mô hình ly nước khi giao xong (Drone trống rỗng) ---
        if (moHinhLyNuocTrenDrone != null) moHinhLyNuocTrenDrone.SetActive(false);

        Debug.Log("🚁 Đã giao xong tới tay khách, Drone đang quay về trạm!");
    }

    void OnTriggerEnter(Collider other) { if (other.CompareTag("Player")) isPlayerNear = true; }
    void OnTriggerExit(Collider other) { if (other.CompareTag("Player")) isPlayerNear = false; }
}