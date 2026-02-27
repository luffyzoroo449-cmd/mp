using UnityEngine;

namespace ShadowRace.Combat
{
    public enum LootType
    {
        Money,
        HealthPotion,
        ManaPotion,
        RareGem,
        WeaponPart
    }

    [RequireComponent(typeof(Collider2D))]
    public class LootDrop : MonoBehaviour
    {
        public LootType lootType;
        public int amount = 10;
        public float pickupRadius = 1.5f;
        public LayerMask playerLayer;

        private void Update()
        {
            Collider2D[] players = Physics2D.OverlapCircleAll(transform.position, pickupRadius, playerLayer);
            foreach (Collider2D p in players)
            {
                if (p.CompareTag("Player"))
                {
                    OnPickup(p.gameObject);
                    break;
                }
            }
        }

        private void OnPickup(GameObject player)
        {
            ShadowRace.Player.PlayerStats stats = player.GetComponent<ShadowRace.Player.PlayerStats>();
            if (stats != null)
            {
                switch (lootType)
                {
                    case LootType.Money:
                        stats.money += amount;
                        Debug.Log($"Picked up {amount} Money!");
                        break;
                    case LootType.HealthPotion:
                        stats.Heal(amount);
                        Debug.Log($"Restored {amount} HP!");
                        break;
                    case LootType.ManaPotion:
                        stats.currentMP = Mathf.Min(stats.currentMP + amount, stats.maxMP);
                        Debug.Log($"Restored {amount} MP!");
                        break;
                    case LootType.RareGem:
                        // Would integrate with crafting/inventory manager
                        Debug.Log($"Found a Rare Gem!");
                        break;
                    case LootType.WeaponPart:
                        // Would integrate with crafting/inventory manager
                        Debug.Log($"Found Weapon Parts!");
                        break;
                }
                
                // Play pickup sound/VFX here
                Destroy(gameObject);
            }
        }

        private void OnDrawGizmosSelected()
        {
            Gizmos.color = Color.green;
            Gizmos.DrawWireSphere(transform.position, pickupRadius);
        }
    }
}
