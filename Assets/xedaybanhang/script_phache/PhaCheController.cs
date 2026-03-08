using UnityEngine;

public class PhaCheController : MonoBehaviour
{
    public enum PhaCheState
    {
        ChuaCoLy,
        CoLy,
        CoTra,
        CoTac,
        HoanThanh,
        None
    }

    // Bỏ static, Unity sẽ tự nhớ giá trị này khi tắt/bật object
    public PhaCheState currentState = PhaCheState.ChuaCoLy;

    [Header("Gán Object Ly Tương Ứng")]
    public GameObject lyTrong;
    public GameObject lyCoTra;
    public GameObject lyCoTraTac;
    public GameObject lyHoanThanh;

    // Chạy mỗi khi Object được Bật lên (Active) hoặc khi Game Start
    void OnEnable()
    {
        // Mỗi khi bật lại (ví dụ tắt shop đi bật lại xe), 
        // nó sẽ tự kiểm tra state đang là gì để hiển thị đúng cái ly đó.
        UpdateVisual();
    }

    // Hàm cập nhật hình ảnh dựa trên State hiện tại
    void UpdateVisual()
    {
        // Tắt hết trước cho sạch
        if (lyTrong) lyTrong.SetActive(false);
        if (lyCoTra) lyCoTra.SetActive(false);
        if (lyCoTraTac) lyCoTraTac.SetActive(false);
        if (lyHoanThanh) lyHoanThanh.SetActive(false);

        // Bật cái cần thiết
        switch (currentState)
        {
            case PhaCheState.CoLy:
                if (lyTrong) lyTrong.SetActive(true);
                break;
            case PhaCheState.CoTra:
                if (lyCoTra) lyCoTra.SetActive(true);
                break;
            case PhaCheState.CoTac:
                if (lyCoTraTac) lyCoTraTac.SetActive(true);
                break;
            case PhaCheState.HoanThanh:
                if (lyHoanThanh) lyHoanThanh.SetActive(true);
                break;
        }

        Debug.Log($"[PhaChe] Đã cập nhật hình ảnh theo trạng thái: {currentState}");
    }

    public void SetState(PhaCheState newState)
    {
        currentState = newState;
        UpdateVisual();
    }

    // --- CÁC HÀM GỌI TỪ NÚT BẤM (GIỮ NGUYÊN LOGIC CŨ) ---

    public void LayLy()
    {
        if (currentState == PhaCheState.ChuaCoLy)
        {
            if (QuanLyKho.Instance.SuDungNguyenLieu("Ly"))
            {
                SetState(PhaCheState.CoLy);
            }
        }
    }

    public void DoTra()
    {
        if (currentState == PhaCheState.CoLy)
        {
            if (QuanLyKho.Instance.SuDungNguyenLieu("Tra"))
            {
                SetState(PhaCheState.CoTra);
            }
        }
    }

    public void ThemTac()
    {
        if (currentState == PhaCheState.CoTra)
        {
            if (QuanLyKho.Instance.SuDungNguyenLieu("Tac"))
            {
                SetState(PhaCheState.CoTac);
            }
        }
    }

    public void ThemDa()
    {
        if (currentState == PhaCheState.CoTac)
        {
            if (QuanLyKho.Instance.SuDungNguyenLieu("Da"))
            {
                SetState(PhaCheState.HoanThanh);
            }
        }
    }

    //public void ThuHoiLy()
    //{
    //    if (currentState == PhaCheState.HoanThanh)
    //    {
    //        // --- LOGIC MỚI: BƯNG NƯỚC LÊN TAY ---
    //        if (PlayerHand.Instance != null)
    //        {
    //            // Ghi nhớ món đang cầm vào tay Minh
    //            PlayerHand.Instance.monDangCam = "TraTac";
    //            Debug.Log("Minh đang cầm: " + PlayerHand.Instance.monDangCam);
    //        }
    //        else
    //        {
    //            Debug.LogWarning("⚠️ Chưa tìm thấy PlayerHand! Hãy nhớ gắn script PlayerHand vào nhân vật Minh nhé.");
    //        }

    //        // --- LOGIC CŨ: RESET BÀN PHA CHẾ ---
    //        // Hàm SetState sẽ tự động tắt lyHoanThanh.SetActive(false) giúp bạn
    //        SetState(PhaCheState.ChuaCoLy);
    //    }
    //}
    public void ThuHoiLy()
    {
        if (currentState == PhaCheState.HoanThanh)
        {
            // =========================================================
            // BƯỚC 1: KÍCH HOẠT MINIGAME ĐỂ QUYẾT ĐỊNH CHẤT LƯỢNG
            // =========================================================
            if (MinigamePhaChe.Instance != null)
            {
                // Gọi bảng Minigame lên. 
                // Truyền chữ "TraTac" để lát hệ thống biết đường giao cho khách.
                // Truyền 'null' ở vế sau vì chúng ta không muốn lệnh Destroy của Minigame làm mất luôn cái ly gốc trên bàn.
                MinigamePhaChe.Instance.BatDauMinigame("TraTac", null);
            }
            else
            {
                Debug.LogWarning("⚠️ Chưa tìm thấy MinigamePhaChe trên Scene! Trả về ly trà mặc định.");
                if (PlayerHand.Instance != null) PlayerHand.Instance.monDangCam = "TraTac";
            }

            // =========================================================
            // BƯỚC 2: RESET BÀN PHA CHẾ ĐỂ LÀM LY MỚI
            // =========================================================
            // Ngay khi Minigame hiện lên, ly trên bàn sẽ được ẩn đi (SetState về rỗng) 
            // tạo cảm giác nhân vật đã "cầm cái ly lên tay" để lắc/pha chế.
            SetState(PhaCheState.ChuaCoLy);
        }
    }

}