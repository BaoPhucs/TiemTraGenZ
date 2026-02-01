using UnityEngine;

public class PhaCheClick : MonoBehaviour
{
    public enum ActionType
    {
        LayLy,
        DoTra,
        ThemTac,
        ThemDa
    }

    public ActionType action;
    public PhaCheController phaChe;

    void OnMouseDown()
    {
        if (phaChe == null) return;

        switch (action)
        {
            case ActionType.LayLy:
                phaChe.LayLy();
                break;
            case ActionType.DoTra:
                phaChe.DoTra();
                break;
            case ActionType.ThemTac:
                phaChe.ThemTac();
                break;
            case ActionType.ThemDa:
                phaChe.ThemDa();
                break;
        }
    }
}
