using UnityEngine;

namespace ShadowRace.World
{
    public class StoreItem : MonoBehaviour
    {
        public ShadowRace.Combat.WeaponData assignedWeapon;
        public int cost;

        public void Purchase(ShadowRace.Player.PlayerStats playerStats)
        {
            if (playerStats.money >= cost)
            {
                playerStats.money -= cost;
                
                // Add assignedWeapon to Player's WeaponManager
                ShadowRace.Combat.WeaponManager manager = playerStats.GetComponent<ShadowRace.Combat.WeaponManager>();
                if (manager != null)
                {
                    // Logic to find an empty slot or replace current equipped weapon
                    Debug.Log($"Purchased {assignedWeapon.weaponName} for {cost} coins!");
                }
            }
            else
            {
                Debug.Log("Not enough money for this item!");
            }
        }
    }
}
