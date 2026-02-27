using UnityEngine;

namespace ShadowRace.AI
{
    public class MeleeEnemy : EnemyBrain
    {
        [Header("Melee Specifics")]
        public float attackDamage = 20f;
        public float attackCooldown = 1.5f;
        private float lastAttackTime;

        protected override void ExecuteMeleeAttack()
        {
            base.ExecuteMeleeAttack(); // stops movement

            if (Time.time >= lastAttackTime + attackCooldown)
            {
                // Face player before striking
                if (playerTarget != null)
                {
                    float dir = playerTarget.position.x - transform.position.x;
                    if (dir > 0 && !isFacingRight) Flip();
                    else if (dir < 0 && isFacingRight) Flip();
                }

                Debug.Log($"{gameObject.name} struck player for {attackDamage} damage!");
                lastAttackTime = Time.time;
                
                // TODO: Apply damage to PlayerStats if inside hitbox hit
            }
        }
    }
}
