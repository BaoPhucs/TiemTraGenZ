using UnityEngine;

public class PhaCheController : MonoBehaviour
{
    public enum PhaCheState
    {
        ChuaCoLy, CoLy, CoTra, CoTraTac, CoTraChanh, CoTraSua, CoMatcha, CoMatchaSua, CoCaPhe, CoCaPheSua, HoanThanh
    }

    public PhaCheState currentState = PhaCheState.ChuaCoLy;
    private string monDangPhaTam = "";

    [Header("=== GÁN OBJECT LY TRUNG GIAN ===")]
    public GameObject lyTrong;
    public GameObject lyCoTra;
    public GameObject lyCoTraTac;
    public GameObject lyCoTraChanh;
    public GameObject lyCoTraSua;
    public GameObject lyCoMatcha;
    public GameObject lyCoMatchaSua;
    public GameObject lyCoCaPhe;
    public GameObject lyCoCaPheSua;

    [Header("=== GÁN OBJECT LY HOÀN THÀNH (ĐÃ CÓ ĐÁ) ===")]
    public GameObject hoanThanh_TraDa;
    public GameObject hoanThanh_TraTac;
    public GameObject hoanThanh_TraChanh;
    public GameObject hoanThanh_TraSua;
    public GameObject hoanThanh_MatchaLatte;
    public GameObject hoanThanh_CaPheDen;
    public GameObject hoanThanh_CaPheSua;

    void OnEnable() { UpdateVisual(); }

    void UpdateVisual()
    {
        // 1. Tắt sạch sẽ tất cả các ly trung gian
        if (lyTrong) lyTrong.SetActive(false);
        if (lyCoTra) lyCoTra.SetActive(false);
        if (lyCoTraTac) lyCoTraTac.SetActive(false);
        if (lyCoTraChanh) lyCoTraChanh.SetActive(false);
        if (lyCoTraSua) lyCoTraSua.SetActive(false);
        if (lyCoMatcha) lyCoMatcha.SetActive(false);
        if (lyCoMatchaSua) lyCoMatchaSua.SetActive(false);
        if (lyCoCaPhe) lyCoCaPhe.SetActive(false);
        if (lyCoCaPheSua) lyCoCaPheSua.SetActive(false);

        // 2. Tắt sạch sẽ tất cả các ly hoàn thành
        if (hoanThanh_TraDa) hoanThanh_TraDa.SetActive(false);
        if (hoanThanh_TraTac) hoanThanh_TraTac.SetActive(false);
        if (hoanThanh_TraChanh) hoanThanh_TraChanh.SetActive(false);
        if (hoanThanh_TraSua) hoanThanh_TraSua.SetActive(false);
        if (hoanThanh_MatchaLatte) hoanThanh_MatchaLatte.SetActive(false);
        if (hoanThanh_CaPheDen) hoanThanh_CaPheDen.SetActive(false);
        if (hoanThanh_CaPheSua) hoanThanh_CaPheSua.SetActive(false);

        // 3. Bật đúng cái ly cần thiết
        switch (currentState)
        {
            case PhaCheState.CoLy: if (lyTrong) lyTrong.SetActive(true); break;
            case PhaCheState.CoTra: if (lyCoTra) lyCoTra.SetActive(true); break;
            case PhaCheState.CoTraTac: if (lyCoTraTac) lyCoTraTac.SetActive(true); break;
            case PhaCheState.CoTraChanh: if (lyCoTraChanh) lyCoTraChanh.SetActive(true); break;
            case PhaCheState.CoTraSua: if (lyCoTraSua) lyCoTraSua.SetActive(true); break;
            case PhaCheState.CoMatcha: if (lyCoMatcha) lyCoMatcha.SetActive(true); break;
            case PhaCheState.CoMatchaSua: if (lyCoMatchaSua) lyCoMatchaSua.SetActive(true); break;
            case PhaCheState.CoCaPhe: if (lyCoCaPhe) lyCoCaPhe.SetActive(true); break;
            case PhaCheState.CoCaPheSua: if (lyCoCaPheSua) lyCoCaPheSua.SetActive(true); break;

            case PhaCheState.HoanThanh:
                // Kiểm tra xem đang pha món gì để hiện đúng ly hoàn thành đó
                switch (monDangPhaTam)
                {
                    case "TraDa": if (hoanThanh_TraDa) hoanThanh_TraDa.SetActive(true); break;
                    case "TraTac": if (hoanThanh_TraTac) hoanThanh_TraTac.SetActive(true); break;
                    case "TraChanh": if (hoanThanh_TraChanh) hoanThanh_TraChanh.SetActive(true); break;
                    case "TraSua": if (hoanThanh_TraSua) hoanThanh_TraSua.SetActive(true); break;
                    case "MatchaLatte": if (hoanThanh_MatchaLatte) hoanThanh_MatchaLatte.SetActive(true); break;
                    case "CaPheDen": if (hoanThanh_CaPheDen) hoanThanh_CaPheDen.SetActive(true); break;
                    case "CaPheSua": if (hoanThanh_CaPheSua) hoanThanh_CaPheSua.SetActive(true); break;
                }
                break;
        }
    }

