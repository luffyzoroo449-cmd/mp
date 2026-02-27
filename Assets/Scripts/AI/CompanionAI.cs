using UnityEngine;

namespace ShadowRace.AI
{
    public class CompanionAI : EnemyBrain
    {
        [Header("Companion Links")]
        public Transform followTarget; // Usually the Player
        public float followDistance = 4f;

        protected override void Start()
        {
            base.Start();

            // Override base layers to target Enemies instead of Players
            // Assuming Enums/Layers are properly set up in Editor
            playerLayer = 1 << LayerMask.NameToLayer("Enemy");

            if (followTarget == null)
            {
                GameObject p = GameObject.FindGameObjectWithTag("Player");
                if (p != null) followTarget = p.transform;
            }
        }

        protected override void Update()
        {
            if (currentState == AIState.Dead) return;

            // In Companion mode, if we have no enemy target, we default back to Patrolling near the Player
            if (currentState == AIState.Idle || currentState == AIState.Patrol)
            {
                if (followTarget != null)
                {
                    float distToFollow = Vector2.Distance(transform.position, followTarget.position);

                    // If we fall behind, run to player
                    if (distToFollow > followDistance)
                    {
                        MoveTowardsPoint(followTarget.position, moveSpeed);
                    }
                    else
                    {
                        // Stop moving
                        rb.velocity = new Vector2(0, rb.velocity.y);
                    }
                }
            }

            // Let base methods handle enemy detection and attack
            base.Update();
        }

        // Override the patrol to not wander off, but stick to player
        protected override void ExecutePatrol()
        {
            // Companion Patrol simply stays near followTarget. Base Update handles this for now.
        }

        protected override void ExecuteDeath()
        {
            // Companions might just become incapacitated instead of dying
            Debug.Log($"Companion downed! Needs revival.");
            rb.velocity = Vector2.zero;
            
            // In a full implementation, the player could interact with them to revive.
            ChangeState(AIState.Dead);
        }
    }
}
