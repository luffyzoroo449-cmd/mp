using System.Collections.Generic;
using UnityEngine;

namespace ShadowRace.Quests
{
    public enum QuestStatus
    {
        NotStarted,
        Active,
        Completed
    }

    [System.Serializable]
    public class Quest
    {
        public string questID;
        public string title;
        [TextArea(2, 4)]
        public string description;
        public int requiredLevel = 1;
        
        public QuestStatus status = QuestStatus.NotStarted;
        
        // Rewards
        public int rewardXP = 500;
        public int rewardMoney = 1000;
        public ShadowRace.Combat.WeaponData rewardWeapon;
    }

    public class QuestManager : MonoBehaviour
    {
        public static QuestManager Instance { get; private set; }

        public List<Quest> allQuests;
        private Dictionary<string, Quest> questDictionary;

        private void Awake()
        {
            if (Instance == null)
            {
                Instance = this;
            }
            InitializeQuests();
        }

        private void InitializeQuests()
        {
            questDictionary = new Dictionary<string, Quest>();
            foreach (Quest q in allQuests)
            {
                if (!questDictionary.ContainsKey(q.questID))
                {
                    questDictionary.Add(q.questID, q);
                }
            }
        }

        public void AcceptQuest(string questID)
        {
            if (questDictionary.TryGetValue(questID, out Quest q))
            {
                if (q.status == QuestStatus.NotStarted)
                {
                    q.status = QuestStatus.Active;
                    Debug.Log($"Quest Accepted: {q.title}");
                }
            }
        }

        public void CompleteQuest(string questID, ShadowRace.Player.PlayerStats player)
        {
            if (questDictionary.TryGetValue(questID, out Quest q))
            {
                if (q.status == QuestStatus.Active)
                {
                    q.status = QuestStatus.Completed;
                    player.AddXP(q.rewardXP);
                    player.money += q.rewardMoney;

                    if (q.rewardWeapon != null)
                    {
                        Debug.Log($"Quest Completed! Rewarded with the legendary {q.rewardWeapon.weaponName}.");
                        // Logic to add weapon to player manager
                    }
                    else
                    {
                        Debug.Log($"Quest Completed: {q.title}!");
                    }
                }
            }
        }
    }
}
