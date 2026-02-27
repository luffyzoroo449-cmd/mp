using UnityEngine;

namespace ShadowRace.Combat
{
    public class WeaponProxy : MonoBehaviour
    {
        public WeaponData weaponData;
        
        [Header("Ranged State")]
        public int currentAmmo;
        public bool isReloading;
        protected float nextFireTime;

        protected virtual void Start()
        {
            if (weaponData != null && IsRanged())
            {
                currentAmmo = weaponData.magazineSize;
            }
        }

        public bool IsRanged()
        {
            return weaponData.type != WeaponType.Melee;
        }

        public virtual bool CanAttack()
        {
            if (isReloading) return false;
            
            if (IsRanged() && currentAmmo <= 0)
            {
                // Trigger reload implicitly or let player handle it
                return false;
            }

            if (Time.time < nextFireTime) return false;

            return true;
        }

        public virtual void ConsumeAmmo()
        {
            if (IsRanged())
            {
                currentAmmo--;
            }
        }

        public virtual void ResetAttackTimer()
        {
            float rate = IsRanged() ? weaponData.fireRate : weaponData.attackRate;
            nextFireTime = Time.time + (1f / rate);
        }

        public virtual void Upgrade()
        {
            if (weaponData.currentTier < weaponData.maxTier)
            {
                weaponData.currentTier++;
                weaponData.baseDamage *= 1.15f; // 15% increase per level as example
                Debug.Log($"{weaponData.weaponName} upgraded to Tier {weaponData.currentTier}!");
            }
        }

        public virtual void ConsumeDurability(float amount)
        {
            if (weaponData.usesDurability)
            {
                weaponData.currentDurability = Mathf.Max(0f, weaponData.currentDurability - amount);
                if (weaponData.currentDurability <= 0f)
                {
                    Debug.Log($"{weaponData.weaponName} has broken!");
                }
            }
        }
    }
}
