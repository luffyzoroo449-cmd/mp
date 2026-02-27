using UnityEngine;

namespace ShadowRace.UI
{
    public class TallyScreenManager : MonoBehaviour
    {
        public static TallyScreenManager Instance { get; private set; }

        [Header("UI Panels")]
        public GameObject tallyPanel;
        // TextMeshProUGUI references would go here (Base XP, Base Money, Time Bonus, Combo Bonus, Untouchable Bonus, Total)

        [Header("Level Stats")]
        private float levelStartTime;
        private float currentParTime = 120f; // 2 minutes par
        private bool hasTakenDamage = false;

        private void Awake()
        {
            if (Instance == null) Instance = this;
            else Destroy(gameObject);
        }

        public void StartLevelTally(float parTime)
        {
            levelStartTime = Time.time;
            currentParTime = parTime;
            hasTakenDamage = false;
        }

        public void PlayerTookDamage()
        {
            hasTakenDamage = true;
        }

        public void ShowTallyScreen(int baseXP, int baseMoney)
        {
            // Pause Game
            Time.timeScale = 0f;
            tallyPanel.SetActive(true);

            float timeTaken = Time.time - levelStartTime;
            
            // Calculate Multipliers
            float xpMultiplier = 1f;
            float moneyMultiplier = 1f;

            bool speedDemon = timeTaken <= currentParTime;
            bool untouchable = !hasTakenDamage;
            int maxCombo = ShadowRace.Player.ComboTracker.Instance != null ? ShadowRace.Player.ComboTracker.Instance.highestComboThisLevel : 0;
            bool executioner = maxCombo >= 20;

            if (speedDemon) moneyMultiplier += 0.2f; // 20% more money
            if (untouchable) xpMultiplier += 0.5f; // 50% more XP
            if (executioner) 
            {
                xpMultiplier += 0.1f;
                moneyMultiplier += 0.1f;
            }

            int finalXP = Mathf.RoundToInt(baseXP * xpMultiplier);
            int finalMoney = Mathf.RoundToInt(baseMoney * moneyMultiplier);

            // In a real implementation we would run a Coroutine to "count up" the numbers visually on the UI here.
            Debug.Log($"--- LEVEL CLEARED ---");
            Debug.Log($"Speed Demon: {speedDemon}, Untouchable: {untouchable}, Executioner: {executioner}");
            Debug.Log($"Total XP: {finalXP}, Total Money: {finalMoney}");

            // Apply to PlayerStats
            GameObject player = GameObject.FindGameObjectWithTag("Player");
            if (player != null)
            {
                PlayerStats stats = player.GetComponent<PlayerStats>();
                if (stats != null)
                {
                    stats.GainXP(finalXP);
                    stats.money += finalMoney;
                }
            }
        }
    }
}
