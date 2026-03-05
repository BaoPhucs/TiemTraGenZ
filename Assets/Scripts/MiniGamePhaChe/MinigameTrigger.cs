using UnityEngine;

public class MinigameTrigger : MonoBehaviour
{
    private MinigamePhaChe minigameUI;
    private bool dungGanMay = false;

    void Start()
    {
        // Tự động tìm UI Minigame trên màn hình (khỏi cần kéo thả tốn thời gian)
        minigameUI = FindObjectOfType<MinigamePhaChe>();
    }

    void Update()
    {
        // Kích hoạt khi đứng gần và bấm E
        if (dungGanMay && Input.GetKeyDown(KeyCode.E))
        {
            if (minigameUI != null)
            {
                minigameUI.BatDauMinigame();
            }
        }
    }

    void OnTriggerEnter(Collider other)
    {
        // Chạm vào người chơi thì bật cờ cho phép bấm E
        if (other.CompareTag("Player"))
        {
            dungGanMay = true;
            Debug.Log("Đã đứng gần máy pha chế! Bấm E để pha!");
        }
    }

    void OnTriggerExit(Collider other)
    {
        // Đi ra xa thì tắt
        if (other.CompareTag("Player"))
        {
            dungGanMay = false;
        }
    }
}