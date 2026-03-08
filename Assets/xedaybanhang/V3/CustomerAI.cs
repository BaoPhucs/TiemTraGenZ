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
                // SỬA LỖI 1: NẾU GHẾ BỊ NGƯỜI CHƠI THU HỒI GIỮA CHỪNG
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

                // ========================================================
                // BỔ SUNG ÁN PHẠT TẠI ĐÂY: KHÁCH HẾT KIÊN NHẪN (ĐỢI QUÁ LÂU)
                // ========================================================
                if (currentPatience <= 0)
                {
                    Debug.Log("<color=red>🤬 Khách: Đợi mỏi cổ luôn! Quán phục vụ quá chậm! Đi về!</color>");

                    if (QuanLyKho.Instance != null)
                    {
                        QuanLyKho.Instance.DiemViral -= 5; 
                        QuanLyKho.Instance.SaveGame(); // Ép lưu vào ổ cứng ngay lập tức
                    }

                    GetMadAndLeave();
                }

                // Chờ giao nước
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
                    // SỬA LỖI: CỘNG VIRAL TRƯỚC RỒI MỚI LƯU BẰNG HÀM NHẬN TIỀN
                    QuanLyKho.Instance.DiemViral += 10;
                    QuanLyKho.Instance.NhanTienBanNuoc(15000);
                }
                else
                {
                    Debug.Log($"<color=orange>😒 Khách: Uống cũng tạm, hơi nhạt.</color>");
                    // SỬA LỖI: TRỪ VIRAL TRƯỚC RỒI MỚI LƯU BẰNG HÀM NHẬN TIỀN
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
                // SỬA LỖI: TRỪ VIRAL VÀ ÉP LƯU NGAY LẬP TỨC
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

        if (targetSeat != null) targetSeat.isOccupied = false; // Trả lại ghế

        agent.enabled = true; // Bật lại hệ thống tìm đường

        if (anim != null)
        {
            anim.SetBool("isSitting", false);
            anim.SetBool("isWalking", true);
        }

        DiLangThangRoiBienMat(); 
    }

    void LeaveHappily()
    {
        if (lyRongPrefab != null && targetSeat != null)
        {
            GameObject rac = Instantiate(lyRongPrefab, targetSeat.sitPosition.position, targetSeat.sitPosition.rotation);
            DonRac scriptDonRac = rac.GetComponentInChildren<DonRac>();
            if (scriptDonRac != null) scriptDonRac.gheDangNgoi = targetSeat;
        }

        if (targetSeat != null) targetSeat.isOccupied = false; // Trả lại ghế

        agent.enabled = true; // Bật lại hệ thống tìm đường

        if (anim != null)
        {
            anim.SetBool("isSitting", false);
            anim.SetBool("isWalking", true);
        }

        DiLangThangRoiBienMat(); // Gọi hàm đi về
    }

    // ========================================================
    // SỬA LỖI 2: KHÁCH ĐI BỘ TẠI CHỖ KHI BỎ ĐI
    // ========================================================
    void DiLangThangRoiBienMat()
    {
        // Chọn ngẫu nhiên 1 điểm cách đó 10m để khách bước đi rồi mới tàng hình
        Vector3 randomDest = transform.position + new Vector3(Random.Range(-10f, 10f), 0, Random.Range(-10f, 10f));
        if (NavMesh.SamplePosition(randomDest, out NavMeshHit hit, 10f, NavMesh.AllAreas))
        {
            agent.SetDestination(hit.position);
        }

        Destroy(gameObject, 2.5f); // Chờ 2.5s để khách đi khuất bóng rồi xóa
    }

    // Đề phòng trường hợp lỗi game khách bốc hơi giữa chừng, ghế vẫn phải được nhả ra
    void OnDestroy()
    {
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