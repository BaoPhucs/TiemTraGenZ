using UnityEngine;
using UnityEngine.SceneManagement; // Cần cái này để chuyển cảnh

public class MainMenuController : MonoBehaviour
{
    // Hàm này sẽ gắn vào nút Play
    public void PlayGame()
    {
        // Load cái màn chơi thành phố (Nhớ tên Scene phải chuẩn)
        SceneManager.LoadScene("SampleScene");
    }

    // Hàm này gắn vào nút Quit
    public void QuitGame()
    {
        Debug.Log("Đã thoát game!");
        Application.Quit();
    }
}