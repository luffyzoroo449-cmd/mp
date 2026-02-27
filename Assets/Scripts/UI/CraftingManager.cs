using UnityEngine;
using ShadowRace.Combat;
using ShadowRace.Player;

namespace ShadowRace.UI
{
    public class CraftingManager : MonoBehaviour
    {
        public static CraftingManager Instance { get; private set; }

        private void Awake()
        {
            if (Instance == null) Instance = this;
            else Destroy(gameObject);
        }

        public bool AttemptUpgrade(WeaponData weapon, PlayerStats stats, PlayerInventory inventory)
        {
            if (weapon.currentTier >= weapon.maxTier)
            {
                Debug.Log("Weapon is already at max tier.");
                return false;
            }

            int orensCost = weapon.GetUpgradeCost();
            int ironCost = weapon.requiredIronIngots * weapon.currentTier; // Scales per tier
            string coreID = weapon.requiredMonsterCoreID;

            // Check Orens
            if (stats.money < orensCost)
            {
                Debug.Log($"Not enough Orens. Need {orensCost}.");
                return false;
            }

            // Check Iron
            if (inventory.ironIngots < ironCost)
            {
                Debug.Log($"Not enough Iron Ingots. Need {ironCost}.");
                return false;
            }

            // Check Monster Core
            if (!inventory.HasMonsterCore(coreID, 1))
            {
                Debug.Log($"Missing required Monster Core: {coreID}");
                return false;
            }

            // All checks passed, consume and upgrade
            stats.money -= orensCost;
            inventory.ironIngots -= ironCost;
            inventory.ConsumeMonsterCore(coreID, 1);

            // Assuming there's a WeaponProxy attached somewhere that manages the active state,
            // but for data persistence we change the SO here.
            weapon.currentTier++;
            weapon.baseDamage *= 1.15f; // 15% increase per tier
            weapon.currentDurability = weapon.maxDurability; // Upgrading repairs weapon completely

            Debug.Log($"SUCCESS: {weapon.weaponName} Upgraded to Tier {weapon.currentTier}!");
            return true;
        }
    }
}
