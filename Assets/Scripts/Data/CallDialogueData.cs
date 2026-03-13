using UnityEngine;

namespace TiemTraGenZ.Data
{
    [CreateAssetMenu(fileName = "NewCall", menuName = "TiemTraGenZ/Call Data")]
    public class CallDialogueData : ScriptableObject
    {
        [Header("Caller Information")]
        public string callerName;
        public Sprite callerAvatar;

        [Header("Call Type")]
        public CallType callType;

        [Header("Dialogue Content (ĐÃ BỎ TEXT)")]
        public DialogueData dialogue; // Cứ để đây cho khỏi báo lỗi các file cũ

        [Header("Audio (KÉO FILE MP3 VÀO ĐÂY)")]
        public AudioClip ringtone;
        public AudioClip voiceAudio; // THÊM MỚI: Dành cho giọng nói mp3
    }

    public enum CallType
    {
        Mom, Creditor, Friend, Special
    }
}