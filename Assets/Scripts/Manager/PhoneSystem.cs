using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using TMPro;
using TiemTraGenZ.Data;
using TiemTraGenZ.Player;
using System; // Added for System.Action

namespace TiemTraGenZ.Manager
{
    public class PhoneSystem : MonoBehaviour
    {
        public static PhoneSystem Instance { get; private set; }

        public Animator playerAnimator;             // Dùng cho animation mới
        
        //public event System.Action OnCallEnded;     // Event bắn ra khi cúp máy

        [Header("--- ANIMATION COMPONENT CU --")]
        [SerializeField] private GameObject phonePanel;
        [SerializeField] private GameObject incomingCallView;
        [SerializeField] private GameObject inCallView;
        
        [Header("Incoming Call UI")]
        [SerializeField] private Image callerAvatar;
        [SerializeField] private TextMeshProUGUI callerNameText;
        [SerializeField] private TextMeshProUGUI statusText;
        [SerializeField] private Button acceptButton;
        [SerializeField] private Button rejectButton;
        
        [Header("In Call UI")]
        [SerializeField] private Image callerAvatarSmall;
        [SerializeField] private TextMeshProUGUI callerNameInCall;
        [SerializeField] private TextMeshProUGUI callDurationText;
        [SerializeField] private TextMeshProUGUI dialogueContentText;
        [SerializeField] private Button nextButton;
        [SerializeField] private Button hangUpButton;
        
        [Header("Audio")]
        [SerializeField] private AudioSource ringtoneAudio;
        
        [Header("References")]
        [SerializeField] private PlayerAnimationController playerAnimationController;
        
        // Current call data
        private CallDialogueData currentCall;
        private bool isCallActive = false;
        private float callDuration = 0f;
        
        // Internal dialogue management
        private Queue<DialogueLine> dialogueQueue = new Queue<DialogueLine>();
        private bool isTyping = false;
        private Coroutine typingCoroutine;
        private Coroutine callTimerCoroutine;

        private void Awake()
        {
            if (Instance == null)
            {
                Instance = this;
                DontDestroyOnLoad(gameObject); // Persist across scenes
            }
            else
            {
                Destroy(gameObject);
                return;
            }
            
            // Register button listeners
            if (acceptButton) acceptButton.onClick.AddListener(AcceptCall);
            if (rejectButton) rejectButton.onClick.AddListener(RejectCall);
            if (nextButton) nextButton.onClick.AddListener(OnNextButtonClicked);
            if (hangUpButton) hangUpButton.onClick.AddListener(HangUpCall);
        }

        private void Start()
        {
            // Hide all phone UI by default
            if (phonePanel) phonePanel.SetActive(false);
            if (incomingCallView) incomingCallView.SetActive(false);
            if (inCallView) inCallView.SetActive(false);
            
            // Auto-find PlayerAnimationController if not assigned
            if (playerAnimationController == null)
            {
                playerAnimationController = FindObjectOfType<PlayerAnimationController>();
                if (playerAnimationController != null)
                    Debug.Log("[PhoneSystem] Auto-found PlayerAnimationController");
                else
                    Debug.LogWarning("[PhoneSystem] PlayerAnimationController not found in scene. Phone animations will be disabled.");
            }
        }

        private void Update()
        {
            // Keyboard shortcuts for incoming call
            if (incomingCallView != null && incomingCallView.activeSelf)
            {
                if (Input.GetKeyDown(KeyCode.B))
                {
                    Debug.Log("[PhoneSystem] B pressed - Accepting call");
                    AcceptCall();
                    return; // Consume input to prevent processing as "next dialogue"
                }
                if (Input.GetKeyDown(KeyCode.C))
                {
                    Debug.Log("[PhoneSystem] C pressed - Rejecting call");
                    RejectCall();
                    return;
                }
            }
            
            // Keyboard shortcuts during call
            if (isCallActive)
            {
                if (Input.GetKeyDown(KeyCode.B))
                    OnNextButtonClicked();
                if (Input.GetKeyDown(KeyCode.C))
                    HangUpCall();
            }
        }

