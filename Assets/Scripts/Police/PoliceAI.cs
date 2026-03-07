using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;
using System.Collections.Generic;

public class PoliceAI : MonoBehaviour
{
    public static List<PoliceAI> danhSachCongAn = new List<PoliceAI>();

    public enum PoliceState { Patrol, CheckCart, FindPlayer, Talk, Chase, Search }
    public PoliceState currentState = PoliceState.Patrol;

    [Header("--- CÀI ĐẶT TẦM NHÌN & PHẠT ---")]
    public float tocDoDiTuan = 3.5f;
    public float tocDoRuotDuoi = 8.0f;
    public float khoangCachPhatHienXe = 15f;
    public float khoangCachBat = 3.0f;
    public float khoangCachMatDau = 20f;

    [Header("--- HỆ THỐNG TRUY NÃ ---")]
    public static bool dangTruyNa = false;
    public float mucDoCanhBao = 0f;
    public float thoiGianAnToanCaiDat = 3.0f;
    public float thoiGianTimKiem = 7.0f;

    private float thoiGianAnToanHT = 0f;
    private float thoiGianBoCuocHT = 0f;

    private Transform xeDayBiPhatHien;
    private float thoiGianKhamXe = 2.0f;
    private float thoiGianChongKet = 10.0f;

    [Header("--- THAM CHIẾU ---")]
    public Transform player;
    public Animator anim;
    private NavMeshAgent agent;
    private Vector3 diemXuatPhat;

    [Header("--- GIAO DIỆN UI ---")]
    public Image imgBaoDongDau;
    public Image imgBaoDongUI_Main;
    public GameObject panelHoiThoai;
    public Button btnNopPhat;
    public Button btnBoChay;
    public GameObject videoBadEnding;

    [Header("--- ÂM THANH ---")]
    public AudioSource audioSource;
    public AudioClip nhacPhatHienVaKiemTra;
    public AudioClip nhacTruyDuoi;

    void Awake() { danhSachCongAn.Add(this); }
    void OnDestroy() { danhSachCongAn.Remove(this); }

    void Start()
    {
        Time.timeScale = 1f;
        dangTruyNa = false;
        thoiGianAnToanHT = 0f;
        agent = GetComponent<NavMeshAgent>();
        diemXuatPhat = transform.position;

        if (btnNopPhat != null) btnNopPhat.onClick.AddListener(ChonNopPhat);
        if (btnBoChay != null) btnBoChay.onClick.AddListener(ChonBoChay);
        if (videoBadEnding != null) videoBadEnding.SetActive(false);

        if (imgBaoDongUI_Main != null) imgBaoDongUI_Main.gameObject.SetActive(false);

        HienChuot(false);
        currentState = PoliceState.Patrol;
        TimDiemTuanTraMoi();
    }

