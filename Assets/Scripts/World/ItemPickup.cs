using UnityEngine;
using ShadowRace.Core;

namespace ShadowRace.World
{
    public class ItemPickup : MonoBehaviour
    {
        public enum ItemType
        {
            LoreBook,
            SmallKey,
            BossKey,
            HealthUpgrade
        }

        [Header("Pickup Settings")]
        public ItemType type;
        public string itemName = "Mystery Item";
        [Tooltip("The ID used to save if this specific item in the world has been picked up before.")]
        public string uniqueWorldID;

        private void Start()
        {
            // If this item was already collected in this save file, destroy it on load
            if (!string.IsNullOrEmpty(uniqueWorldID) && GameManager.Instance != null)
            {
                if (PlayerPrefs.GetInt($"{GameManager.Instance.saveFileName}_{uniqueWorldID}", 0) == 1)
                {
                    Destroy(gameObject);
                }
            }
        }

        private void OnTriggerEnter2D(Collider2D other)
        {
            if (other.CompareTag("Player"))
            {
                CollectItem(other.GetComponent<ShadowRace.Player.PlayerStats>());
            }
        }

        private void CollectItem(ShadowRace.Player.PlayerStats player)
        {
            Debug.Log($"Picked up: {itemName}");

            switch (type)
            {
                case ItemType.LoreBook:
                    // Unlock Lore Entry in a UI Manager
                    break;
                case ItemType.SmallKey:
                    // Add key to player inventory
                    break;
                case ItemType.BossKey:
                    // Unlock boss door for this region
                    break;
                case ItemType.HealthUpgrade:
                    if (player != null)
                    {
                        player.maxHP += 20;
                        player.currentHP = player.maxHP;
                    }
                    break;
            }

            // Save the fact that this item was collected so it doesn't respawn
            if (!string.IsNullOrEmpty(uniqueWorldID) && GameManager.Instance != null)
            {
                PlayerPrefs.SetInt($"{GameManager.Instance.saveFileName}_{uniqueWorldID}", 1);
            }

            // Optional: Play Sound/VFX
            // ObjectPooler.Instance.SpawnFromPool("PickupVFX", transform.position, Quaternion.identity);

            Destroy(gameObject);
        }
    }
}