        /// <summary>
        /// Trigger an incoming call with CallDialogueData
        /// </summary>
        public void TriggerCall(CallDialogueData callData)
        {
            if (callData == null)
            {
                Debug.LogError("[PhoneSystem] CallDialogueData is null!");
                return;
            }
            
            currentCall = callData;
            ShowIncomingCall();
        }

        /// <summary>
        /// Show incoming call screen
        /// </summary>
        private void ShowIncomingCall()
        {
            Debug.Log($"[PhoneSystem] Incoming call from {currentCall.callerName}");
            
            // Show phone panel and incoming view
            if (phonePanel) phonePanel.SetActive(true);
            if (incomingCallView) incomingCallView.SetActive(true);
            if (inCallView) inCallView.SetActive(false);
            
            // Update UI
            if (callerAvatar) callerAvatar.sprite = currentCall.callerAvatar;
            if (callerNameText) callerNameText.text = currentCall.callerName;
            if (statusText) statusText.text = "Đang gọi...";
            
            // Play ringtone
            if (ringtoneAudio && currentCall.ringtone)
            {
                ringtoneAudio.clip = currentCall.ringtone;
                ringtoneAudio.loop = true;
                ringtoneAudio.Play();
            }
        }

        /// <summary>
        /// Accept the incoming call
        /// </summary>
        public void AcceptCall()
        {
            Debug.Log("[PhoneSystem] Call accepted");
            
            // Stop ringtone
            if (ringtoneAudio) ringtoneAudio.Stop();
            
            // Switch to in-call view
            if (incomingCallView) incomingCallView.SetActive(false);
            if (inCallView) inCallView.SetActive(true);
            
            // Update in-call UI
            if (callerAvatarSmall) callerAvatarSmall.sprite = currentCall.callerAvatar;
            if (callerNameInCall) callerNameInCall.text = currentCall.callerName;
            
            // Start call timer
            isCallActive = true;
            callDuration = 0f;
            if (callTimerCoroutine != null) StopCoroutine(callTimerCoroutine);
            callTimerCoroutine = StartCoroutine(UpdateCallTimer());
            
            // Start phone animation
            if (playerAnimationController)
                playerAnimationController.StartPhoneAnimation();
            
            // Start dialogue - Load into internal queue
            if (currentCall.dialogue != null)
            {
                dialogueQueue.Clear();
                foreach (var line in currentCall.dialogue.lines)
                {
                    dialogueQueue.Enqueue(line);
                }
                
                if (nextButton) nextButton.gameObject.SetActive(true);
                DisplayNextDialogueLine();
            }
            else
            {
                Debug.LogWarning("[PhoneSystem] No dialogue data!");
            }
        }

        /// <summary>
        /// Reject the incoming call
        /// </summary>
        public void RejectCall()
        {
            Debug.Log("[PhoneSystem] Call rejected");
            
            // Stop ringtone
            if (ringtoneAudio) ringtoneAudio.Stop();
            
            // Hide phone UI
            if (phonePanel) phonePanel.SetActive(false);
            if (incomingCallView) incomingCallView.SetActive(false);
            
            currentCall = null;
        }

