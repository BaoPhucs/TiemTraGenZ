using UnityEngine;
using UnityEngine.AI;
using TMPro;
using UnityEngine.UI;
using System.Collections.Generic;

[System.Serializable]
public struct OrderVoice
{
    public string tenMon;
    public AudioClip fileAmThanh;
}

public class CustomerAI : MonoBehaviour
{
    public enum CustomerState { Walking, MovingToSeat, Ordering, Waiting, Drinking, Leaving }

    [Header("Thông số AI")]
    public CustomerState currentState = CustomerState.Walking;
    public float patienceMax = 60f;
    private float currentPatience;

    public string orderMon = "";

    [Header("Giao diện UI")]
    public GameObject chatBubble;
    public TextMeshProUGUI txtOrder;
    public Image imgPatience;

    [Header("Âm thanh Order")]
    public AudioSource mouthAudioSource;
    public List<OrderVoice> danhSachGiongNoi;

    private NavMeshAgent agent;
    private SeatPoint targetSeat;
    private bool isPlayerNear = false;

    // --- THÊM MỚI: Biến lưu trữ quê hương bản quán của AI ---
    private Vector3 diemXuatPhat;

    public GameObject lyRongPrefab;
    public Animator anim;

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();

        // GHI NHỚ NGAY VỊ TRÍ LÚC VỪA ĐƯỢC SINH RA
        diemXuatPhat = transform.position;

        currentPatience = patienceMax;
        if (chatBubble != null) chatBubble.SetActive(false);

        if (mouthAudioSource == null) mouthAudioSource = GetComponent<AudioSource>();

