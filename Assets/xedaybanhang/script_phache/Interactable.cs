using UnityEngine;
using static PhaCheController;

public class Interactable : MonoBehaviour
{
    [Header("UI")]
    public string interactText = "Ấn E để tương tác";
    public SpawnOnInteract spawner;

    [Header("Action")]
    public InteractType interactType;
    public PhaCheController phaChe;

    public enum InteractType
    {
        LayLy, DoTra, ThemTac, ThemDa, LayBan, LayGhe,
        ThemChanh, DoTraSua, DoMatcha, ThemSua, DoCaPhe
    }

    public void Interact()
    {
        switch (interactType)
        {
            case InteractType.LayLy: phaChe.LayLy(); break;
            case InteractType.DoTra: phaChe.DoTra(); break;
            case InteractType.ThemTac: phaChe.ThemTac(); break;
            case InteractType.ThemChanh: phaChe.ThemChanh(); break;
            case InteractType.DoTraSua: phaChe.DoTraSua(); break;
            case InteractType.DoMatcha: phaChe.DoMatcha(); break;
            case InteractType.ThemSua: phaChe.ThemSua(); break;
            case InteractType.DoCaPhe: phaChe.DoCaPhe(); break;
            case InteractType.ThemDa: phaChe.ThemDa(); break;
            case InteractType.LayBan:
            case InteractType.LayGhe:
                if (spawner != null) spawner.Spawn();
                break;
        }
    }

    public bool CanShow()
    {
        if (phaChe == null) return true;

        switch (interactType)
        {
            case InteractType.DoTra:
            case InteractType.DoTraSua:
            case InteractType.DoMatcha:
            case InteractType.DoCaPhe:
                return phaChe.currentState == PhaCheState.CoLy;

            case InteractType.ThemTac:
            case InteractType.ThemChanh:
                return phaChe.currentState == PhaCheState.CoTra;

            case InteractType.ThemSua:
                return (phaChe.currentState == PhaCheState.CoMatcha || phaChe.currentState == PhaCheState.CoCaPhe);

            case InteractType.ThemDa:
                return (phaChe.currentState == PhaCheState.CoTra ||
                        phaChe.currentState == PhaCheState.CoTraTac ||
                        phaChe.currentState == PhaCheState.CoTraChanh ||
                        phaChe.currentState == PhaCheState.CoTraSua ||
                        phaChe.currentState == PhaCheState.CoMatchaSua ||
                        phaChe.currentState == PhaCheState.CoCaPhe ||
                        phaChe.currentState == PhaCheState.CoCaPheSua);
        }
        return true;
    }
}