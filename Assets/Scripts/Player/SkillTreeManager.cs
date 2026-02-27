using System.Collections.Generic;
using UnityEngine;

namespace ShadowRace.Player
{
    public class SkillTreeManager : MonoBehaviour
    {
        public PlayerStats playerStats;
        
        [Header("Available Skills")]
        public List<SkillNode> allSkills;
        
        [Header("Unlocked Skills")]
        public List<SkillNode> unlockedSkills = new List<SkillNode>();
        public int availableSkillPoints = 0;

        private void Start()
        {
            if (playerStats == null)
            {
                playerStats = GetComponent<PlayerStats>();
            }
        }

        // Ideally called from a level up event in PlayerStats
        public void AddSkillPoint()
        {
            availableSkillPoints++;
            Debug.Log($"Gained a skill point! Total: {availableSkillPoints}");
        }

        public bool TryUnlockSkill(SkillNode skill)
        {
            if (unlockedSkills.Contains(skill))
            {
                Debug.Log("Skill already unlocked.");
                return false;
            }

            if (availableSkillPoints < skill.skillPointCost)
            {
                Debug.Log("Not enough skill points.");
                return false;
            }

            if (playerStats.level < skill.requiredLevel)
            {
                Debug.Log($"Level too low. Requires level {skill.requiredLevel}.");
                return false;
            }

            // Check prerequisites
            foreach (var preReq in skill.prerequisites)
            {
                if (!unlockedSkills.Contains(preReq))
                {
                    Debug.Log($"Missing prerequisite skill: {preReq.skillName}");
                    return false;
                }
            }

            // Unlock successful
            availableSkillPoints -= skill.skillPointCost;
            unlockedSkills.Add(skill);
            ApplySkillPassive(skill);
            
            Debug.Log($"Successfully unlocked skill: {skill.skillName}");
            return true;
        }

        private void ApplySkillPassive(SkillNode skill)
        {
            switch (skill.category)
            {
                case SkillCategory.Combat:
                    // Increase base damage dynamically here via a combat manager or stats multiplier
                    Debug.Log($"Applied +{skill.valueBoost} to Combat");
                    break;
                case SkillCategory.Defense:
                    playerStats.defense += skill.valueBoost;
                    break;
                case SkillCategory.Magic:
                    playerStats.maxMP += skill.valueBoost;
                    playerStats.currentMP += skill.valueBoost;
                    break;
                case SkillCategory.Speed:
                    // This would tie into PlayerController movement speed multipliers
                    break;
                case SkillCategory.CriticalMastery:
                    playerStats.criticalChance += skill.valueBoost;
                    break;
            }
        }
    }
}
