using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;

[InitializeOnLoad]
public class EpChayMenu
{
    static EpChayMenu()
    {
        // Đường dẫn tới HomeScene của sếp (Dựa theo ảnh Build Settings sếp gửi)
        string duongDanMenu = "Assets/Scenes/HomeScene.unity";

        SceneAsset menuScene = AssetDatabase.LoadAssetAtPath<SceneAsset>(duongDanMenu);

        if (menuScene != null)
        {
            // Ép nút Play của Unity luôn luôn khởi động cái Scene này
            EditorSceneManager.playModeStartScene = menuScene;
        }
        else
        {
            Debug.LogWarning("⚠️ [Hệ thống] Không tìm thấy HomeScene! Hãy kiểm tra lại đường dẫn: " + duongDanMenu);
        }
    }
}