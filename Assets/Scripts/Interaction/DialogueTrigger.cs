using UnityEngine;
using TiemTraGenZ.Data;
using TiemTraGenZ.Manager;

namespace TiemTraGenZ.Interaction
{
    public class DialogueTrigger : MonoBehaviour
    {
        [Header("Dialogue Content")]
        public DialogueData dialogue;

        [Header("Trigger Settings")]
        public bool triggerOnEnter = false;
        public bool triggerOnInteract = true;
        public bool triggerOnStart = false; // Add this for testing!
        public string playerTag = "Player";

        private bool isPlayerInRange = false;

        private void Start()
        {
            if (triggerOnStart)
            {
                TriggerDialogue();
            }
        }

        private void Update()
        {
            if (isPlayerInRange && triggerOnInteract && Input.GetKeyDown(KeyCode.E))
            {
                Debug.Log("[DialogueTrigger] Player pressed E, triggering dialogue...");
                TriggerDialogue();
            }
        }

        private void OnTriggerEnter(Collider other)
        {
            Debug.Log($"[DialogueTrigger] OnTriggerEnter: {other.name} (Tag: {other.tag})");
            if (other.CompareTag(playerTag))
            {
                Debug.Log("[DialogueTrigger] Player entered trigger zone!");
                isPlayerInRange = true;
                if (triggerOnEnter)
                {
                    Debug.Log("[DialogueTrigger] Auto-triggering dialogue on enter...");
                    TriggerDialogue();
                }
            }
        }

        private void OnTriggerExit(Collider other)
        {
            if (other.CompareTag(playerTag))
            {
                Debug.Log("[DialogueTrigger] Player exited trigger zone.");
                isPlayerInRange = false;
            }
        }

        public void TriggerDialogue()
        {
            Debug.Log("[DialogueTrigger] TriggerDialogue() called!");
            if (DialogueManager.Instance != null && dialogue != null)
            {
                Debug.Log($"[DialogueTrigger] Starting dialogue: {dialogue.name}");
                DialogueManager.Instance.StartDialogue(dialogue);
            }
            else
            {
                Debug.LogWarning($"[DialogueTrigger] Missing! Manager: {DialogueManager.Instance != null}, Data: {dialogue != null}");
            }
        }
    }
}
