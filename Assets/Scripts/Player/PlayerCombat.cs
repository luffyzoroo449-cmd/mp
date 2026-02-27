using UnityEngine;

namespace ShadowRace.Player
{
    [RequireComponent(typeof(PlayerInputHandler))]
    public class PlayerCombat : MonoBehaviour
    {
        public PlayerInputHandler InputHandler { get; private set; }
        public PlayerStats Stats { get; private set; }
        private Animator anim;

        [Header("Melee Combat")]
        public Transform meleeAttackPoint;
        public float meleeAttackRange = 0.5f;
        public LayerMask enemyLayers;
        public float meleeDamage = 20f;
        public float meleeAttackRate = 2f;
        private float nextMeleeTime = 0f;

        [Header("Ranged Combat")]
        public Transform rangedFirePoint;
        public float rangedDamage = 15f;
        public float rangedFireRate = 5f;
        public float rangedMPCost = 5f;
        private float nextRangedTime = 0f;

        private void Awake()
        {
            InputHandler = GetComponent<PlayerInputHandler>();
            Stats = GetComponent<PlayerStats>();
            anim = GetComponent<Animator>();
        }

        private void Update()
        {
            CheckCombatInput();
        }

        private void CheckCombatInput()
        {
            if (InputHandler.AttackInput)
            {
                if (Time.time >= nextMeleeTime)
                {
                    MeleeAttack();
                    nextMeleeTime = Time.time + 1f / meleeAttackRate;
                }
                InputHandler.UseAttackInput();
            }

            if (InputHandler.SpecialSkillInput)
            {
                if (Time.time >= nextRangedTime && Stats.currentMP >= rangedMPCost)
                {
                    RangedAttack();
                    nextRangedTime = Time.time + 1f / rangedFireRate;
                }
                InputHandler.UseSpecialSkillInput();
            }
        }

        private void MeleeAttack()
        {
            // Trigger attack animation
            if (anim != null) anim.SetTrigger("MeleeAttack");
            Debug.Log("Player performs Melee Attack");

            // Detect enemies in range
            Collider2D[] hitEnemies = Physics2D.OverlapCircleAll(meleeAttackPoint.position, meleeAttackRange, enemyLayers);

            // Damage them
            foreach (Collider2D enemy in hitEnemies)
            {
                Debug.Log($"Hit {enemy.name} for {meleeDamage} damage.");
                // enemy.GetComponent<EnemyStats>().TakeDamage(meleeDamage);
            }
        }

        private void RangedAttack()
        {
            Stats.currentMP -= rangedMPCost;
            
            // Trigger fire animation
            if (anim != null) anim.SetTrigger("RangedAttack");
            Debug.Log("Player fires Ranged Projectile");

            // Apply AAA Combat Recoil
            PlayerController controller = GetComponent<PlayerController>();
            if (controller != null)
            {
                // Kick the player backwards based on facing direction
                int facingDir = transform.right.x > 0 ? 1 : -1;
                Vector2 recoilForce = new Vector2(-facingDir * 15f, 2f); // Sharp backward and slight upward kick 
                controller.ApplyRecoil(recoilForce);

                // Option to trigger HitStop/CameraShake for heavy weapons
                if (ShadowRace.Core.CameraShake.Instance != null)
                {
                    ShadowRace.Core.CameraShake.Instance.ShakeCamera(2f, 0.1f);
                }
            }

            // Instantiate projectile here using rangedFirePoint.position and rotation
        }

        private void OnDrawGizmosSelected()
        {
            if (meleeAttackPoint == null) return;
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(meleeAttackPoint.position, meleeAttackRange);
        }
    }
}
