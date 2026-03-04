using UnityEngine;

public class PlayerHand : MonoBehaviour
{
    public static PlayerHand Instance;

    [Header("Món đang cầm trên tay")]
    public string monDangCam = ""; // Rỗng là không cầm gì

    void Awake()
    {
        Instance = this;
    }
}