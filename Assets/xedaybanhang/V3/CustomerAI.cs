using UnityEngine;
using UnityEngine.AI;
using TMPro;
using UnityEngine.UI;

public class CustomerAI : MonoBehaviour
{
    public enum CustomerState
    {
        Walking, MovingToSeat, Ordering, Waiting, Drinking, Leaving
    }

    [Header("Thông số AI")]
    public CustomerState currentState = CustomerState.Walking;
    public float patienceMax = 40f;
    private float currentPatience;

    [Header("Đơn hàng (Order)")]
    public string orderMon = "";
    public string[] menu = { "TraTac", "TraDa", "TraChanh" };

    [Header("Giao diện UI")]
    public GameObject chatBubble;
    public TextMeshProUGUI txtOrder;
    public Image imgPatience;

    private NavMeshAgent agent;
    private SeatPoint targetSeat;
    private bool isPlayerNear = false;

    [Header("Rác (Ly rỗng)")]
    public GameObject lyRongPrefab;

    [Header("Hoạt ảnh (Animation)")]
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
                if (agent.remainingDistance <= 0.2f && !agent.pathPending)
                {
                    SitDownAndOrder();
                }
                break;

            case CustomerState.Waiting:
                currentPatience -= Time.deltaTime;

                if (imgPatience != null)
                {
                    imgPatience.fillAmount = currentPatience / patienceMax;
                }

                if (currentPatience <= 0)
                {
                    GetMadAndLeave();
                }

                if (isPlayerNear && Input.GetKeyDown(KeyCode.E))
                {
                    if (PlayerHand.Instance != null && PlayerHand.Instance.monDangCam != "")
                    {
                        ReceiveDrink(PlayerHand.Instance.monDangCam);
                        PlayerHand.Instance.monDangCam = "";
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

                // --- KÍCH HOẠT ANIMATION ĐI BỘ ---
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

        // --- KÍCH HOẠT ANIMATION NGỒI & UỐNG ---
        if (anim != null)
        {
            anim.SetBool("isWalking", false);
            anim.SetBool("isSitting", true);
        }

        orderMon = menu[Random.Range(0, menu.Length)];

        if (chatBubble != null) chatBubble.SetActive(true);
        if (txtOrder != null) txtOrder.text = "Cho 1\n" + orderMon;
    }

    public void ReceiveDrink(string monPhaChe)
    {
        if (currentState != CustomerState.Waiting) return;

        if (monPhaChe == orderMon)
        {
            Debug.Log($"<color=green>🎉 Khách: Ngon quá! Trả tiền nè! (Đã nhận đúng món {monPhaChe})</color>");

            if (chatBubble != null) chatBubble.SetActive(false);
            currentState = CustomerState.Drinking;

            if (QuanLyKho.Instance != null)
            {
                QuanLyKho.Instance.NhanTienBanNuoc(15000);
            }

            Invoke("LeaveHappily", 3f);
        }
        else
        {
            Debug.Log($"<color=red>😡 Khách: Pha sai rồi! Trả {orderMon} mà đưa {monPhaChe} à? Bo xì!</color>");
            GetMadAndLeave();
        }
    }

    void GetMadAndLeave()
    {
        if (chatBubble != null) chatBubble.SetActive(false);
        currentState = CustomerState.Leaving;

        if (targetSeat != null) targetSeat.isOccupied = false;

        agent.enabled = true;

        // --- KÍCH HOẠT ANIMATION ĐỨNG DẬY BỎ ĐI ---
        if (anim != null)
        {
            anim.SetBool("isSitting", false);
            anim.SetBool("isWalking", true);
        }

        Destroy(gameObject, 1f);
        Debug.Log("📉 Khách giận! Đã bị trừ điểm Viral/Uy tín quán!");
    }

    void LeaveHappily()
    {
        Debug.Log("<color=yellow>👋 Khách: Uống xong rồi, đi về đây!</color>");

        if (lyRongPrefab != null && targetSeat != null)
        {
            GameObject rac = Instantiate(lyRongPrefab, targetSeat.sitPosition.position, targetSeat.sitPosition.rotation);

            DonRac scriptDonRac = rac.GetComponentInChildren<DonRac>();
            if (scriptDonRac != null)
            {
                scriptDonRac.gheDangNgoi = targetSeat;
                Debug.Log("Đã truyền dữ liệu ghế cho rác thành công!");
            }
            else
            {
                Debug.LogError("Cảnh báo: Không tìm thấy script DonRac trên Prefab Ly rỗng!");
            }
        }

        // --- KÍCH HOẠT ANIMATION ĐỨNG DẬY ĐI VỀ ---
        if (anim != null)
        {
            anim.SetBool("isSitting", false);
            anim.SetBool("isWalking", true);
        }

        // Kéo dài thời gian bốc hơi để khách kịp diễn animation đứng dậy
        Destroy(gameObject, 1.5f);
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player")) isPlayerNear = true;
    }

    void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player")) isPlayerNear = false;
    }
}