using UnityEngine;
using UnityEngine.SceneManagement; // Bắt buộc phải có để chuyển cảnh

public class MainMenuManager : MonoBehaviour
{
    [Header("Các Tấm Ảnh Popup")]
    public GameObject popupAbout;
    public GameObject popupTutorial;

    [Header("Tên Scene Game Chính")]
    public string tenSceneGame = "SampleScene"; // Sếp nhớ gõ đúng tên Scene game của sếp nhé

    void Start()
    {
        // Đảm bảo thời gian chạy bình thường và nhả chuột cho người chơi bấm
        Time.timeScale = 1f;
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;

        // Giấu ảnh khi mới vào
        if (popupAbout != null) popupAbout.SetActive(false);
        if (popupTutorial != null) popupTutorial.SetActive(false);
    }

    // --- CHỨC NĂNG NÚT PLAY ---
    public void NhanNutPlay()
    {
        Debug.Log("Đang vào game...");
        SceneManager.LoadScene(tenSceneGame);
    }

    // --- CHỨC NĂNG NÚT ABOUT ---
    public void MoAnhAbout()
    {
        if (popupAbout != null) popupAbout.SetActive(true);
    }

    public void DongAnhAbout()
    {
        if (popupAbout != null) popupAbout.SetActive(false);
    }

    // --- CHỨC NĂNG NÚT TUTORIAL ---
    public void MoAnhTutorial()
    {
        if (popupTutorial != null) popupTutorial.SetActive(true);
    }

    public void DongAnhTutorial()
    {
        if (popupTutorial != null) popupTutorial.SetActive(false);
    }
}