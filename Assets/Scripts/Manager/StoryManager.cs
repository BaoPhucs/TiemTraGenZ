using UnityEngine;

namespace TiemTraGenZ.Manager
{
    public enum GameEnding
    {
        None,
        BadEnding_OfficeWorker,      // Về làm văn phòng
        NormalEnding_Franchise,      // Chuỗi nhượng quyền
        TrueEnding_CulturalHeritage  // Di sản văn hóa
    }

    public class StoryManager : MonoBehaviour
    {
        public static StoryManager Instance { get; private set; }

        public GameEnding CurrentEnding { get; private set; } = GameEnding.None;

        [Header("Story Stats")]
        [Tooltip("Vốn")]
        public float capital = 1000f;
        
        [Tooltip("Tình Làng Nghĩa Xóm")]
        public float neighborRelation = 50f;
        
        [Tooltip("Độ Viral")]
        public float viralScore = 0f;

        [Header("Game Progress")]
        public int currentDay = 1;
        public int maxDays = 90;

        [Header("Ending Configuration")]
        public float minCapitalForNormal = 5000f;
        public float minRelationForTrue = 80f;
        public float minViralForTrue = 1000f;

        // ──────────────────────────────────────────
        // Phone Call Assets — Mẹ
        // ──────────────────────────────────────────
        [Header("Calls — Mẹ")]
        [SerializeField] private Data.CallDialogueData momDay1Call;
        [SerializeField] private Data.CallDialogueData momDay7Call;
        [SerializeField] private Data.CallDialogueData momDay30LowCapital;
        [SerializeField] private Data.CallDialogueData momDay30HighCapital;
        [SerializeField] private Data.CallDialogueData momLowRelation;
        [SerializeField] private Data.CallDialogueData momFinalCall;

        // ──────────────────────────────────────────
        // Phone Call Assets — Chủ Nợ (Anh Tài)
        // ──────────────────────────────────────────
        [Header("Calls — Chủ Nợ")]
        [SerializeField] private Data.CallDialogueData creditorWarn1;
        [SerializeField] private Data.CallDialogueData creditorWarning;
        [SerializeField] private Data.CallDialogueData creditorThreat;
        [SerializeField] private Data.CallDialogueData creditorAggressive;
        [SerializeField] private Data.CallDialogueData creditorSweet;

        // ──────────────────────────────────────────
        // Phone Call Assets — Bạn Bè (Hùng)
        // ──────────────────────────────────────────
        [Header("Calls — Bạn Bè (Hùng)")]
        [SerializeField] private Data.CallDialogueData friendOpening;
        [SerializeField] private Data.CallDialogueData friendViralMilestone;
        [SerializeField] private Data.CallDialogueData friendTrueEnding;
        [SerializeField] private Data.CallDialogueData[] friendRandomCallsA;
        [SerializeField] private Data.CallDialogueData[] friendRandomCallsB;

        // ──────────────────────────────────────────
        // Phone Call Assets — Endings (Ngày 90)
        // ──────────────────────────────────────────
        [Header("Calls — Endings (Ngày 90)")]
        [SerializeField] private Data.CallDialogueData badEndingCall;
        [SerializeField] private Data.CallDialogueData normalEndingCall;
        [SerializeField] private Data.CallDialogueData trueEndingCall;

        // ──────────────────────────────────────────
        // One-time trigger flags
        // ──────────────────────────────────────────
        private bool triggeredMomDay30 = false;
        private bool triggeredCreditorWarn1 = false;
        private bool triggeredCreditorDay20 = false;
        private bool triggeredCreditorDay30 = false;
        private bool triggeredCreditorDay45 = false;
        private bool triggeredFriendOpening = false;
        private bool triggeredViralMilestone = false;
        private bool triggeredMomLowRelation = false;

