using System.Collections.Generic;
using UnityEngine;

namespace ShadowRace.Player
{
    public enum SkillCategory
    {
        Combat,
        Defense,
        Magic,
        Speed,
        CriticalMastery
    }

    [CreateAssetMenu(fileName = "New Skill", menuName = "ShadowRace/Skill Data")]
    public class SkillNode : ScriptableObject
    {
        public string skillName;
        public SkillCategory category;
        [TextArea(2, 4)]
        public string description;
        public int requiredLevel = 1;
        public int skillPointCost = 1;
        public float valueBoost = 5f; // E.g., +5 Damage, +5% Crit Event
        
        // Requirements
        public List<SkillNode> prerequisites;
    }
}
