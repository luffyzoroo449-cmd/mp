using UnityEngine;
using ShadowRace.Core;
using ShadowRace.AI;

namespace ShadowRace.Quests
{
    public class HiddenQuestBossSpawner : MonoBehaviour
    {
        [Header("Spawner Rules")]
        [Tooltip("The designated level (multiple of 5) that this boss appears in.")]
        public int targetLevel = 5;
        public GameObject legendaryBossPrefab;
        public Transform spawnPoint;

        private void Start()
        {
            CheckAndSpawn();
        }

        private void CheckAndSpawn()
        {
            if (GameManager.Instance == null) return;

            // PRD Rule: "Hidden Quest Boss: Unlocked every 5 levels. Very high difficulty."
            if (GameManager.Instance.currentLevel == targetLevel && (targetLevel % 5 == 0))
            {
                Debug.Log($"Hidden Quest Boss Requirements Met for Level {targetLevel}! Spawning Legendary Boss.");
                
                if (legendaryBossPrefab != null && spawnPoint != null)
                {
                    GameObject bossObj = Instantiate(legendaryBossPrefab, spawnPoint.position, spawnPoint.rotation);
                    BossAI legendaryBoss = bossObj.GetComponent<BossAI>();
                    
                    if (legendaryBoss != null)
                    {
                        // Overclock stats to ensure "Very high difficulty"
                        legendaryBoss.maxHP *= 3f; 
                        legendaryBoss.currentHP = legendaryBoss.maxHP;
                        legendaryBoss.attackRange *= 1.2f;
                        legendaryBoss.skillCooldown *= 0.5f; // Fires skills twice as fast
                    }
                }
            }
            else
            {
                gameObject.SetActive(false); // Disable this spawner if criteria isn't met
            }
        }
    }
}
