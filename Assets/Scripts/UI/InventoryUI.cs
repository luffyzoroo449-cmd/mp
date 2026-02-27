using UnityEngine;
using TMPro;
using UnityEngine.UI;
using ShadowRace.Player;

namespace ShadowRace.UI
{
    public class InventoryUI : MonoBehaviour
    {
        [Header("UI Panels")]
        public GameObject inventoryPanel;

        [Header("Text References")]
        public TextMeshProUGUI hpText;
        public TextMeshProUGUI mpText;
        public TextMeshProUGUI staminaText;
        public TextMeshProUGUI attackText;
        public TextMeshProUGUI defenseText;
        public TextMeshProUGUI critText;
        public TextMeshProUGUI gemsText;
        public TextMeshProUGUI partsText;

        [Header("Scripts")]
        public PlayerStats playerStats;

        private bool isOpen = false;

        private void Update()
        {
            // E.g., Tab to toggle inventory
            if (Input.GetKeyDown(KeyCode.Tab))
            {
                if (isOpen) CloseInventory();
                else OpenInventory();
            }
        }

        public void OpenInventory()
        {
            if (playerStats == null) return;
            
            inventoryPanel.SetActive(true);
            isOpen = true;
            Time.timeScale = 0f; // Pause game while checking bags
            UpdateUI();
        }

        public void CloseInventory()
        {
            inventoryPanel.SetActive(false);
            isOpen = false;
            Time.timeScale = 1f;
        }

        private void UpdateUI()
        {
            hpText.text = $"HP: {playerStats.currentHP} / {playerStats.maxHP}";
            mpText.text = $"MP: {playerStats.currentMP} / {playerStats.maxMP}";
            staminaText.text = $"ST: {playerStats.currentStamina} / {playerStats.maxStamina}";
            
            // Assume base player stats
            attackText.text = $"ATK: (See Weapon)"; 
            defenseText.text = $"DEF: {playerStats.defense}";
            critText.text = $"CRIT: {playerStats.criticalChance}%";

            // Assuming we added simple ints to PlayerStats for Gems/Parts
            // gemsText.text = $"Rare Gems: {playerStats.rareGems}";
            // partsText.text = $"Weapon Parts: {playerStats.weaponParts}";
        }
    }
}
