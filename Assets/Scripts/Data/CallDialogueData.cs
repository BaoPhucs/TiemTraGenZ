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
        
        [Header("Dialogue Content")]
        public DialogueData dialogue;
        
        [Header("Audio")]
        public AudioClip ringtone;
    }
    
    public enum CallType
    {
        Mom,
        Creditor,
        Friend,
        Special
    }
}
