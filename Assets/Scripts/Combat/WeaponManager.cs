using UnityEngine;
using ShadowRace.Player;

namespace ShadowRace.Combat
{
    public class WeaponManager : MonoBehaviour
    {
        [Header("Equipped Weapons")]
        public WeaponProxy[] equippedWeapons = new WeaponProxy[2];
        private int currentWeaponIndex = 0;

        [Header("References")]
        public Transform weaponHolder; // Where to spawn weapon sprites
        public PlayerStats playerStats; // Reference to track money
        public PlayerInputHandler inputHandler; // Hook for switching weapons

        [Header("Projectile Spawn")]
        public Transform firePoint;

        private void Start()
        {
            playerStats = GetComponent<PlayerStats>();
            inputHandler = GetComponent<PlayerInputHandler>();
            
            EquipWeapon(0); // Equip primary by default
        }

        private void Update()
        {
            HandleWeaponSwitching();
            HandleCombat();
        }

        private void HandleWeaponSwitching()
        {
            // Simple swap on pressing 'Q' or numeric keys, expand in new input system
            if (UnityEngine.Input.GetKeyDown(KeyCode.Q))
            {
                currentWeaponIndex = (currentWeaponIndex + 1) % equippedWeapons.Length;
                EquipWeapon(currentWeaponIndex);
            }
        }

        private void EquipWeapon(int index)
        {
            if (equippedWeapons[index] == null) return;
            
            // Enable current weapon GameObject, disable others
            for (int i = 0; i < equippedWeapons.Length; i++)
            {
                if (equippedWeapons[i] != null)
                {
                    equippedWeapons[i].gameObject.SetActive(i == index);
                }
            }
            
            Debug.Log($"Equipped {equippedWeapons[index].weaponData.weaponName}");
        }

        private void HandleCombat()
        {
            if (inputHandler.AttackInput)
            {
                WeaponProxy currentWeapon = equippedWeapons[currentWeaponIndex];
                if (currentWeapon != null && currentWeapon.CanAttack())
                {
                    if (currentWeapon.IsRanged())
                    {
                        FireRanged(currentWeapon);
                    }
                    else
                    {
                        PerformMelee(currentWeapon);
                    }
                }
                
                // Use input depending on style (auto-fire vs semi-auto)
                // inputHandler.UseAttackInput();
            }
        }

        private void FireRanged(WeaponProxy weapon)
        {
            Debug.Log($"Fired {weapon.weaponData.weaponName}. Ammo left: {weapon.currentAmmo - 1}");
            weapon.ConsumeAmmo();
            weapon.ResetAttackTimer();

            if (weapon.weaponData.projectilePrefab != null && firePoint != null)
            {
                GameObject projObj = Instantiate(weapon.weaponData.projectilePrefab, firePoint.position, firePoint.rotation);
                Projectile projectile = projObj.GetComponent<Projectile>();
                if (projectile != null)
                {
                    // Apply random spread
                    float randomAngle = Random.Range(-weapon.weaponData.bulletSpread, weapon.weaponData.bulletSpread);
                    projObj.transform.Rotate(0, 0, randomAngle);
                    
                    // Calc Critical
                    float finalDamage = weapon.weaponData.baseDamage;
                    if (Random.Range(0f, 100f) <= weapon.weaponData.criticalChance)
                    {
                        finalDamage *= weapon.weaponData.criticalMultiplier;
                        Debug.Log("CRITICAL HIT!");
                    }

                    projectile.Initialize(finalDamage, weapon.weaponData.projectileSpeed, weapon.weaponData.elementType);
                }
            }
        }

        private void PerformMelee(WeaponProxy weapon)
        {
            Debug.Log($"Swung {weapon.weaponData.weaponName}.");
            weapon.ResetAttackTimer();
            // Trigger animation, overlap circle logic moved here or to proxy
        }

        public void AttemptUpgradeCurrentWeapon()
        {
            WeaponProxy currentWeapon = equippedWeapons[currentWeaponIndex];
            if (currentWeapon == null) return;

            int cost = currentWeapon.weaponData.GetUpgradeCost();

            if (playerStats.money >= cost)
            {
                if (currentWeapon.weaponData.currentLevel < currentWeapon.weaponData.maxLevel)
                {
                    playerStats.money -= cost;
                    currentWeapon.Upgrade();
                    Debug.Log($"Upgraded! Remaining money: {playerStats.money}");
                }
                else
                {
                    Debug.Log("Weapon is at Max Level!");
                }
            }
            else
            {
                Debug.Log($"Not enough money! Need {cost}, have {playerStats.money}.");
            }
        }
    }
}
