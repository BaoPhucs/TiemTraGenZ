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
        LayLy,
        DoTra,
        ThemTac,
        ThemDa,
        LayBan,
        LayGhe
    }
    public void Interact()
    {
        switch (interactType)
        {
            case InteractType.LayLy:
                phaChe.LayLy();
                break;
            case InteractType.DoTra:
                phaChe.DoTra();
                break;
            case InteractType.ThemTac:
                phaChe.ThemTac();
                break;
            case InteractType.ThemDa:
                phaChe.ThemDa();
                break;
            case InteractType.LayBan:
            case InteractType.LayGhe:
                if (spawner != null)
                    spawner.Spawn();
                break;
        }
    }
    public bool CanShow()
    {
        if (phaChe == null) return true;

        switch (interactType)
        {
            case InteractType.DoTra:
                return phaChe.currentState == PhaCheState.CoLy;
            case InteractType.ThemTac:
                return phaChe.currentState == PhaCheState.CoTra;
            case InteractType.ThemDa:
                return phaChe.currentState == PhaCheState.CoTac;
        }
        return true;
    }


}
