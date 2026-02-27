using UnityEngine;

namespace ShadowRace.Core
{
    public class DifficultyManager : MonoBehaviour
    {
        public static DifficultyManager Instance { get; private set; }

        [Header("Global Modifiers")]
        public float globalEnemyDamageMultiplier = 1f;
        public float globalEnemyAttackSpeedMultiplier = 1f;
        public float eliteSpawnRate = 0.05f; // Base 5%

        [Header("Session Tracking")]
        private int deathsThisLevel = 0;
        private int consecutiveUntouchableRuns = 0;

        private void Awake()
        {
            if (Instance == null) Instance = this;
            else Destroy(gameObject);
        }

        public void RegisterPlayerDeath()
        {
            deathsThisLevel++;
            consecutiveUntouchableRuns = 0; // Reset streak
            RecalculateDifficulty();
        }

        public void RegisterLevelCompletion(bool tookDamage)
        {
            if (!tookDamage)
            {
                consecutiveUntouchableRuns++;
            }
            else
            {
                consecutiveUntouchableRuns = 0;
            }

            // Reset deaths per level
            deathsThisLevel = 0;
            RecalculateDifficulty();
        }

        private void RecalculateDifficulty()
        {
            // Reset to base
            globalEnemyDamageMultiplier = 1f;
            globalEnemyAttackSpeedMultiplier = 1f;
            eliteSpawnRate = 0.05f;

            // Punitive Scaling (Make it easier if failing)
            if (deathsThisLevel >= 5)
            {
                globalEnemyAttackSpeedMultiplier = 0.9f; // Slower attacks
                globalEnemyDamageMultiplier = 0.85f; // Less hitting power
                Debug.Log("[Difficulty] Scaled Down (-15% damage, -10% speed).");
            }

            // Hardcore Scaling (Make it harder if crushing it)
            if (consecutiveUntouchableRuns >= 3)
            {
                eliteSpawnRate = 0.15f; // Triple elite spawn chance
                globalEnemyDamageMultiplier = 1.15f; // Higher risk
                Debug.Log("[Difficulty] Scaled Up (+15% damage, 15% Elite chance).");
            }
        }

        public bool RollForEliteSpawn()
        {
            return Random.value <= eliteSpawnRate;
        }
    }
}
