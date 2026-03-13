using UnityEngine;

public class TutorialManager : MonoBehaviour
{
    public static TutorialManager Instance { get; private set; }

    // Biến nhớ tạm trong RAM
    public static bool DaHuongDanBaTu = false;

    // =========================================================================
    // PHÉP MÀU NẰM Ở ĐÂY: Hàm này CHỈ CHẠY 1 LẦN DUY NHẤT khi sếp bấm nút PLAY.
    // Nó hoàn toàn bị "mù" và KHÔNG chạy khi sếp dùng code để LoadScene (Chơi lại sau Ending).
    // =========================================================================
    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.SubsystemRegistration)]
    static void XoaTriNhoKhiBamPlay()
    {
        DaHuongDanBaTu = false;
        Debug.Log("🔄 [Tutorial] Đã reset bộ nhớ hướng dẫn khi vừa bật game!");
    }

    [Header("UI Mũi Tên Chỉ Đường")]
    public GameObject muiTenTrenDauBaTu;
    public GameObject muiTenDuoiChanMinh;

    [Header("Mục tiêu")]
    public Transform viTriBaTu;
    public Transform viTriMinh;

    private bool dangHuongDan = false;

    private void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
    }

    private void Start()
    {
        if (muiTenTrenDauBaTu) muiTenTrenDauBaTu.SetActive(false);
        if (muiTenDuoiChanMinh) muiTenDuoiChanMinh.SetActive(false);
    }

    private void Update()
    {
        if (dangHuongDan && muiTenDuoiChanMinh != null && viTriBaTu != null && viTriMinh != null)
        {
            muiTenDuoiChanMinh.transform.position = viTriMinh.position + new Vector3(0.2f, 0.1f, 0);
            Vector3 targetPosition = new Vector3(viTriBaTu.position.x, muiTenDuoiChanMinh.transform.position.y, viTriBaTu.position.z);
            muiTenDuoiChanMinh.transform.LookAt(targetPosition);
        }
    }

    public void ShowTutorial()
    {
        dangHuongDan = true;
        if (muiTenTrenDauBaTu) muiTenTrenDauBaTu.SetActive(true);
        if (muiTenDuoiChanMinh) muiTenDuoiChanMinh.SetActive(true);
        Debug.Log("🟢 [Tutorial] Đã bật mũi tên chỉ đường đến Bà Tư!");
    }

    public void HideTutorial()
    {
        dangHuongDan = false;
        if (muiTenTrenDauBaTu) muiTenTrenDauBaTu.SetActive(false);
        if (muiTenDuoiChanMinh) muiTenDuoiChanMinh.SetActive(false);
        Debug.Log("🔴 [Tutorial] Đã tắt mũi tên chỉ đường!");
    }
}