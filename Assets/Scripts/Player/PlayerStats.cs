using UnityEngine;

namespace ShadowRace.Player
{
    public class PlayerStats : MonoBehaviour
    {
        [Header("Health")]
        public float maxHP = 100f;
        public float currentHP;

        [Header("Energy/Magic")]
        public float maxMP = 50f;
        public float currentMP;

        [Header("Stamina")]
        public float maxStamina = 100f;
        public float currentStamina;

        [Header("Progression")]
        public int level = 1;
        public float currentXP = 0f;
        public float xpToNextLevel = 100f;
        public int money = 0;

        [Header("Combat Stats")]
        public float defense = 10f;
        [Range(0f, 100f)]
        public float criticalChance = 5f;

        private void Awake()
        {
            currentHP = maxHP;
            currentMP = maxMP;
            currentStamina = maxStamina;
        }

        public void TakeDamage(float amount)
        {
            float actualDamage = Mathf.Max(1, amount - (defense * 0.1f));
            currentHP = Mathf.Clamp(currentHP - actualDamage, 0, maxHP);
            
            // Register for end-of-level multiplier penalty
            if (ShadowRace.UI.TallyScreenManager.Instance != null && actualDamage > 0)
            {
                ShadowRace.UI.TallyScreenManager.Instance.PlayerTookDamage();
            }

            if (currentHP <= 0)
            {
                Die();
            }
        }

        public void Heal(float amount)
        {
            currentHP = Mathf.Clamp(currentHP + amount, 0, maxHP);
        }

        public void GainXP(float amount)
        {
            currentXP += amount;
            if (currentXP >= xpToNextLevel)
            {
                LevelUp();
            }
        }

        private void LevelUp()
        {
            level++;
            currentXP -= xpToNextLevel;
            xpToNextLevel *= 1.5f; // Scale next level requirement
            
            // Basic stat increases on level up
            maxHP += 10;
            maxMP += 5;
            defense += 2;
            
            currentHP = maxHP;
            currentMP = maxMP;
            
            Debug.Log($"Level Up! Now Level {level}");
        }

        private void Die()
        {
            Debug.Log("Player Died!");
            
            // Adjust Dynamic Difficulty Global Variables
            if (ShadowRace.Core.DifficultyManager.Instance != null)
            {
                ShadowRace.Core.DifficultyManager.Instance.RegisterPlayerDeath();
            }

            // Trigger death event/animation
        }
    }
}
