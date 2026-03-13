using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using TMPro;
using TiemTraGenZ.Data;
using TiemTraGenZ.Player;

namespace TiemTraGenZ.Manager
{
    public class PhoneSystem : MonoBehaviour
    {
        public static PhoneSystem Instance { get; private set; }

        public Animator playerAnimator;

        [Header("--- UI COMPONENT ---")]
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
        [SerializeField] private AudioSource voiceAudioSource; // THÊM MỚI: Loa phát giọng nói

        [Header("References")]
        [SerializeField] private PlayerAnimationController playerAnimationController;

        private CallDialogueData currentCall;
        private bool isCallActive = false;
        private float callDuration = 0f;

        private Coroutine callTimerCoroutine;
        private Coroutine autoHangUpCoroutine;

        private void Awake()
        {
            if (Instance == null)
            {
                Instance = this;
                DontDestroyOnLoad(gameObject);
            }
            else
            {
                Destroy(gameObject);
                return;
            }

            if (acceptButton) acceptButton.onClick.AddListener(AcceptCall);
            if (rejectButton) rejectButton.onClick.AddListener(RejectCall);
            if (hangUpButton) hangUpButton.onClick.AddListener(HangUpCall);
        }

        private void Start()
        {
            if (phonePanel) phonePanel.SetActive(false);
            if (incomingCallView) incomingCallView.SetActive(false);
            if (inCallView) inCallView.SetActive(false);

            if (playerAnimationController == null)
            {
                playerAnimationController = FindObjectOfType<PlayerAnimationController>();
            }
        }

        private void Update()
        {
            if (incomingCallView != null && incomingCallView.activeSelf)
            {
                if (Input.GetKeyDown(KeyCode.B))
                {
                    AcceptCall();
                    return;
                }
                if (Input.GetKeyDown(KeyCode.C))
                {
                    RejectCall();
                    return;
                }
            }

            // Đang trong cuộc gọi thì chỉ cho phép bấm C để cúp máy ngang
            if (isCallActive)
            {
                if (Input.GetKeyDown(KeyCode.C))
                    HangUpCall();
            }
        }

        public void TriggerCall(CallDialogueData callData)
        {
            if (callData == null) return;
            currentCall = callData;
            ShowIncomingCall();
        }

        private void ShowIncomingCall()
        {
            if (phonePanel) phonePanel.SetActive(true);
            if (incomingCallView) incomingCallView.SetActive(true);
            if (inCallView) inCallView.SetActive(false);

            if (callerAvatar) callerAvatar.sprite = currentCall.callerAvatar;
            if (callerNameText) callerNameText.text = currentCall.callerName;
            if (statusText) statusText.text = "Đang gọi...";

            if (ringtoneAudio && currentCall.ringtone)
            {
                ringtoneAudio.clip = currentCall.ringtone;
                ringtoneAudio.loop = true;
                ringtoneAudio.Play();
            }
        }

        public void AcceptCall()
        {
            if (ringtoneAudio) ringtoneAudio.Stop();

            if (incomingCallView) incomingCallView.SetActive(false);
            if (inCallView) inCallView.SetActive(true);

            if (callerAvatarSmall) callerAvatarSmall.sprite = currentCall.callerAvatar;
            if (callerNameInCall) callerNameInCall.text = currentCall.callerName;

            // ẨN GIAO DIỆN TEXT CŨ (Chữ và nút Tiếp tục)
            if (dialogueContentText != null) dialogueContentText.gameObject.SetActive(false);
            if (nextButton != null) nextButton.gameObject.SetActive(false);

            isCallActive = true;
            callDuration = 0f;
            if (callTimerCoroutine != null) StopCoroutine(callTimerCoroutine);
            callTimerCoroutine = StartCoroutine(UpdateCallTimer());

            if (playerAnimationController)
                playerAnimationController.StartPhoneAnimation();

            // PHÉP MÀU: PHÁT GIỌNG NÓI
            if (voiceAudioSource != null && currentCall.voiceAudio != null)
            {
                voiceAudioSource.clip = currentCall.voiceAudio;
                voiceAudioSource.Play();

                // Hẹn giờ tự động cúp máy khi audio chạy xong
                if (autoHangUpCoroutine != null) StopCoroutine(autoHangUpCoroutine);
                autoHangUpCoroutine = StartCoroutine(AutoHangUpAfterVoice(currentCall.voiceAudio.length));
            }
        }

        private IEnumerator AutoHangUpAfterVoice(float duration)
        {
            yield return new WaitForSeconds(duration);
            if (isCallActive) // Nếu sếp chưa bấm C cúp ngang thì hệ thống tự cúp
            {
                HangUpCall();
            }
        }

        public void RejectCall()
        {
            if (ringtoneAudio) ringtoneAudio.Stop();
            if (phonePanel) phonePanel.SetActive(false);
            if (incomingCallView) incomingCallView.SetActive(false);
            currentCall = null;

            // --- THÊM MỚI Ở ĐÂY: BẤM C TỪ CHỐI VẪN HIỆN HƯỚNG DẪN ---
            if (TutorialManager.DaHuongDanBaTu == false)
            {
                if (TutorialManager.Instance != null)
                {
                    TutorialManager.Instance.ShowTutorial();
                }
            }
        }

        public void HangUpCall()
        {
            if (callTimerCoroutine != null) StopCoroutine(callTimerCoroutine);
            if (autoHangUpCoroutine != null) StopCoroutine(autoHangUpCoroutine);

            if (voiceAudioSource != null) voiceAudioSource.Stop();

            isCallActive = false;
            currentCall = null;

            if (phonePanel) phonePanel.SetActive(false);
            if (incomingCallView) incomingCallView.SetActive(false);
            if (inCallView) inCallView.SetActive(false);

            if (playerAnimationController)
            {
                playerAnimationController.StopPhoneAnimation();
            }

            if (StoryManager.Instance != null && StoryManager.Instance.CurrentEnding != GameEnding.None)
            {
                var endingUI = FindObjectOfType<UI_EndingManager>(true);
                if (endingUI != null) endingUI.ShowEnding(StoryManager.Instance.CurrentEnding);
            }

            if (TutorialManager.DaHuongDanBaTu == false)
            {
                if (TutorialManager.Instance != null)
                {
                    TutorialManager.Instance.ShowTutorial();
                }
            }
        }

        private IEnumerator UpdateCallTimer()
        {
            while (isCallActive)
            {
                callDuration += Time.deltaTime;
                int minutes = Mathf.FloorToInt(callDuration / 60f);
                int seconds = Mathf.FloorToInt(callDuration % 60f);
                if (callDurationText) callDurationText.text = $"{minutes:00}:{seconds:00}";
                yield return null;
            }
        }

        public bool IsOnCall() => isCallActive;
        public CallType GetCurrentCallType() => currentCall != null ? currentCall.callType : CallType.Friend;
    }
}