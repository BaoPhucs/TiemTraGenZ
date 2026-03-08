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
    public float patienceMax = 60f;
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
                if (targetSeat == null)
                {
                    Debug.Log("<color=red>😡 Khách: Ủa ghế của tui đâu? Làm ăn kì cục! Bỏ về!</color>");
                    if (QuanLyKho.Instance != null)
                    {
                        QuanLyKho.Instance.DiemViral -= 2;
                        QuanLyKho.Instance.SaveGame();
                    }
                    GetMadAndLeave();
                    return;
                }

                if (agent.remainingDistance <= 0.2f && !agent.pathPending)
                {
                    SitDownAndOrder();
                }
                break;

            case CustomerState.Waiting:
                currentPatience -= Time.deltaTime;

                if (imgPatience != null) imgPatience.fillAmount = currentPatience / patienceMax;

                if (currentPatience <= 0)
                {
                    Debug.Log("<color=red>🤬 Khách: Đợi mỏi cổ luôn! Quán phục vụ quá chậm! Đi về!</color>");
                    if (QuanLyKho.Instance != null)
                    {
                        QuanLyKho.Instance.DiemViral -= 5;
                        QuanLyKho.Instance.SaveGame();
                    }
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

        if (anim != null)
        {
            anim.SetBool("isWalking", false);
            anim.SetBool("isSitting", true);
        }

        orderMon = menu[Random.Range(0, menu.Length)];

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
                if (isPerfect)
                {
                    Debug.Log($"<color=green>🎉 Khách: Ngon Tuyệt Vời! Đánh giá 5 sao!</color>");
                    QuanLyKho.Instance.DiemViral += 10;
                    QuanLyKho.Instance.NhanTienBanNuoc(15000);
                }
                else
                {
                    Debug.Log($"<color=orange>😒 Khách: Uống cũng tạm, hơi nhạt.</color>");
                    QuanLyKho.Instance.DiemViral -= 5;
                    QuanLyKho.Instance.NhanTienBanNuoc(10000);
                }
            }
            Invoke("LeaveHappily", 3f);
        }
        else
        {
            Debug.Log($"<color=red>😡 Khách: Pha sai món rồi! Bo xì!</color>");
            if (QuanLyKho.Instance != null)
            {
                QuanLyKho.Instance.DiemViral -= 15;
                QuanLyKho.Instance.SaveGame();
            }
            GetMadAndLeave();
        }
    }

    void GetMadAndLeave()
    {
        if (chatBubble != null) chatBubble.SetActive(false);
        currentState = CustomerState.Leaving;

        // Khách giận bỏ đi không uống -> không có rác -> Trả ghế bình thường
        if (targetSeat != null) targetSeat.isOccupied = false;

        agent.enabled = true;

        if (anim != null)
        {
            anim.SetBool("isSitting", false);
            anim.SetBool("isWalking", true);
        }

        DiLangThangRoiBienMat();
    }

    void LeaveHappily()
    {
        currentState = CustomerState.Leaving;
        bool coDeLaiRac = false; // Cờ theo dõi xem có để rác lại không

        if (lyRongPrefab != null && targetSeat != null)
        {
            GameObject rac = Instantiate(lyRongPrefab, targetSeat.sitPosition.position, targetSeat.sitPosition.rotation);

            // Tìm script DonRac trên cả Object gốc lẫn Object con
            DonRac scriptDonRac = rac.GetComponent<DonRac>();
            if (scriptDonRac == null) scriptDonRac = rac.GetComponentInChildren<DonRac>();

            if (scriptDonRac != null)
            {
                scriptDonRac.gheDangNgoi = targetSeat;
                coDeLaiRac = true; // Xác nhận là có rác nằm trên ghế!
            }
        }

        // ========================================================
        // SỬA LỖI TẠI ĐÂY: NẾU TRÊN GHẾ CÓ RÁC THÌ TUYỆT ĐỐI KHÔNG TRẢ GHẾ!
        // Ghế sẽ bị "phong ấn" cho đến khi bạn bấm dọn rác bên file DonRac.cs
        // ========================================================
        if (!coDeLaiRac && targetSeat != null)
        {
            targetSeat.isOccupied = false;
        }

        agent.enabled = true;

        if (anim != null)
        {
            anim.SetBool("isSitting", false);
            anim.SetBool("isWalking", true);
        }

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
        // Vá thêm logic: Chỉ nhả ghế khi Game xóa Object mà không phải do đi về
        if (targetSeat != null && targetSeat.isOccupied && currentState != CustomerState.Leaving)
        {
            targetSeat.isOccupied = false;
        }
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