        /// <summary>
        /// Hang up the current call
        /// </summary>
        public void HangUpCall()
        {
            Debug.Log("[PhoneSystem] Call ended");
            
            // Stop call timer
            if (callTimerCoroutine != null)
            {
                StopCoroutine(callTimerCoroutine);
                callTimerCoroutine = null;
            }
            
            // Stop typing coroutine
            if (typingCoroutine != null)
            {
                StopCoroutine(typingCoroutine);
                typingCoroutine = null;
            }
            
            isCallActive = false;
            isTyping = false;
            currentCall = null;
            dialogueQueue.Clear();
            
            // Hide all UI
            if (phonePanel) phonePanel.SetActive(false);
            if (incomingCallView) incomingCallView.SetActive(false);
            if (inCallView) inCallView.SetActive(false);
            
            // Stop phone animation
            if (playerAnimationController)
            {
                playerAnimationController.StopPhoneAnimation();
                Debug.Log("[PhoneSystem] Stopped phone animation");
            }
            
            // Thay vì bắn event (bị lỗi vì UI inactive không đăng ký được),
            // ta gọi THẲNG UI_EndingManager luôn nếu đang ở cuộc gọi Ending
            if (StoryManager.Instance != null && StoryManager.Instance.CurrentEnding != GameEnding.None)
            {
                Debug.Log($"[PhoneSystem] Phát hiện CurrentEnding = {StoryManager.Instance.CurrentEnding}. Đang tìm UI_EndingManager...");
                
                // FindObjectOfType(true) = tìm cả những GameObject bị inactive
                var endingUI = FindObjectOfType<UI_EndingManager>(true);
                if (endingUI != null)
                {
                    Debug.Log("[PhoneSystem] Tìm thấy UI_EndingManager! Gọi ShowEnding trực tiếp.");
                    endingUI.ShowEnding(StoryManager.Instance.CurrentEnding);
                }
                else
                {
                    Debug.LogError("[PhoneSystem] KHÔNG TÌM THẤY UI_EndingManager trong Scene! Hãy kiểm tra lại Hierarchy.");
                }
            }
            else
            {
                Debug.Log("[PhoneSystem] Cuộc gọi thường, không phải Ending.");
            }
        }

        /// <summary>
        /// Handle next button click - display next dialogue line
        /// </summary>
        private void OnNextButtonClicked()
        {
            // Skip typing if currently typing
            if (isTyping)
            {
                if (typingCoroutine != null) StopCoroutine(typingCoroutine);
                isTyping = false;
                // Show full text immediately
                if (dialogueContentText != null && dialogueQueue.Count > 0)
                {
                    // Get the current line that was being typed
                    return;
                }
            }
            
            // Display next line
            DisplayNextDialogueLine();
        }
        
        /// <summary>
        /// Display next dialogue line in phone UI
        /// </summary>
        private void DisplayNextDialogueLine()
        {
            if (dialogueQueue.Count == 0)
            {
                Debug.Log("[PhoneSystem] Dialogue finished");
                if (nextButton) nextButton.gameObject.SetActive(false); // Ẩn nút đi
                return;
            }
            
            DialogueLine line = dialogueQueue.Dequeue();
            
            // Nếu đây là câu cuối cùng, cũng có thể ẩn nút luôn
            // Nhưng tốt nhất là để user bấm thêm 1 lần để biết đã hết (hoặc sửa tuỳ ý)
            
            // Update dialogue content text
            if (dialogueContentText != null)
            {
                if (typingCoroutine != null) StopCoroutine(typingCoroutine);
                typingCoroutine = StartCoroutine(TypeDialogue(line.content));
            }
        }
        
        /// <summary>
        /// Type dialogue with typewriter effect
        /// </summary>
        private IEnumerator TypeDialogue(string text)
        {
            isTyping = true;
            dialogueContentText.text = "";
            
            foreach (char letter in text.ToCharArray())
            {
                if (!isTyping) // Allow interruption
                {
                    dialogueContentText.text = text; // Show full text
                    yield break;
                }
                dialogueContentText.text += letter;
                yield return new WaitForSeconds(0.02f);
            }
            
            isTyping = false;
        }

        /// <summary>
        /// Update call duration timer
        /// </summary>
        private IEnumerator UpdateCallTimer()
        {
            while (isCallActive)
            {
                callDuration += Time.deltaTime;
                
                int minutes = Mathf.FloorToInt(callDuration / 60f);
                int seconds = Mathf.FloorToInt(callDuration % 60f);
                
                if (callDurationText)
                    callDurationText.text = $"{minutes:00}:{seconds:00}";
                
                yield return null;
            }
        }

        /// <summary>
        /// Check if currently on a call
        /// </summary>
        public bool IsOnCall()
        {
            return isCallActive;
        }

        /// <summary>
        /// Get current call type
        /// </summary>
        public CallType GetCurrentCallType()
        {
            return currentCall != null ? currentCall.callType : CallType.Friend;
        }
    }
}
