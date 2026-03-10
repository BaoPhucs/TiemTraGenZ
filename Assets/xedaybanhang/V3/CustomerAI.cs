using UnityEngine;
using UnityEngine.AI;
using TMPro;
using UnityEngine.UI;

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

    private NavMeshAgent agent;
    private SeatPoint targetSeat;
    private bool isPlayerNear = false;

    public GameObject lyRongPrefab;
    public Animator anim;

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        currentPatience = patienceMax;
        if (chatBubble != null) chatBubble.SetActive(false);
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
        DiLangThangRoiBienMat();
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
        DiLangThangRoiBienMat();
    }

    void DiLangThangRoiBienMat()
    {
        Vector3 randomDest = transform.position + new Vector3(Random.Range(-10f, 10f), 0, Random.Range(-10f, 10f));
        if (NavMesh.SamplePosition(randomDest, out NavMeshHit hit, 10f, NavMesh.AllAreas))
        {
            agent.SetDestination(hit.position);
        }
        Destroy(gameObject, 2.5f);
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