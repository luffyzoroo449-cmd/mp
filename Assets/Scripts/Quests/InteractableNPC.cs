using UnityEngine;
using ShadowRace.UI;

namespace ShadowRace.Quests
{
    public class InteractableNPC : MonoBehaviour
    {
        [Header("Interaction Settings")]
        public float interactRadius = 2f;
        public LayerMask playerLayer;
        public bool hasInteracted = false;

        [Header("Dialogue / Quest Data")]
        public DialogueData npcDialogue;
        [Tooltip("Optional: The ID of the quest this NPC gives when spoken to.")]
        public string questIDToGive;

        private void Update()
        {
            Collider2D player = Physics2D.OverlapCircle(transform.position, interactRadius, playerLayer);
            
            if (player != null && player.CompareTag("Player"))
            {
                // Wait for player to press 'E' or assigned Interact button
                if (Input.GetKeyDown(KeyCode.E) && !hasInteracted)
                {
                    OnInteract(player.gameObject);
                }
            }
        }

        private void OnInteract(GameObject player)
        {
            hasInteracted = true;
            Debug.Log($"Interacting with {gameObject.name}");

            // Trigger Dialogue UI
            if (npcDialogue != null && DialogueManager.Instance != null)
            {
                DialogueManager.Instance.StartDialogue(npcDialogue);
            }

            // Trigger Quest Acceptance
            if (!string.IsNullOrEmpty(questIDToGive) && QuestManager.Instance != null)
            {
                QuestManager.Instance.AcceptQuest(questIDToGive);
            }
            
            // Note: In a full system, we might reset `hasInteracted` when dialogue ends, 
            // or keep it true if this NPC only speaks once.
        }

        private void OnDrawGizmosSelected()
        {
            Gizmos.color = Color.cyan;
            Gizmos.DrawWireSphere(transform.position, interactRadius);
        }
    }
}
