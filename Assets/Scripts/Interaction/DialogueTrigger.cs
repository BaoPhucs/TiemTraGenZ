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
        public float interactDistance = 2.5f; // Bán kính đo khoảng cách (mét)
        public bool triggerOnEnter = false;
        public bool triggerOnInteract = true;
        public bool triggerOnStart = false;
        public string playerTag = "Player";

        private Transform playerTransform;
        private bool wasInRange = false; // Ghi nhớ trạng thái để không spam

        private void Start()
        {
            // Tự động radar dò tìm Minh ngay khi vào game
            GameObject playerObj = GameObject.FindGameObjectWithTag(playerTag);
            if (playerObj != null)
            {
                playerTransform = playerObj.transform;
            }

            if (triggerOnStart)
            {
                TriggerDialogue();
            }
        }

        private void Update()
        {
            if (playerTransform == null) return;

            // Đo khoảng cách tuyệt đối từ NPC đến người chơi
            float distance = Vector3.Distance(transform.position, playerTransform.position);
            bool isCurrentlyInRange = distance <= interactDistance;

            // XỬ LÝ KHI NGƯỜI CHƠI ĐỨNG GẦN NPC
            if (isCurrentlyInRange)
            {
                // NẾU LÀ BƯỚC VÀO LẦN ĐẦU TIÊN
                if (!wasInRange)
                {
                    wasInRange = true;
                    if (triggerOnEnter)
                    {
                        Debug.Log("[DialogueTrigger] Auto-triggering dialogue on enter...");
                        TriggerDialogue();
                    }
                }

                // NẾU BẤM PHÍM E
                if (triggerOnInteract && Input.GetKeyDown(KeyCode.E))
                {
                    Debug.Log("[DialogueTrigger] Player pressed E, triggering dialogue...");
                    TriggerDialogue();
                }
            }
            // XỬ LÝ KHI NGƯỜI CHƠI ĐI RA XA
            else
            {
                if (wasInRange)
                {
                    wasInRange = false;
                    Debug.Log("[DialogueTrigger] Player exited trigger zone.");
                }
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