using UnityEngine;

namespace ShadowRace.AI
{
    public class LavaBossAI : BossAI
    {
        [Header("Lava Specifics")]
        public GameObject lavaPillarPrefab;
        public float pillarDamage = 40f;
        public int pillarsToSpawn = 5;

        protected override void ExecuteState()
        {
            base.ExecuteState(); // Runs inherited boss logic including phases
        }

        // We override the base random skill logic to add Lava specific capabilities
        private new void ExecuteRandomBossSkill() // Using 'new' intentionally to mask base behavior for a specific rewrite if needed, though overriding might be cleaner architecture-wise if base method was virtual.
        {
            if (currentPhase == 1)
            {
                Debug.Log($"{gameObject.name} (Lava) used basic Heavy Strike!");
            }
            else if (currentPhase == 2)
            {
                if (Random.value > 0.5f) EruptLavaPillars();
                else GetComponent<BossAI>().Invoke("GroundPound", 0f); // Calling disguised parent method
            }
            else if (currentPhase == 3)
            {
                // Both!
                EruptLavaPillars();
                GetComponent<BossAI>().Invoke("BulletHell", 0f); 
            }
        }

        private void EruptLavaPillars()
        {
            Debug.Log("Lava Boss erupts Pillars from the ground!");
            
            if (lavaPillarPrefab == null || playerTarget == null) return;

            // Spawn random pillars relative to the player
            for (int i = 0; i < pillarsToSpawn; i++)
            {
                Vector3 randomOffset = new Vector3(Random.Range(-5f, 5f), 0, 0);
                Vector3 spawnPos = playerTarget.position + randomOffset;
                
                // You would typically raycast down here to find the actual ground coordinates
                
                GameObject pillar = Instantiate(lavaPillarPrefab, spawnPos, Quaternion.identity);
                Destroy(pillar, 3f);
            }
        }
    }
}
