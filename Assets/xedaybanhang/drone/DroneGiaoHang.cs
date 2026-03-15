using UnityEngine;
using TMPro;

public class DroneGiaoHang : MonoBehaviour
{
    public enum DroneState { NghiNgoi, ChoGiaoNuoc, DangBayDi, DangBayVe }

    [Header("=== Trạng thái Drone ===")]
    public DroneState currentState = DroneState.NghiNgoi;
    public string donHangHienTai = "";

    [Header("=== Mở Khóa Drone (MỚI) ===")]
    public GameObject moHinhDroneGoc; // Kéo toàn bộ mô hình 3D con Drone vào đây
    private bool daBatDongHo = false; // Ngăn nó nổ đơn khi chưa mua

    [Header("=== Cài đặt Đơn hàng App ===")]
    public float thoiGianChoDonMoi = 20f;
    private float demNguocDon;

    [Header("=== Giao diện UI ===")]
    public GameObject bangThongBao;
    public TextMeshProUGUI txtDonHang;

    [Header("=== Tọa độ bay ===")]
    public Transform diemDap;
    public Transform diemBayDi;
    public float tocDoBay = 5f;
    public float tocDoXoay = 5f;

    [Header("=== Hiệu ứng Visual/Audio ===")]
    public GameObject moHinhLyNuocTrenDrone;
    public AudioSource audioSource;
    public AudioClip soundTingTing;
    public AudioClip soundLoopFlying;

    private bool isPlayerNear = false;

    void Start()
    {
        demNguocDon = thoiGianChoDonMoi;
        if (bangThongBao != null) bangThongBao.SetActive(false);
        transform.position = diemDap.position; // Đậu sẵn ở trạm

        // Mới vào game, Drone không chở gì
        if (moHinhLyNuocTrenDrone != null) moHinhLyNuocTrenDrone.SetActive(false);

        // Mới vào game -> Ẩn Drone đi nếu chưa mua
        if (moHinhDroneGoc != null) moHinhDroneGoc.SetActive(false);
    }

    void Update()
    {
        // ========================================================
        // 1. KIỂM TRA ĐÃ MUA DRONE CHƯA?
        // ========================================================
        if (QuanLyKho.Instance == null || !QuanLyKho.Instance.unlockDrone)
        {
            if (moHinhDroneGoc != null && moHinhDroneGoc.activeSelf) moHinhDroneGoc.SetActive(false);
            return; // Chưa mua thì KHÔNG chạy code bên dưới
        }

        // ========================================================
        // 2. NẾU ĐÃ MUA -> HIỆN HÌNH LÊN
        // ========================================================
        if (moHinhDroneGoc != null && !moHinhDroneGoc.activeSelf)
        {
            moHinhDroneGoc.SetActive(true);
            // Vừa mua xong là bắt đầu cho đồng hồ chạy
            if (!daBatDongHo)
            {
                demNguocDon = thoiGianChoDonMoi;
                daBatDongHo = true;
            }
        }

        // ========================================================
        // 3. VẬN HÀNH BÌNH THƯỜNG
        // ========================================================
        switch (currentState)
        {
            case DroneState.NghiNgoi:
                demNguocDon -= Time.deltaTime;
                if (demNguocDon <= 0) NhanDonHangMoi();
                break;

            case DroneState.ChoGiaoNuoc:
                if (isPlayerNear && Input.GetKeyDown(KeyCode.E))
                {
                    if (PlayerHand.Instance != null && PlayerHand.Instance.monDangCam != "")
                    {
                        GiaoNuocChoDrone(PlayerHand.Instance.monDangCam, PlayerHand.Instance.isPerfectDrink);
                    }
                }
                break;

            case DroneState.DangBayDi:
                DiChuyenToiMucTieu(diemBayDi.position);
                if (Vector3.Distance(transform.position, diemBayDi.position) < 0.2f)
                {
                    HoanThanhDon();
                }
                break;

            case DroneState.DangBayVe:
                DiChuyenToiMucTieu(diemDap.position);
                if (Vector3.Distance(transform.position, diemDap.position) < 0.2f)
                {
                    currentState = DroneState.NghiNgoi;
                    demNguocDon = thoiGianChoDonMoi;
                    if (audioSource != null) audioSource.Stop();
                }
                break;
        }
    }

    void DiChuyenToiMucTieu(Vector3 viTriMucTieu)
    {
        // 1. Tịnh tiến bay đi
        transform.position = Vector3.MoveTowards(transform.position, viTriMucTieu, tocDoBay * Time.deltaTime);

        // 2. Xoay mặt
        Vector3 direction = (viTriMucTieu - transform.position).normalized;
        direction.y = 0; // Ép trục Y = 0 để Drone không chúi nhủi

        if (direction.sqrMagnitude > 0.001f)
        {
            Quaternion lookRotation = Quaternion.LookRotation(direction);
            transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, tocDoXoay * Time.deltaTime);
        }
    }

    void NhanDonHangMoi()
    {
        currentState = DroneState.ChoGiaoNuoc;

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

        if (audioSource != null && soundTingTing != null) audioSource.PlayOneShot(soundTingTing);

        Debug.Log("🚁 TING TING! Có đơn online mới: " + donHangHienTai);
    }

    void GiaoNuocChoDrone(string monPhaChe, bool isPerfect)
    {
        if (monPhaChe == donHangHienTai)
        {
            PlayerHand.Instance.monDangCam = "";
            PlayerHand.Instance.isPerfectDrink = false;

            if (bangThongBao != null) bangThongBao.SetActive(false);
            currentState = DroneState.DangBayDi;

            if (moHinhLyNuocTrenDrone != null) moHinhLyNuocTrenDrone.SetActive(true);

            if (audioSource != null && soundLoopFlying != null)
            {
                audioSource.clip = soundLoopFlying;
                audioSource.loop = true;
                audioSource.Play();
            }

            if (QuanLyKho.Instance != null)
            {
                int tienCoBan = 10000;
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
                QuanLyKho.Instance.DiemViral += 10;
            }
            Debug.Log("🚁 Drone cất cánh đi giao hàng: " + donHangHienTai);
        }
        else
        {
            Debug.Log("❌ Bỏ nhầm món rồi! Drone không nhận! Cần " + donHangHienTai + " nhưng sếp đưa " + monPhaChe);
        }
    }

    void HoanThanhDon()
    {
        currentState = DroneState.DangBayVe;
        if (moHinhLyNuocTrenDrone != null) moHinhLyNuocTrenDrone.SetActive(false);
        Debug.Log("🚁 Đã giao xong tới tay khách, Drone đang quay về trạm!");
    }

    void OnTriggerEnter(Collider other) { if (other.CompareTag("Player")) isPlayerNear = true; }
    void OnTriggerExit(Collider other) { if (other.CompareTag("Player")) isPlayerNear = false; }
}