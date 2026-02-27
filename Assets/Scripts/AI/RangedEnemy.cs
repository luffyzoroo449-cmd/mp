using UnityEngine;
using ShadowRace.Combat; // Access to Projectile

namespace ShadowRace.AI
{
    public class RangedEnemy : EnemyBrain
    {
        [Header("Ranged Specifics")]
        public float attackDamage = 15f;
        public float attackCooldown = 2f;
        public float projectileSpeed = 15f;
        public GameObject projectilePrefab;
        public Transform firePoint;
        
        private float lastAttackTime;

        protected override void ExecuteRangedAttack()
        {
            base.ExecuteRangedAttack(); // stops movement

            if (Time.time >= lastAttackTime + attackCooldown)
            {
                // Face player
                if (playerTarget != null)
                {
                    float dir = playerTarget.position.x - transform.position.x;
                    if (dir > 0 && !isFacingRight) Flip();
                    else if (dir < 0 && isFacingRight) Flip();
                }

                FireProjectile();
                lastAttackTime = Time.time;
            }
        }

        private void FireProjectile()
        {
            if (projectilePrefab != null && firePoint != null)
            {
                GameObject projObj = Instantiate(projectilePrefab, firePoint.position, firePoint.rotation);
                Projectile projectile = projObj.GetComponent<Projectile>();
                if (projectile != null)
                {
                    Debug.Log($"{gameObject.name} fired a projectile!");
                    projectile.Initialize(attackDamage, projectileSpeed, ElementType.None);
                }
            }
        }
    }
}
