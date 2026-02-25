using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Collections;

namespace TiemTraGenZ.Manager
{
    public class UI_EndingManager : MonoBehaviour
    {
        [Header("UI Components")]
        public GameObject endingPanel;       // Panel đen che toàn màn hình
        public CanvasGroup canvasGroup;      // Để làm hiệu ứng mờ dần
        public TextMeshProUGUI endingText;   // Text hiển thị nội dung kết cục
        public Button backToMenuButton;      // Nút để thoát

        [Header("Ending Texts")]
        [TextArea(3, 10)]
        public string textBadEnding = "Tiệm Trà chìm vào quên lãng. Minh trở lại với màn hình máy tính và những deadline vô vị nơi công sở...";
        [TextArea(3, 10)]
        public string textNormalEnding = "Tiệm Trà phủ sóng khắp cả nước với 50 chi nhánh. Minh trở thành một triệu phú, nhưng mỗi lần uống ly trà công nghiệp ấy, vị đắng của sự mất mát lại hiện lên...";
        [TextArea(3, 10)]
        public string textTrueEnding = "Tiệm Trà không trở thành chuỗi khổng lồ, nhưng lại là tài sản vô giá của khu phố. Nó là điểm giao thoa giữa hương vị truyền thống và nhịp sống trẻ gen Z...";

        private bool isEndingShown = false; // Đánh dấu đã hiện xong Ending chưa

        private void Update()
        {
            // Khi màn hình Ending đã hiện xong, bấm F để quay về Menu
            if (isEndingShown && Input.GetKeyDown(KeyCode.F))
            {
                ReturnToMenu();
            }
        }

        /// <summary>
        /// Được gọi trực tiếp từ PhoneSystem.HangUpCall() khi cuộc gọi Ending kết thúc.
        /// KHÔNG dùng Event pattern nữa vì GameObject bị inactive sẽ không đăng ký được.
        /// </summary>
        public void ShowEnding(GameEnding endingResult)
        {
            Debug.Log($"[UI_EndingManager] ShowEnding được gọi! Ending = {endingResult}");

            // Bước 1: Bật chính GameObject này lên trước (nếu đang bị tắt)
            if (!gameObject.activeSelf)
            {
                gameObject.SetActive(true);
                Debug.Log("[UI_EndingManager] Đã bật gameObject lên.");
            }

            if (endingPanel == null || canvasGroup == null || endingText == null)
            {
                Debug.LogError("[UI_EndingManager] Thiếu UI references trong Inspector!");
                return;
            }

            // Bước 2: Reset alpha về 0 để chuẩn bị fade in
            canvasGroup.alpha = 0f;

            // Bước 3: Gán chữ tương ứng với kết cục
            switch (endingResult)
            {
                case GameEnding.BadEnding_OfficeWorker:
                    endingText.text = textBadEnding;
                    break;
                case GameEnding.NormalEnding_Franchise:
                    endingText.text = textNormalEnding;
                    break;
                case GameEnding.TrueEnding_CulturalHeritage:
                    endingText.text = textTrueEnding;
                    break;
                default:
                    endingText.text = "Cuộc hành trình kết thúc...";
                    break;
            }

            // Bước 4: Ẩn nút Back to menu (chờ fade xong mới hiện)
            if (backToMenuButton != null)
            {
                backToMenuButton.gameObject.SetActive(false);
                backToMenuButton.onClick.AddListener(ReturnToMenu);
            }

            // Bước 5: Bật Panel và bắt đầu Fade in
            endingPanel.SetActive(true);
            StartCoroutine(FadeInEnding());
        }

        private IEnumerator FadeInEnding()
        {
            float duration = 2.0f;
            float elapsed = 0f;

            while (elapsed < duration)
            {
                elapsed += Time.deltaTime;
                canvasGroup.alpha = Mathf.Lerp(0f, 1f, elapsed / duration);
                yield return null;
            }

            canvasGroup.alpha = 1f;

            // Hiển thị nút Back to menu sau khi fadeIn xong
            if (backToMenuButton != null)
            {
                backToMenuButton.gameObject.SetActive(true);
            }

            isEndingShown = true;
            Debug.Log("[UI_EndingManager] Fade xong! Bấm F hoặc click nút để quay về Menu.");
        }

        public void ReturnToMenu()
        {
            Debug.Log("[UI_EndingManager] Đang tải lại Main Menu...");
            Time.timeScale = 1f;
            SceneManager.LoadScene(0); 
        }
    }
}