    public void SetState(PhaCheState newState)
    {
        currentState = newState;
        UpdateVisual();
    }

    public void LayLy() { if (currentState == PhaCheState.ChuaCoLy && QuanLyKho.Instance.SuDungNguyenLieu("Ly")) { SetState(PhaCheState.CoLy); } }

    public void DoTra() { if (currentState == PhaCheState.CoLy && QuanLyKho.Instance.SuDungNguyenLieu("Tra")) { SetState(PhaCheState.CoTra); monDangPhaTam = "TraDa"; } }

    public void DoTraSua() { if (currentState == PhaCheState.CoLy && QuanLyKho.Instance.unlockTraSua && QuanLyKho.Instance.SuDungNguyenLieu("TraSua")) { SetState(PhaCheState.CoTraSua); monDangPhaTam = "TraSua"; } }

    public void DoMatcha() { if (currentState == PhaCheState.CoLy && QuanLyKho.Instance.unlockMatcha && QuanLyKho.Instance.SuDungNguyenLieu("Matcha")) { SetState(PhaCheState.CoMatcha); } }

    public void DoCaPhe() { if (currentState == PhaCheState.CoLy && (QuanLyKho.Instance.unlockCaPheDen || QuanLyKho.Instance.unlockCaPheSua) && QuanLyKho.Instance.SuDungNguyenLieu("CaPhe")) { SetState(PhaCheState.CoCaPhe); monDangPhaTam = "CaPheDen"; } }

    public void ThemTac() { if (currentState == PhaCheState.CoTra && QuanLyKho.Instance.unlockTraTac && QuanLyKho.Instance.SuDungNguyenLieu("Tac")) { SetState(PhaCheState.CoTraTac); monDangPhaTam = "TraTac"; } }

    public void ThemChanh() { if (currentState == PhaCheState.CoTra && QuanLyKho.Instance.unlockTraChanh && QuanLyKho.Instance.SuDungNguyenLieu("Chanh")) { SetState(PhaCheState.CoTraChanh); monDangPhaTam = "TraChanh"; } }

    public void ThemSua()
    {
        if (currentState == PhaCheState.CoMatcha && QuanLyKho.Instance.SuDungNguyenLieu("Sua")) { SetState(PhaCheState.CoMatchaSua); monDangPhaTam = "MatchaLatte"; }
        else if (currentState == PhaCheState.CoCaPhe && QuanLyKho.Instance.unlockCaPheSua && QuanLyKho.Instance.SuDungNguyenLieu("Sua")) { SetState(PhaCheState.CoCaPheSua); monDangPhaTam = "CaPheSua"; }
    }

    public void ThemDa()
    {
        if (currentState == PhaCheState.CoTra || currentState == PhaCheState.CoTraTac ||
            currentState == PhaCheState.CoTraChanh || currentState == PhaCheState.CoTraSua ||
            currentState == PhaCheState.CoMatchaSua || currentState == PhaCheState.CoCaPhe || currentState == PhaCheState.CoCaPheSua)
        {
            if (QuanLyKho.Instance.SuDungNguyenLieu("Da")) { SetState(PhaCheState.HoanThanh); }
        }
    }

    public void ThuHoiLy()
    {
        if (currentState == PhaCheState.HoanThanh)
        {
            if (MinigamePhaChe.Instance != null) MinigamePhaChe.Instance.BatDauMinigame(monDangPhaTam, null);
            SetState(PhaCheState.ChuaCoLy);
        }
    }
}