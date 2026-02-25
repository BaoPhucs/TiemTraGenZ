using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections;
using System.Collections.Generic;
using TiemTraGenZ.Data;

namespace TiemTraGenZ.Manager
{
    public class DialogueManager : MonoBehaviour
    {
        public static DialogueManager Instance { get; private set; }

        [Header("UI Components")]
        [SerializeField] private GameObject dialoguePanel;
        [SerializeField] private Image avatarImage;
        [SerializeField] private TextMeshProUGUI nameText;
        [SerializeField] private TextMeshProUGUI contentText;
        [SerializeField] private Button nextButton;
        [SerializeField] private Button skipButton;

        [Header("Settings")]
        [SerializeField] private float typingSpeed = 0.05f;

        private Queue<DialogueLine> sentences;
        private bool isTyping = false;
        private string currentFullSentence = "";
        private Coroutine typingCoroutine;

        private void Awake()
        {
            if (Instance == null)
            {
                Instance = this;
            }
            else
            {
                Destroy(gameObject);
            }

            sentences = new Queue<DialogueLine>();
            
            if (nextButton != null)
            {
                nextButton.onClick.AddListener(DisplayNextSentence);
            }
            
            if (skipButton != null)
            {
                skipButton.onClick.AddListener(SkipDialogue);
            }
        }
        
        private void Update()
        {
            if (dialoguePanel != null && dialoguePanel.activeSelf)
            {
                // B key for Next
                if (Input.GetKeyDown(KeyCode.B))
                {
                    DisplayNextSentence();
                }
                
                // C key for Skip
                if (Input.GetKeyDown(KeyCode.C))
                {
                    SkipDialogue();
                }
            }
        }

        private void Start()
        {
            // Hide panel on start by default
            if (dialoguePanel != null) dialoguePanel.SetActive(false);
        }

        public void StartDialogue(DialogueData dialogue)
        {
            Debug.Log($"[DialogueManager] StartDialogue called! Panel null? {dialoguePanel == null}");
            if (dialoguePanel == null) return;

            Debug.Log($"[DialogueManager] Activating panel... Lines count: {dialogue.lines.Count}");
            dialoguePanel.SetActive(true);
            sentences.Clear();

            foreach (DialogueLine line in dialogue.lines)
            {
                sentences.Enqueue(line);
            }

            Debug.Log($"[DialogueManager] Calling DisplayNextSentence...");
            DisplayNextSentence();
        }

        public void DisplayNextSentence()
        {
            Debug.Log($"[DialogueManager] DisplayNextSentence called! isTyping: {isTyping}, sentences.Count: {sentences.Count}");
            
            // If typing, click meant "skip to end"
            if (isTyping)
            {
                StopCoroutine(typingCoroutine);
                contentText.text = currentFullSentence;
                isTyping = false;
                return;
            }

            if (sentences.Count == 0)
            {
                Debug.Log("[DialogueManager] No more sentences, ending dialogue...");
                EndDialogue();
                return;
            }

            DialogueLine line = sentences.Dequeue();
            Debug.Log($"[DialogueManager] Displaying line - Speaker: {line.speakerName}, Content: {line.content}");
            
            // Update UI
            if (nameText != null) nameText.text = line.speakerName;
            if (avatarImage != null)
            {
                avatarImage.sprite = line.speakerAvatar;
                avatarImage.gameObject.SetActive(line.speakerAvatar != null);
            }

            Debug.Log($"[DialogueManager] Panel active? {dialoguePanel.activeSelf}, NameText: {nameText?.text}, ContentText will type...");
            
            currentFullSentence = line.content;
            if (typingCoroutine != null) StopCoroutine(typingCoroutine);
            typingCoroutine = StartCoroutine(TypeSentence(line.content, line.typingSpeedMultiplier));
        }

        IEnumerator TypeSentence(string sentence, float multiplier)
        {
            isTyping = true;
            contentText.text = "";
            float speed = typingSpeed / (multiplier > 0 ? multiplier : 1f);

            foreach (char letter in sentence.ToCharArray())
            {
                contentText.text += letter;
                yield return new WaitForSeconds(speed);
            }

            isTyping = false;
        }

        public void SkipDialogue()
        {
            Debug.Log("[DialogueManager] Skipping entire dialogue...");
            sentences.Clear();
            if (typingCoroutine != null) StopCoroutine(typingCoroutine);
            EndDialogue();
        }
        
        public void EndDialogue()
        {
            if (dialoguePanel != null) dialoguePanel.SetActive(false);
            Debug.Log("End of conversation.");
        }
    }
}
