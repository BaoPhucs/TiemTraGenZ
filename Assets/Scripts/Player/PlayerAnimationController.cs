using UnityEngine;

namespace TiemTraGenZ.Player
{
    public class PlayerAnimationController : MonoBehaviour
    {
        [Header("References")]
        [SerializeField] private Animator animator;
        [SerializeField] private GameObject phoneModel; // IP 17 model
        
        [Header("Animation Parameters")]
        private const string IS_ON_PHONE = "isOnPhone";
        
        private void Awake()
        {
            // Auto-find if not assigned
            if (animator == null)
                animator = GetComponent<Animator>();
                
            // Phone model should be inactive by default
            if (phoneModel != null)
                phoneModel.SetActive(false);
        }
        
        /// <summary>
        /// Start phone call animation - show phone and trigger animation
        /// </summary>
        public void StartPhoneAnimation()
        {
            if (animator != null)
            {
                animator.SetBool(IS_ON_PHONE, true);
                Debug.Log("[PlayerAnimation] Started phone animation");
            }
            
            if (phoneModel != null)
            {
                phoneModel.SetActive(true);
                Debug.Log("[PlayerAnimation] Phone model visible");
            }
        }
        
        /// <summary>
        /// Stop phone call animation - hide phone and return to idle
        /// </summary>
        public void StopPhoneAnimation()
        {
            if (animator != null)
            {
                animator.SetBool(IS_ON_PHONE, false);
                Debug.Log("[PlayerAnimation] Stopped phone animation");
            }
            
            if (phoneModel != null)
            {
                phoneModel.SetActive(false);
                Debug.Log("[PlayerAnimation] Phone model hidden");
            }
        }
        
        /// <summary>
        /// Check if currently in phone animation
        /// </summary>
        public bool IsOnPhone()
        {
            if (animator != null)
                return animator.GetBool(IS_ON_PHONE);
            return false;
        }
    }
}
