using UnityEngine;

namespace ShadowRace.AI
{
    public enum AIState
    {
        Idle,
        Patrol,
        DetectPlayer,
        Chase,
        Attack_Melee,
        Attack_Ranged,
        Evade,
        Retreat,
        Rage,
        Dead
    }

    [RequireComponent(typeof(Rigidbody2D))]
    public class EnemyBrain : MonoBehaviour
    {
        [Header("State")]
        public AIState currentState = AIState.Idle;
        private AIState previousState;

        [Header("Targeting")]
        public Transform playerTarget;
        public float detectionRadius = 10f;
        public float attackRange = 1.5f;
        public float loseInterestRadius = 15f;
        public float evadeDistance = 3f;
        public LayerMask playerLayer;
        public LayerMask obstacleLayer;
        protected Vector2 lastKnownPlayerPosition;

        [Header("Movement Options")]
        public float moveSpeed = 3f;
        public float chaseSpeed = 5f;
        public Transform[] patrolPoints;
        private int currentPatrolIndex = 0;
        
        [Header("Stats Hooks")]
        public float currentHP = 100f;
        public float maxHP = 100f;
        public bool canRage = false;

        protected Rigidbody2D rb;
        protected bool isFacingRight = true;

        protected virtual void Start()
        {
            rb = GetComponent<Rigidbody2D>();
            currentHP = maxHP;
            if (playerTarget == null)
            {
                // Fallback standard tag lookup
                GameObject p = GameObject.FindGameObjectWithTag("Player");
                if (p != null) playerTarget = p.transform;
            }
        }

        // --- NEW COMBAT JUICE OVERRIDE ---
        public void TakeDamage(float amount, bool isCritical = false)
        {
            if (currentState == AIState.Dead) return;

            currentHP -= amount;

            // Combat Juice Hooks
            if (ShadowRace.Core.ObjectPooler.Instance != null)
            {
                // Spawn Floating Damage Text from Pool
                GameObject textObj = ShadowRace.Core.ObjectPooler.Instance.SpawnFromPool("DamageText", transform.position + Vector3.up * 1.5f, Quaternion.identity);
                if (textObj != null)
                {
                    ShadowRace.UI.FloatingDamageNumber fdn = textObj.GetComponent<ShadowRace.UI.FloatingDamageNumber>();
                    if (fdn != null) fdn.Initialize(amount, isCritical);
                }
            }

            if (ShadowRace.Core.HitStopManager.Instance != null)
            {
                // Trigger Hit Stop and Blood VFX
                ShadowRace.Core.HitStopManager.Instance.HitStop(0.05f); // Micro-pause for impact
                ShadowRace.Core.HitStopManager.Instance.SpawnVFX(transform.position);
            }

            if (isCritical && ShadowRace.Core.CameraShake.Instance != null)
            {
                // Only shake camera on big hits
                ShadowRace.Core.CameraShake.Instance.ShakeCamera(3f, 0.2f);
            }

            // Check Death
            if (currentHP <= 0)
            {
                ChangeState(AIState.Dead);
            }
            else
            {
                // Aggro the enemy
                if (currentState == AIState.Idle || currentState == AIState.Patrol)
                {
                    ChangeState(AIState.Chase);
                }
            }
        }

        protected virtual void Update()
        {
            if (currentState == AIState.Dead) return;

            CheckTransitions();
            ExecuteState();
        }

        private void CheckTransitions()
        {
            if (currentHP <= 0)
            {
                ChangeState(AIState.Dead);
                return;
            }

            if (canRage && currentHP <= maxHP * 0.25f && currentState != AIState.Rage)
            {
                ChangeState(AIState.Rage);
                return;
            }

            float distToPlayer = playerTarget != null ? Vector2.Distance(transform.position, playerTarget.position) : Mathf.Infinity;
            bool hasLOS = HasLineOfSight();

            // Always update memory if we can see the player
            if (hasLOS && playerTarget != null)
            {
                lastKnownPlayerPosition = playerTarget.position;
            }

            switch (currentState)
            {
                case AIState.Idle:
                case AIState.Patrol:
                    if (distToPlayer <= detectionRadius && hasLOS)
                    {
                        ChangeState(AIState.DetectPlayer);
                    }
                    else if (currentState == AIState.Idle && patrolPoints.Length > 0)
                    {
                        ChangeState(AIState.Patrol);
                    }
                    break;
                case AIState.DetectPlayer:
                    ChangeState(AIState.Chase);
                    break;
                case AIState.Chase:
                    // Tactics: Evade if low health and player is rushing
                    if (currentHP < (maxHP * 0.4f) && distToPlayer <= evadeDistance)
                    {
                        ChangeState(AIState.Evade);
                    }
                    else if (distToPlayer > loseInterestRadius)
                    {
                        ChangeState(AIState.Patrol);
                    }
                    else if (distToPlayer <= attackRange && hasLOS)
                    {
                        ChangeState(AIState.Attack_Melee); // Override based on archetype in children classes
                    }
                    else if (!hasLOS && distToPlayer > evadeDistance)
                    {
                         // Move to last known position before deciding to patrol
                    }
                    break;
                case AIState.Evade:
                    if (distToPlayer > evadeDistance * 1.5f || !hasLOS)
                    {
                        ChangeState(AIState.Chase); // Return to chase once safe distance is met
                    }
                    break;
                case AIState.Attack_Melee:
                case AIState.Attack_Ranged:
                    if (distToPlayer > attackRange || !hasLOS)
                    {
                        ChangeState(AIState.Chase);
                    }
                    break;
            }
        }

