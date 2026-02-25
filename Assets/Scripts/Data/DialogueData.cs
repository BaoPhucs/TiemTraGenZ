using UnityEngine;
using System.Collections.Generic;

namespace TiemTraGenZ.Data
{
    [CreateAssetMenu(fileName = "NewDialogue", menuName = "TiemTraGenZ/Dialogue Data")]
    public class DialogueData : ScriptableObject
    {
        [Header("Conversation Config")]
        public string conversationID;
        public DialogueData nextConversation; // Optional: Chain into the next conversation

        [Header("Dialogue Lines")]
        public List<DialogueLine> lines;
    }

    [System.Serializable]
    public struct DialogueLine
    {
        public string speakerName;
        public Sprite speakerAvatar;
        [TextArea(3, 10)]
        public string content;
        public float typingSpeedMultiplier; // 1.0f is default
    }
}
