using System.Collections.Generic;
using UnityEngine;
using ShadowRace.Combat;

namespace ShadowRace.World
{
    public class SecretStore : MonoBehaviour
    {
        [Header("Store Settings")]
        public float spawnChance = 0.25f; // 25% chance to actually appear when level loads
        public bool isSpawned = false;
        public LayerMask playerLayer;
        public float interactRadius = 2f;
        
        [Header("Inventory Generation")]
        public List<WeaponData> possibleWeapons;
        public List<WeaponData> storeStock;
        public int itemsToStock = 3;

        private void Start()
        {
            if (Random.value <= spawnChance)
            {
                isSpawned = true;
                GenerateStock();
                Debug.Log("A Secret Store has spawned in this zone!");
            }
            else
            {
                // Disable visuals/colliders for the store
                gameObject.SetActive(false);
            }
        }

        private void GenerateStock()
        {
            storeStock = new List<WeaponData>();
            for (int i = 0; i < itemsToStock; i++)
            {
                if (possibleWeapons.Count > 0)
                {
                    // Pick random weapon
                    WeaponData w = possibleWeapons[Random.Range(0, possibleWeapons.Count)];
                    storeStock.Add(w);
                }
            }
        }

        private void Update()
        {
            if (!isSpawned) return;

            Collider2D player = Physics2D.OverlapCircle(transform.position, interactRadius, playerLayer);
            if (player != null && player.CompareTag("Player"))
            {
                // Prompt user to press interact key (e.g., 'E')
                if (Input.GetKeyDown(KeyCode.E))
                {
                    OpenStoreUI(player.gameObject);
                }
            }
        }

        private void OpenStoreUI(GameObject playerObj)
        {
            Debug.Log($"--- Welcome to the Secret Store! ---");
            // Here, you would instantiate a UI Panel and list the storeStock
            for (int i = 0; i < storeStock.Count; i++)
            {
                int cost = storeStock[i].GetUpgradeCost() * 5; // Expensive base cost
                Debug.Log($"[Item {i}] {storeStock[i].weaponName} - Costs: {cost} Coins");
            }
            Debug.Log("------------------------------------");
            
            // Player input handling for purchasing goes to a dedicated Buy item function
        }
    }
}
