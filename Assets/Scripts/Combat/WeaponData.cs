using UnityEngine;

namespace ShadowRace.Combat
{
    public enum WeaponType
    {
        Melee,
        Pistol,
        Shotgun,
        Rifle,
        Sniper,
        RocketLauncher,
        MagicStaff
    }

    public enum WeaponRarity
    {
        Common,
        Uncommon,
        Rare,
        Epic,
        Legendary
    }

    public enum ElementType
    {
        None,
        Fire,
        Ice,
        Poison,
        Lightning,
        Void
    }

    [CreateAssetMenu(fileName = "New Weapon", menuName = "ShadowRace/Weapon Data")]
    public class WeaponData : ScriptableObject
    {
        [Header("Weapon Identity")]
        public string weaponName;
        public WeaponType type;
        public ElementType elementType;
        public WeaponRarity rarity = WeaponRarity.Common;
        [TextArea(2, 4)]
        public string description;
        public Sprite weaponIcon;

        [Header("Core Stats")]
        public float baseDamage = 10f;
        [Range(0f, 100f)]
        public float criticalChance = 5f;
        [Tooltip("Extra damage multiplier on crit")]
        public float criticalMultiplier = 1.5f;

        [Header("Upgrades & Crafting")]
        public int currentTier = 1;
        public int maxTier = 5;
        
        [Tooltip("The ID of the specific Monster Core required to forge the next tier.")]
        public string requiredMonsterCoreID;
        public int requiredIronIngots = 5;
        public int baseUpgradeOrens = 100;
        
        [Header("Melee Attributes")]
        public float attackRange = 1.5f;
        public float attackRate = 1f; // Attacks per second
        public float comboTimeWindow = 0.5f;

        [Header("Ranged Attributes")]
        public float fireRate = 0.5f; // Shots per second
        public int magazineSize = 10;
        public float reloadTime = 1.5f;
        public float projectileSpeed = 20f;
        public float bulletSpread = 0f;
        public GameObject projectilePrefab;

        [Header("Recoil / Visual Feedback")]
        public float screenShakeIntensity = 1f;
        public float enemyKnockbackForce = 2f;
        [Tooltip("How hard this weapon kicks the player backward when fired mid-air.")]
        public Vector2 playerRecoilForce = Vector2.zero;

        [Header("Durability")]
        public bool usesDurability = true;
        public float maxDurability = 100f;
        public float currentDurability = 100f;

        public int GetUpgradeCost()
        {
            return baseUpgradeOrens * currentTier;
        }
    }
}
