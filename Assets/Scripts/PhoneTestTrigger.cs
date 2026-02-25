using UnityEngine;
using TiemTraGenZ.Manager;
using TiemTraGenZ.Data;

public class PhoneTestTrigger : MonoBehaviour
{
    [Header("Test Call Data")]
    [SerializeField] private CallDialogueData testCall;
    
    [Header("Test Controls")]
    [Tooltip("Press T to trigger test call")]
    private KeyCode testKey = KeyCode.T;
    
    void Update()
    {
        // Press T to trigger test call
        if (Input.GetKeyDown(testKey))
        {
            TriggerTestCall();
        }
    }
    
    private void TriggerTestCall()
    {
        if (PhoneSystem.Instance == null)
        {
            Debug.LogError("[PhoneTestTrigger] PhoneSystem.Instance not found!");
            return;
        }
        
        if (testCall == null)
        {
            Debug.LogError("[PhoneTestTrigger] Test Call Data not assigned!");
            return;
        }
        
        Debug.Log($"[PhoneTestTrigger] Triggering test call: {testCall.callerName}");
        PhoneSystem.Instance.TriggerCall(testCall);
    }
}