        protected virtual void ExecuteState()
        {
            switch (currentState)
            {
                case AIState.Idle:
                    rb.velocity = new Vector2(0, rb.velocity.y);
                    break;
                case AIState.Patrol:
                    ExecutePatrol();
                    break;
                case AIState.Chase:
                    ExecuteChase();
                    break;
                case AIState.Evade:
                    ExecuteEvade();
                    break;
                case AIState.Attack_Melee:
                    ExecuteMeleeAttack();
                    break;
                case AIState.Attack_Ranged:
                    ExecuteRangedAttack();
                    break;
                case AIState.Dead:
                    ExecuteDeath();
                    break;
            }
        }

        protected virtual void ExecutePatrol()
        {
            if (patrolPoints == null || patrolPoints.Length == 0) return;

            Transform targetPoint = patrolPoints[currentPatrolIndex];
            MoveTowardsPoint(targetPoint.position, moveSpeed);

            if (Vector2.Distance(transform.position, targetPoint.position) < 0.5f)
            {
                currentPatrolIndex = (currentPatrolIndex + 1) % patrolPoints.Length;
            }
        }

        protected virtual void ExecuteChase()
        {
            if (playerTarget == null) return;

            // If we lost sight, move to where they were last seen
            if (!HasLineOfSight())
            {
                MoveTowardsPoint(lastKnownPlayerPosition, chaseSpeed);
            }
            else
            {
                MoveTowardsPoint(playerTarget.position, chaseSpeed);
            }
        }

        protected virtual void ExecuteEvade()
        {
            if (playerTarget == null) return;
            // Move away from the player
            float direction = Mathf.Sign(transform.position.x - playerTarget.position.x);
            rb.velocity = new Vector2(direction * chaseSpeed, rb.velocity.y);

            if (direction > 0 && !isFacingRight) Flip();
            else if (direction < 0 && isFacingRight) Flip();
        }

        protected virtual void MoveTowardsPoint(Vector2 point, float speed)
        {
            float direction = Mathf.Sign(point.x - transform.position.x);
            rb.velocity = new Vector2(direction * speed, rb.velocity.y);

            if (direction > 0 && !isFacingRight) Flip();
            else if (direction < 0 && isFacingRight) Flip();
        }

        protected virtual void ExecuteMeleeAttack()
        {
            rb.velocity = new Vector2(0, rb.velocity.y);
            // Wait for attack cooldown, trigger animation
        }

        protected virtual void ExecuteRangedAttack()
        {
            rb.velocity = new Vector2(0, rb.velocity.y);
            // Aim and fire logic
        }

        protected virtual void ExecuteDeath()
        {
            rb.velocity = Vector2.zero;
            // Disable colliders, play death anim, drop loot
            Debug.Log($"{gameObject.name} died.");
            enabled = false;
        }

        public void ChangeState(AIState newState)
        {
            previousState = currentState;
            currentState = newState;
            // Debug.Log($"{gameObject.name} entered state: {newState}");
        }

        protected bool HasLineOfSight()
        {
            if (playerTarget == null) return false;
            
            Vector2 direction = playerTarget.position - transform.position;
            RaycastHit2D hit = Physics2D.Raycast(transform.position, direction.normalized, detectionRadius, obstacleLayer | playerLayer);
            
            if (hit.collider != null)
            {
                return hit.collider.CompareTag("Player");
            }
            return false;
        }

        protected void Flip()
        {
            isFacingRight = !isFacingRight;
            transform.Rotate(0f, 180f, 0f);
        }

        private void OnDrawGizmosSelected()
        {
            Gizmos.color = Color.yellow;
            Gizmos.DrawWireSphere(transform.position, detectionRadius);
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(transform.position, attackRange);
        }
    }
}
