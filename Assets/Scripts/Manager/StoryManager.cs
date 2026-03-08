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

        [Header("Story Stats (TỰ ĐỘNG ĐỒNG BỘ TỪ KHO)")]
        [Tooltip("Vốn (Tự động lấy từ QuanLyKho)")]
        public float capital = 0f;

        [Tooltip("Tình Làng Nghĩa Xóm")]
        public float neighborRelation = 50f;

        [Tooltip("Độ Viral (Tự động lấy từ QuanLyKho)")]
        public float viralScore = 0f;

        [Header("Game Progress")]
        public int currentDay = 1;
        public int maxDays = 90;

        [Header("Ending Configuration")]
        public float minCapitalForNormal = 500000f;
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

        // =======================================================
        // ĐÂY LÀ TRÁI TIM CỦA VIỆC ĐỒNG BỘ: CẬP NHẬT MỖI FRAME
        // =======================================================
        private void Update()
        {
            if (QuanLyKho.Instance != null)
            {
                // Bắt StoryManager luôn luôn lấy điểm thực tế từ Kho
                capital = QuanLyKho.Instance.TienHienCo;
                viralScore = QuanLyKho.Instance.DiemViral;
                neighborRelation = QuanLyKho.Instance.DiemTinhLang;
            }

            // Liên tục kiểm tra xem điểm Viral đã đủ mốc để Hùng gọi chưa
            CheckStatMilestoneTriggers();
        }

        public void AddStat(float money, float relation, float viral)
        {
            // Nếu có hàm nào lỡ gọi AddStat, nó sẽ đẩy ngược tiền và viral vào Kho để lưu lại
            if (QuanLyKho.Instance != null)
            {
                QuanLyKho.Instance.TienHienCo += (int)money;
                QuanLyKho.Instance.DiemViral += (int)viral;
                QuanLyKho.Instance.DiemTinhLang += (int)relation;
                QuanLyKho.Instance.SaveGame();
            }
            neighborRelation += relation;
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
            if (currentDay >= maxDays) return;

            currentDay++;
            TriggerDailyEvents();
        }

        private void TriggerDailyEvents()
        {
            Debug.Log($"[StoryManager] Day {currentDay} — checking phone events...");

            // (Các logic TriggerCall của bạn được giữ nguyên không thay đổi)
            if (currentDay == 1 && momDay1Call != null) { TriggerCall(momDay1Call); return; }
            if (currentDay == 7 && momDay7Call != null) { TriggerCall(momDay7Call); return; }

            if (currentDay == 30 && !triggeredMomDay30)
            {
                triggeredMomDay30 = true;
                if (capital < 40000f && momDay30LowCapital != null) TriggerCall(momDay30LowCapital);
                else if (momDay30HighCapital != null) TriggerCall(momDay30HighCapital);
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
                if (viralScore >= 1000f && friendTrueEnding != null) { TriggerCall(friendTrueEnding); return; }
                if (momFinalCall != null) { TriggerCall(momFinalCall); return; }
            }

            if (currentDay == 10 && !triggeredCreditorWarn1 && creditorWarn1 != null) { triggeredCreditorWarn1 = true; TriggerCall(creditorWarn1); return; }

            if (currentDay == 20 && !triggeredCreditorDay20)
            {
                triggeredCreditorDay20 = true;
                if (capital < 30000f && creditorWarning != null) { TriggerCall(creditorWarning); return; }
            }

            if (currentDay == 30 && !triggeredCreditorDay30)
            {
                triggeredCreditorDay30 = true;
                if (capital < 20000f && creditorThreat != null) { TriggerCall(creditorThreat); return; }
                else if (capital >= 300000f && creditorSweet != null) { TriggerCall(creditorSweet); return; }
            }

            if (currentDay == 45 && !triggeredCreditorDay45 && capital < 10000f && creditorAggressive != null)
            {
                triggeredCreditorDay45 = true;
                TriggerCall(creditorAggressive);
                return;
            }

            if (currentDay == 3 && !triggeredFriendOpening && friendOpening != null) { triggeredFriendOpening = true; TriggerCall(friendOpening); return; }

            if (currentDay >= 5 && currentDay <= 50 && TryRandomFriendCall(friendRandomCallsA)) return;
            if (currentDay >= 20 && currentDay <= 70 && TryRandomFriendCall(friendRandomCallsB)) return;
        }

        private void CheckStatMilestoneTriggers()
        {
            if (!triggeredViralMilestone && viralScore >= 100f && friendViralMilestone != null)
            {
                triggeredViralMilestone = true;
                Debug.Log("[StoryManager] Đã đạt mốc 100 Viral — Kích hoạt cuộc gọi của bạn thân!");
                TriggerCall(friendViralMilestone);
            }
        }

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
            if (PhoneSystem.Instance == null) return;
            if (PhoneSystem.Instance.IsOnCall()) return;
            PhoneSystem.Instance.TriggerCall(callData);
        }

        public void TriggerEnding(GameEnding ending)
        {
            CurrentEnding = ending;
            switch (ending)
            {
                case GameEnding.BadEnding_OfficeWorker: if (badEndingCall != null) TriggerCall(badEndingCall); break;
                case GameEnding.NormalEnding_Franchise: if (normalEndingCall != null) TriggerCall(normalEndingCall); break;
                case GameEnding.TrueEnding_CulturalHeritage: if (trueEndingCall != null) TriggerCall(trueEndingCall); break;
            }
        }
    }
}