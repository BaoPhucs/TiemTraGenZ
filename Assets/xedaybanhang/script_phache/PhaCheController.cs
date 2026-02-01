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

    public PhaCheState currentState = PhaCheState.ChuaCoLy;

    public GameObject lyTrong;
    public GameObject lyCoTra;
    public GameObject lyCoTraTac;
    public GameObject lyHoanThanh;

    void Start()
    {
        SetState(PhaCheState.ChuaCoLy);
    }

    void SetState(PhaCheState newState)
    {
        currentState = newState;

        lyTrong.SetActive(newState == PhaCheState.CoLy);
        lyCoTra.SetActive(newState == PhaCheState.CoTra);
        lyCoTraTac.SetActive(newState == PhaCheState.CoTac);
        lyHoanThanh.SetActive(newState == PhaCheState.HoanThanh);
    }

    public void LayLy()
    {
        if (currentState == PhaCheState.ChuaCoLy)
            SetState(PhaCheState.CoLy);
    }

    public void DoTra()
    {
        if (currentState == PhaCheState.CoLy)
            SetState(PhaCheState.CoTra);
    }

    public void ThemTac()
    {
        if (currentState == PhaCheState.CoTra)
            SetState(PhaCheState.CoTac);
    }

    public void ThemDa()
    {
        if (currentState == PhaCheState.CoTac)
            SetState(PhaCheState.HoanThanh);
    }
}
