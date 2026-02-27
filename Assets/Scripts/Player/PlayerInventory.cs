using System.Collections.Generic;
using UnityEngine;

namespace ShadowRace.Player
{
    public class PlayerInventory : MonoBehaviour
    {
        public static PlayerInventory Instance { get; private set; }

        [Header("Crafting Materials")]
        public int ironIngots = 0;
        
        // Dictionary mapping string IDs to quantities
        // e.g. "Flaming Gland" -> 3
        private Dictionary<string, int> monsterCores = new Dictionary<string, int>();

        private void Awake()
        {
            if (Instance == null) Instance = this;
            else Destroy(gameObject);
        }

        public void AddMaterial(string materialID, int amount)
        {
            if (materialID == "Iron Ingot")
            {
                ironIngots += amount;
                return;
            }

            if (monsterCores.ContainsKey(materialID))
            {
                monsterCores[materialID] += amount;
            }
            else
            {
                monsterCores[materialID] = amount;
            }
            Debug.Log($"Inventory: Added {amount}x {materialID}");
        }

        public bool HasMonsterCore(string coreID, int amountRequired = 1)
        {
            if (string.IsNullOrEmpty(coreID)) return true; // No requirement
            if (monsterCores.TryGetValue(coreID, out int amount))
            {
                return amount >= amountRequired;
            }
            return false;
        }

        public void ConsumeMonsterCore(string coreID, int amount = 1)
        {
            if (string.IsNullOrEmpty(coreID)) return;
            if (monsterCores.ContainsKey(coreID))
            {
                monsterCores[coreID] = Mathf.Max(0, monsterCores[coreID] - amount);
            }
        }
    }
}