    void Update()
    {
        if (player == null || Time.timeScale == 0f) return;

        if (thoiGianAnToanHT > 0f) thoiGianAnToanHT -= Time.deltaTime;

        if (currentState == PoliceState.Talk)
        {
            SetAnimation(0);
            agent.isStopped = true;
            agent.velocity = Vector3.zero;
            HienChuot(true);

            if (Input.GetKeyDown(KeyCode.A)) ChonNopPhat();
            if (Input.GetKeyDown(KeyCode.D)) ChonBoChay();
            return;
        }

        float khoangCachVoiMinh = Vector3.Distance(transform.position, player.position);

        if (!dangTruyNa && currentState == PoliceState.Patrol && KiemTraThayXeDay())
        {
            currentState = PoliceState.CheckCart;
            thoiGianKhamXe = 2.0f;
            thoiGianChongKet = 10.0f;
        }

        switch (currentState)
        {
            case PoliceState.Patrol:
                StopBGM();
                agent.isStopped = false;
                agent.speed = tocDoDiTuan;
                SetAnimation(1);
                if (!agent.pathPending && agent.remainingDistance < 0.5f) TimDiemTuanTraMoi();
                mucDoCanhBao = Mathf.MoveTowards(mucDoCanhBao, 0, Time.deltaTime * 10f);
                break;

            case PoliceState.CheckCart:
                PlayBGM(nhacPhatHienVaKiemTra);
                mucDoCanhBao = 30f;
                if (xeDayBiPhatHien != null)
                {
                    bool daDenSatXe = !agent.pathPending && agent.remainingDistance <= 1.5f;

                    if (daDenSatXe)
                    {
                        agent.isStopped = true;
                        agent.velocity = Vector3.zero;
                        SetAnimation(0);

                        Vector3 huongNhinXe = new Vector3(xeDayBiPhatHien.position.x, transform.position.y, xeDayBiPhatHien.position.z);
                        transform.LookAt(huongNhinXe);

                        thoiGianKhamXe -= Time.deltaTime;
                        if (thoiGianKhamXe <= 0f)
                        {
                            currentState = PoliceState.FindPlayer;
                        }
                    }
                    else
                    {
                        agent.isStopped = false;
                        agent.speed = tocDoDiTuan;
                        SetAnimation(1);
                        agent.SetDestination(xeDayBiPhatHien.position);

                        thoiGianChongKet -= Time.deltaTime;
                        if (thoiGianChongKet <= 0f)
                        {
                            currentState = PoliceState.Patrol;
                        }
                    }
                }
                else currentState = PoliceState.Patrol;
                break;

            case PoliceState.FindPlayer:
                PlayBGM(nhacPhatHienVaKiemTra);
                agent.isStopped = false;
                agent.speed = tocDoDiTuan;
                SetAnimation(1);
                agent.SetDestination(player.position);
                mucDoCanhBao = 60f;

                if (khoangCachVoiMinh <= khoangCachBat && thoiGianAnToanHT <= 0f) HienThiHoiThoai();
                break;

            case PoliceState.Chase:
                PlayBGM(nhacTruyDuoi);
                agent.isStopped = false;
                agent.speed = tocDoRuotDuoi;
                SetAnimation(2);
                agent.SetDestination(player.position);
                mucDoCanhBao = 100f;

                if (khoangCachVoiMinh > khoangCachMatDau)
                {
                    currentState = PoliceState.Search;
                    thoiGianBoCuocHT = thoiGianTimKiem;
                    TimDiemTimKiemLoan();
                }
                else if (khoangCachVoiMinh <= khoangCachBat && thoiGianAnToanHT <= 0f)
                {
                    BatDuocMinh_GameOver();
                }
                break;

            case PoliceState.Search:
                PlayBGM(nhacTruyDuoi);
                agent.isStopped = false;
                agent.speed = tocDoRuotDuoi;
                SetAnimation(2);
                mucDoCanhBao = 70f;
                thoiGianBoCuocHT -= Time.deltaTime;

                if (!agent.pathPending && agent.remainingDistance < 0.5f) TimDiemTimKiemLoan();
                if (khoangCachVoiMinh <= khoangCachMatDau) currentState = PoliceState.Chase;
                if (thoiGianBoCuocHT <= 0f) KetThucNgay_AnToan();
                break;
        }

        CapNhatMauUI();
    }

    bool KiemTraThayXeDay()
    {
        if (agent == null || !agent.isOnNavMesh) return false;

        RaycastHit hit;
        if (Physics.SphereCast(transform.position + Vector3.up, 3f, transform.forward, out hit, khoangCachPhatHienXe))
        {
            if (hit.transform.CompareTag("XeDay"))
            {
                NavMeshPath path = new NavMeshPath();
                agent.CalculatePath(hit.transform.position, path);

                if (path.status == NavMeshPathStatus.PathComplete)
                {
                    xeDayBiPhatHien = hit.transform;
                    return true;
                }
            }
        }
        return false;
    }

    void HienThiHoiThoai()
    {
        currentState = PoliceState.Talk;
        agent.isStopped = true;
        if (panelHoiThoai != null) panelHoiThoai.SetActive(true);
        HienChuot(true);
    }

    public void ChonNopPhat()
    {
        if (panelHoiThoai != null) panelHoiThoai.SetActive(false);
        HienChuot(false);
        KetThucNgay_AnToan();
    }

    public void ChonBoChay()
    {
        if (panelHoiThoai != null) panelHoiThoai.SetActive(false);
        HienChuot(false);
        dangTruyNa = true;

        if (player != null)
        {
            player.SendMessage("SetMovementBlocked", false, SendMessageOptions.DontRequireReceiver);
            player.SendMessage("UnblockMovement", SendMessageOptions.DontRequireReceiver);
        }

        foreach (var ca in danhSachCongAn)
        {
            if (ca != null && ca.gameObject.activeInHierarchy)
            {
                ca.thoiGianAnToanHT = thoiGianAnToanCaiDat;
                if (Vector3.Distance(ca.transform.position, player.position) <= ca.khoangCachMatDau)
                    ca.currentState = PoliceState.Chase;
                else
                {
                    ca.currentState = PoliceState.Search;
                    ca.thoiGianBoCuocHT = ca.thoiGianTimKiem;
                }
                ca.agent.isStopped = false;
                ca.agent.ResetPath();
            }
        }
    }

