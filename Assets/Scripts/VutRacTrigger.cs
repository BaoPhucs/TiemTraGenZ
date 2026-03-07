using UnityEngine;

public class VutRacTrigger : MonoBehaviour
{
    [Header("Kéo thả Canvas vào đây:")]
    public GameObject textUI;

    [Header("Kéo thả THÙNG RÁC vào đây:")]
    public AudioSource amThanhVutRac;

    private bool dungGanThungRac = false;

    void Start()
    {
        if (textUI != null) textUI.SetActive(false);
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            dungGanThungRac = true;
            if (textUI != null) textUI.SetActive(true);
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            dungGanThungRac = false;
            if (textUI != null) textUI.SetActive(false);
        }
    }

    void Update()
    {
        if (dungGanThungRac && Input.GetKeyDown(KeyCode.E))
        {
            ThucHienVutRac();
        }
    }

    void ThucHienVutRac()
    {
        Debug.Log("♻️ ĐÃ VỨT RÁC THÀNH CÔNG! Quán xá sạch sẽ!");

        // Lệnh phát âm thanh vứt rác
        if (amThanhVutRac != null)
        {
            amThanhVutRac.Play();
        }
    }
}