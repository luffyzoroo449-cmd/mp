using System.Collections;
using UnityEngine;

namespace ShadowRace.World
{
    public class EnvironmentalHazard : MonoBehaviour
    {
        [Header("Hazard Settings")]
        [Tooltip("Amount of damage applied per tick.")]
        public float damageAmount = 10f;
        [Tooltip("Time between damage ticks.")]
        public float tickRate = 1f;
        
        [Header("Effects")]
        public ShadowRace.Combat.ElementType elementMask;

        private void OnTriggerEnter2D(Collider2D other)
        {
            if (other.CompareTag("Player"))
            {
                StartCoroutine(DamageRoutine(other.GetComponent<ShadowRace.Player.PlayerStats>()));
            }
        }

        private void OnTriggerExit2D(Collider2D other)
        {
            if (other.CompareTag("Player"))
            {
                StopAllCoroutines();
            }
        }

        private IEnumerator DamageRoutine(ShadowRace.Player.PlayerStats stats)
        {
            while (true)
            {
                if (stats != null)
                {
                    stats.TakeDamage(damageAmount);
                    Debug.Log($"Player took {damageAmount} Environmental Damage ({elementMask})!");
                }
                yield return new WaitForSeconds(tickRate);
            }
        }
    }
}