    void BatDuocMinh_GameOver()
    {
        StopBGM();
        agent.isStopped = true;
        Time.timeScale = 0f;
        HienChuot(true);
        if (videoBadEnding != null) videoBadEnding.SetActive(true);
    }

    void KetThucNgay_AnToan()
    {
        dangTruyNa = false;
        foreach (var ca in danhSachCongAn)
        {
            ca.mucDoCanhBao = 0f;
            ca.currentState = PoliceState.Patrol;
            ca.gameObject.SetActive(false);
        }
        if (imgBaoDongUI_Main != null) imgBaoDongUI_Main.color = Color.green;
    }

    void TimDiemTuanTraMoi()
    {
        if (agent == null || !agent.isOnNavMesh) return;
        Vector3 randomDest = diemXuatPhat + Random.insideUnitSphere * 20f;
        if (NavMesh.SamplePosition(randomDest, out NavMeshHit hit, 20f, NavMesh.AllAreas)) agent.SetDestination(hit.position);
    }

    void TimDiemTimKiemLoan()
    {
        if (agent == null || !agent.isOnNavMesh) return;
        Vector3 randomDest = transform.position + Random.insideUnitSphere * 15f;
        if (NavMesh.SamplePosition(randomDest, out NavMeshHit hit, 15f, NavMesh.AllAreas)) agent.SetDestination(hit.position);
    }

    void CapNhatMauUI()
    {
        Color mau;
        if (dangTruyNa || currentState == PoliceState.Chase || currentState == PoliceState.Search) mau = Color.red;
        else if (mucDoCanhBao >= 50) mau = new Color(1, 0.5f, 0);
        else if (mucDoCanhBao >= 20) mau = Color.yellow;
        else mau = Color.green;

        if (imgBaoDongDau != null)
        {
            imgBaoDongDau.enabled = true;
            imgBaoDongDau.color = mau;
        }

        if (imgBaoDongUI_Main != null)
        {
            if (mucDoCanhBao > 0 || dangTruyNa || currentState != PoliceState.Patrol)
            {
                imgBaoDongUI_Main.gameObject.SetActive(true);
            }
            else if (mucDoCanhBao == 0 && currentState == PoliceState.Patrol)
            {
                imgBaoDongUI_Main.gameObject.SetActive(false);
            }

            float maxBao = 0;
            foreach (var ca in danhSachCongAn)
                if (ca.gameObject.activeInHierarchy) maxBao = Mathf.Max(maxBao, ca.mucDoCanhBao);

            if (dangTruyNa) imgBaoDongUI_Main.color = Color.red;
            else if (maxBao >= 50) imgBaoDongUI_Main.color = new Color(1, 0.5f, 0);
            else if (maxBao >= 20) imgBaoDongUI_Main.color = Color.yellow;
            else imgBaoDongUI_Main.color = Color.green;
        }
    }

    // --- HÀM ÂM THANH ĐÃ ĐƯỢC FIX LỖI GIẬT FRAME ---
    void PlayBGM(AudioClip clip)
    {
        if (audioSource == null || clip == null) return;

        // Chỉ gán và bật nhạc nếu nhạc truyền vào KHÁC với nhạc đang phát
        if (audioSource.clip != clip)
        {
            audioSource.clip = clip;
            audioSource.Play();
        }
        // Hoặc nếu lỡ bị tắt giữa chừng thì bật lại
        else if (!audioSource.isPlaying)
        {
            audioSource.Play();
        }
    }

    void StopBGM()
    {
        if (audioSource != null)
        {
            audioSource.Stop();
            // CỰC KỲ QUAN TRỌNG: Phải ép về null để hàm PlayBGM biết đường bật lại khi cần
            audioSource.clip = null;
        }
    }

    void HienChuot(bool hien) { Cursor.visible = hien; Cursor.lockState = hien ? CursorLockMode.None : CursorLockMode.Locked; }
    void SetAnimation(int i) { if (anim != null) anim.SetInteger("AnimState", i); }
}