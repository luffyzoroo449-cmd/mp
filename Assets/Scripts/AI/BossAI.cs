using System.Collections;
using UnityEngine;
using UnityEngine.Events;

namespace ShadowRace.AI
{
    public class BossAI : EnemyBrain
    {
        [Header("Boss Phases")]
        public int currentPhase = 1;
        public float phase2Threshold = 0.6f; // 60% HP
        public float phase3Threshold = 0.25f; // 25% HP

        [Header("Phase Skills")]
        public float skillCooldown = 5f;
        private float lastSkillTime;

        [Header("Bullet Hell Settings")]
        public int bulletCount = 12;
        public GameObject bossProjectile;
        public Transform bossFirePoint;

        [Header("Cinematics & Arena Alteration")]
        public GameObject introVirtualCamera; // Attach a Cinemachine Virtual Camera GameObject here
        public float introDuration = 3f;
        public UnityEvent onRagePhaseBegin;
        private bool isPlayingIntro = false;

        protected override void Start()
        {
            base.Start();
            if (introVirtualCamera != null)
            {
                StartCoroutine(PlayIntroRoutine());
            }
        }

        private IEnumerator PlayIntroRoutine()
        {
            isPlayingIntro = true;
            
            // Activate the Cinemachine camera to cut to the boss
            introVirtualCamera.SetActive(true);
            
            Debug.Log($"[Cinematic] {gameObject.name} Intro Started!");
            
            // Wait for cinematic completion
            yield return new WaitForSeconds(introDuration);
            
            // Deactivate to return to player camera seamlessly
            introVirtualCamera.SetActive(false);
            
            Debug.Log($"[Cinematic] {gameObject.name} Intro Ended!");
            isPlayingIntro = false;
        }

        protected override void Update()
        {
            // Pause all logic during Cinematic Intros or death
            if (isPlayingIntro || currentState == AIState.Dead) return;

            CheckPhaseProgress();
            base.Update(); // Runs core transitions + ExecuteState
        }

        private void CheckPhaseProgress()
        {
            float hpPercentage = currentHP / maxHP;

            if (currentPhase == 1 && hpPercentage <= phase2Threshold)
            {
                EnterPhase(2);
            }
            else if (currentPhase == 2 && hpPercentage <= phase3Threshold)
            {
                EnterPhase(3);
                ChangeState(AIState.Rage); // Trigger rage state in brain
                
                // Trigger major arena destruction or alteration (e.g. dropping floor, breaking walls)
                Debug.Log($"[Arena Alteration] {gameObject.name} triggered Rage Phase events!");
                onRagePhaseBegin?.Invoke();
            }
        }

        private void EnterPhase(int phase)
        {
            currentPhase = phase;
            Debug.Log($"{gameObject.name} enters Phase {phase}!");
            
            // Trigger phase transition animation / invulnerability frames / roar
            
            // Adjust stats
            if (phase == 2)
            {
                attackRange *= 1.5f;
                moveSpeed *= 1.2f;
                skillCooldown *= 0.8f;
            }
            else if (phase == 3)
            {
                chaseSpeed *= 1.5f;
                skillCooldown *= 0.5f;
            }
        }

        protected override void ExecuteState()
        {
            // Inject boss skill usage before normal execution
            if (Time.time >= lastSkillTime + skillCooldown)
            {
                ExecuteRandomBossSkill();
                lastSkillTime = Time.time;
            }
            else
            {
                base.ExecuteState();
            }
        }

        private void ExecuteRandomBossSkill()
        {
            if (currentPhase == 1)
            {
                Debug.Log($"{gameObject.name} used basic Heavy Strike!");
            }
            else if (currentPhase == 2)
            {
                GroundPound();
            }
            else if (currentPhase == 3)
            {
                if (Random.value > 0.5f) GroundPound();
                else BulletHell();
            }
        }

        private void GroundPound()
        {
            Debug.Log($"{gameObject.name} performs a Ground Pound!");
            // Trigger Camera Shake
            if (ShadowRace.Core.CameraShake.Instance != null)
                ShadowRace.Core.CameraShake.Instance.Shake(2f, 0.5f);
            
            // Damage nearby players
            Collider2D[] hitPlayers = Physics2D.OverlapCircleAll(transform.position, attackRange * 2f, playerLayer);
            foreach (Collider2D p in hitPlayers)
            {
                ShadowRace.Player.PlayerStats stats = p.GetComponent<ShadowRace.Player.PlayerStats>();
                if (stats != null) stats.TakeDamage(30f);
            }
        }

        private void BulletHell()
        {
            Debug.Log($"{gameObject.name} unleashes Bullet Hell!");
            if (bossProjectile == null || bossFirePoint == null) return;

            float angleStep = 360f / bulletCount;
            float angle = 0f;

            for (int i = 0; i < bulletCount; i++)
            {
                float projDirX = bossFirePoint.position.x + Mathf.Sin((angle * Mathf.PI) / 180f);
                float projDirY = bossFirePoint.position.y + Mathf.Cos((angle * Mathf.PI) / 180f);
                
                Vector3 projMoveVector = new Vector3(projDirX, projDirY, 0f);
                Vector2 projDir = (projMoveVector - bossFirePoint.position).normalized;

                GameObject proj = Instantiate(bossProjectile, bossFirePoint.position, Quaternion.identity);
                proj.transform.right = projDir;
                
                ShadowRace.Combat.Projectile projectileScript = proj.GetComponent<ShadowRace.Combat.Projectile>();
                if (projectileScript != null)
                {
                    projectileScript.Initialize(15f, 10f, ShadowRace.Combat.ElementType.None);
                }

                angle += angleStep;
            }
        }
    }
}