        InvokeRepeating("CheckForEmptySeat", 1f, 2f);
    }

    void Update()
    {
        switch (currentState)
        {
            case CustomerState.MovingToSeat:
                if (targetSeat == null)
                {
                    if (QuanLyKho.Instance != null) { QuanLyKho.Instance.DiemViral -= 2; QuanLyKho.Instance.SaveGame(); }
                    GetMadAndLeave();
                    return;
                }
                if (agent.remainingDistance <= 0.2f && !agent.pathPending) SitDownAndOrder();
                break;

            case CustomerState.Waiting:
                currentPatience -= Time.deltaTime;
                if (imgPatience != null) imgPatience.fillAmount = currentPatience / patienceMax;

                if (currentPatience <= 0)
                {
                    if (QuanLyKho.Instance != null) { QuanLyKho.Instance.DiemViral -= 5; QuanLyKho.Instance.SaveGame(); }
                    GetMadAndLeave();
                }

                if (isPlayerNear && Input.GetKeyDown(KeyCode.E))
                {
                    if (PlayerHand.Instance != null && PlayerHand.Instance.monDangCam != "")
                    {
                        ReceiveDrink(PlayerHand.Instance.monDangCam, PlayerHand.Instance.isPerfectDrink);
                        PlayerHand.Instance.monDangCam = "";
                        PlayerHand.Instance.isPerfectDrink = false;
                    }
                }
                break;

            // --- THÊM MỚI: THEO DÕI LÚC KHÁCH ĐANG ĐI VỀ ---
            case CustomerState.Leaving:
                // Nếu khách đã lết về gần tới điểm xuất phát (cách 0.5 mét) thì mới biến mất
                if (!agent.pathPending && agent.remainingDistance <= 0.5f)
                {
                    Destroy(gameObject);
                }
                break;
        }
    }

    void CheckForEmptySeat()
    {
        if (currentState != CustomerState.Walking) return;
        GameObject[] danhSachGhe = GameObject.FindGameObjectsWithTag("GheDaLay");
        foreach (GameObject gheObj in danhSachGhe)
        {
            SeatPoint ghe = gheObj.GetComponent<SeatPoint>();
            if (ghe != null && !ghe.isOccupied)
            {
                ghe.isOccupied = true;
                targetSeat = ghe;
                currentState = CustomerState.MovingToSeat;
                if (anim != null) anim.SetBool("isWalking", true);
                agent.SetDestination(ghe.sitPosition.position);
                CancelInvoke("CheckForEmptySeat");
                return;
            }
        }
    }

    void SitDownAndOrder()
    {
        currentState = CustomerState.Waiting;
        transform.position = targetSeat.sitPosition.position;
        transform.rotation = targetSeat.sitPosition.rotation;
        agent.enabled = false;

        if (anim != null) { anim.SetBool("isWalking", false); anim.SetBool("isSitting", true); }

        if (QuanLyKho.Instance != null)
        {
            var menuHienTai = QuanLyKho.Instance.LayMenuHienTai();
            orderMon = menuHienTai[Random.Range(0, menuHienTai.Count)];
        }
        else orderMon = "TraDa";

        if (chatBubble != null) chatBubble.SetActive(true);
        if (txtOrder != null) txtOrder.text = "Cho 1\n" + orderMon;

        if (mouthAudioSource != null && danhSachGiongNoi != null)
        {
            foreach (OrderVoice ov in danhSachGiongNoi)
            {
                if (ov.tenMon == orderMon && ov.fileAmThanh != null)
                {
                    mouthAudioSource.clip = ov.fileAmThanh;
                    mouthAudioSource.Play();
                    break;
                }
            }
        }
    }

    public void ReceiveDrink(string monPhaChe, bool isPerfect)
    {
        if (currentState != CustomerState.Waiting) return;

        if (monPhaChe == orderMon)
        {
            if (chatBubble != null) chatBubble.SetActive(false);
            currentState = CustomerState.Drinking;

            if (QuanLyKho.Instance != null)
            {
                int tienCoBan = 10000;
                int viralCoBan = 5;

                switch (orderMon)
                {
                    case "TraDa": tienCoBan = 10000; viralCoBan = 5; break;
                    case "CaPheDen": tienCoBan = 20000; viralCoBan = 8; break;
                    case "TraTac": tienCoBan = 25000; viralCoBan = 10; break;
                    case "CaPheSua": tienCoBan = 30000; viralCoBan = 12; break;
                    case "TraChanh": tienCoBan = 45000; viralCoBan = 15; break;
                    case "TraSua": tienCoBan = 60000; viralCoBan = 20; break;
                    case "MatchaLatte": tienCoBan = 85000; viralCoBan = 30; break;
                }

                if (isPerfect)
                {
                    QuanLyKho.Instance.DiemViral += viralCoBan * 2;
                    QuanLyKho.Instance.NhanTienBanNuoc(Mathf.RoundToInt(tienCoBan * 1.5f));
                }
                else
                {
                    QuanLyKho.Instance.DiemViral -= 2;
                    QuanLyKho.Instance.NhanTienBanNuoc(tienCoBan);
                }
            }
            Invoke("LeaveHappily", 3f);
        }
        else
        {
            if (QuanLyKho.Instance != null) { QuanLyKho.Instance.DiemViral -= 15; QuanLyKho.Instance.SaveGame(); }
            GetMadAndLeave();
        }
    }

    void GetMadAndLeave()
    {
        if (chatBubble != null) chatBubble.SetActive(false);
        currentState = CustomerState.Leaving;
        if (targetSeat != null) targetSeat.isOccupied = false;
        agent.enabled = true;
        if (anim != null) { anim.SetBool("isSitting", false); anim.SetBool("isWalking", true); }

        QuayVeDiemXuatPhat(); // THAY ĐỔI TÊN HÀM
    }

    void LeaveHappily()
    {
        currentState = CustomerState.Leaving;
        bool coDeLaiRac = false;

        if (lyRongPrefab != null && targetSeat != null)
        {
            GameObject rac = Instantiate(lyRongPrefab, targetSeat.sitPosition.position, targetSeat.sitPosition.rotation);
            DonRac scriptDonRac = rac.GetComponent<DonRac>();
            if (scriptDonRac == null) scriptDonRac = rac.GetComponentInChildren<DonRac>();

            if (scriptDonRac != null)
            {
                scriptDonRac.gheDangNgoi = targetSeat;
                coDeLaiRac = true;
            }
        }

        if (!coDeLaiRac && targetSeat != null) targetSeat.isOccupied = false;
        agent.enabled = true;
        if (anim != null) { anim.SetBool("isSitting", false); anim.SetBool("isWalking", true); }

        QuayVeDiemXuatPhat(); // THAY ĐỔI TÊN HÀM
    }

    // --- THAY VÌ ĐI LANG THANG, BÂY GIỜ ÉP ĐI VỀ NHÀ ---
    void QuayVeDiemXuatPhat()
    {
        // Cài đặt mục tiêu là điểm xuất phát đã lưu ở Start()
        agent.SetDestination(diemXuatPhat);

        // Không dùng Destroy(gameObject, 2.5f) nữa. 
        // Lệnh Destroy đã được chuyển lên hàm Update() để canh chuẩn xác lúc tới nơi.
    }

    void OnDestroy()
    {
        if (targetSeat != null && targetSeat.isOccupied && currentState != CustomerState.Leaving)
        {
            targetSeat.isOccupied = false;
        }
    }

    void OnTriggerEnter(Collider other) { if (other.CompareTag("Player")) isPlayerNear = true; }
    void OnTriggerExit(Collider other) { if (other.CompareTag("Player")) isPlayerNear = false; }
}