        // Random friend call cooldown (avoid spamming)
        private int lastFriendRandomDay = -99;
        private const int FriendRandomCooldownDays = 3;
        private const float FriendRandomChance = 0.10f;

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
            }
        }

        public void AddStat(float money, float relation, float viral)
        {
            capital += money;
            neighborRelation += relation;
            viralScore += viral;

            // Check stat-based triggers after any stat change
            CheckStatMilestoneTriggers();
        }

        public GameEnding CheckEnding()
        {
            if (capital < minCapitalForNormal)
                return GameEnding.BadEnding_OfficeWorker;

            if (capital >= minCapitalForNormal && neighborRelation < minRelationForTrue)
                return GameEnding.NormalEnding_Franchise;

            if (neighborRelation >= minRelationForTrue && viralScore >= minViralForTrue)
                return GameEnding.TrueEnding_CulturalHeritage;

            return GameEnding.NormalEnding_Franchise;
        }

        public void AdvanceDay()
        {
            if (currentDay >= maxDays) return; // Không cho phép vượt quá ngày Max (90)

            currentDay++;
            TriggerDailyEvents();
        }

        // ──────────────────────────────────────────────────────
        // Daily Event Logic
        // ──────────────────────────────────────────────────────
        private void TriggerDailyEvents()
        {
            Debug.Log($"[StoryManager] Day {currentDay} — checking phone events...");

            // ─── Mẹ ───────────────────────────────────────────
            if (currentDay == 1 && momDay1Call != null)
            {
                TriggerCall(momDay1Call);
                return;
            }

            if (currentDay == 7 && momDay7Call != null)
            {
                TriggerCall(momDay7Call);
                return;
            }

            if (currentDay == 30 && !triggeredMomDay30)
            {
                triggeredMomDay30 = true;
                if (capital < 2000f && momDay30LowCapital != null)
                    TriggerCall(momDay30LowCapital);
                else if (momDay30HighCapital != null)
                    TriggerCall(momDay30HighCapital);
                return;
            }

            if (currentDay == 60 && !triggeredMomLowRelation && neighborRelation < 30f && momLowRelation != null)
            {
                triggeredMomLowRelation = true;
                TriggerCall(momLowRelation);
                return;
            }

            if (currentDay == 89)
            {
                // True Ending hint from friend takes priority over mom's final call
                if (viralScore >= 1000f && friendTrueEnding != null)
                {
                    TriggerCall(friendTrueEnding);
                    return;
                }
                if (momFinalCall != null)
                {
                    TriggerCall(momFinalCall);
                    return;
                }
            }

            // ─── Chủ Nợ ──────────────────────────────────────
            if (currentDay == 10 && !triggeredCreditorWarn1 && creditorWarn1 != null)
            {
                triggeredCreditorWarn1 = true;
                TriggerCall(creditorWarn1);
                return;
            }

            if (currentDay == 20 && !triggeredCreditorDay20)
            {
                triggeredCreditorDay20 = true;
                if (capital < 1500f && creditorWarning != null)
                {
                    TriggerCall(creditorWarning);
                    return;
                }
            }

            if (currentDay == 30 && !triggeredCreditorDay30)
            {
                triggeredCreditorDay30 = true;
                if (capital < 1000f && creditorThreat != null)
                {
                    TriggerCall(creditorThreat);
                    return;
                }
                else if (capital >= 3000f && creditorSweet != null)
                {
                    TriggerCall(creditorSweet);
                    return;
                }
            }

            if (currentDay == 45 && !triggeredCreditorDay45 && capital < 500f && creditorAggressive != null)
            {
                triggeredCreditorDay45 = true;
                TriggerCall(creditorAggressive);
                return;
            }

            // ─── Bạn Bè (Hùng) ────────────────────────────────
            if (currentDay == 3 && !triggeredFriendOpening && friendOpening != null)
            {
                triggeredFriendOpening = true;
                TriggerCall(friendOpening);
                return;
            }

            // Random friend calls — pool A (ngày 5–50)
            if (currentDay >= 5 && currentDay <= 50 && TryRandomFriendCall(friendRandomCallsA))
                return;

            // Random friend calls — pool B (ngày 20–70)
            if (currentDay >= 20 && currentDay <= 70 && TryRandomFriendCall(friendRandomCallsB))
                return;
        }

        // ────────────────────────────────────────────────────
        // Stat-based Milestone Triggers (called on AddStat)
        // ────────────────────────────────────────────────────
        private void CheckStatMilestoneTriggers()
        {
            if (!triggeredViralMilestone && viralScore >= 100f && friendViralMilestone != null)
            {
                triggeredViralMilestone = true;
                Debug.Log("[StoryManager] Viral milestone reached — triggering friend call");
                TriggerCall(friendViralMilestone);
            }
        }

        // ────────────────────────────────────────────────────
        // Helpers
        // ────────────────────────────────────────────────────
        private bool TryRandomFriendCall(Data.CallDialogueData[] pool)
        {
            if (pool == null || pool.Length == 0) return false;
            if (currentDay - lastFriendRandomDay < FriendRandomCooldownDays) return false;
            if (Random.value > FriendRandomChance) return false;

            var call = pool[Random.Range(0, pool.Length)];
            if (call == null) return false;

            lastFriendRandomDay = currentDay;
            TriggerCall(call);
            return true;
        }

        private void TriggerCall(Data.CallDialogueData callData)
        {
            if (PhoneSystem.Instance == null)
            {
                Debug.LogWarning("[StoryManager] PhoneSystem.Instance is null — cannot trigger call.");
                return;
            }
            if (PhoneSystem.Instance.IsOnCall())
            {
                Debug.LogWarning("[StoryManager] Already on a call — skipping.");
                return;
            }
            Debug.Log($"[StoryManager] Triggering call: {callData.name}");
            PhoneSystem.Instance.TriggerCall(callData);
        }

        public void TriggerEnding(GameEnding ending)
        {
            Debug.Log($"[StoryManager] Game Over! Triggering Ending: {ending}");
            CurrentEnding = ending;
            
            switch (ending)
            {
                case GameEnding.BadEnding_OfficeWorker:
                    if (badEndingCall != null) TriggerCall(badEndingCall);
                    break;
                case GameEnding.NormalEnding_Franchise:
                    if (normalEndingCall != null) TriggerCall(normalEndingCall);
                    break;
                case GameEnding.TrueEnding_CulturalHeritage:
                    if (trueEndingCall != null) TriggerCall(trueEndingCall);
                    break;
            }
        }
    }
}
