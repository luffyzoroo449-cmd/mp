using UnityEngine;

namespace ShadowRace.Core
{
    [RequireComponent(typeof(SpriteRenderer), typeof(Animator))]
    public class CullableEntity : MonoBehaviour
    {
        private Animator anim;
        private Rigidbody2D rb;
        private MonoBehaviour[] allScripts;

        public bool isActiveAndEnabled { get; private set; } = true;

        private void Start()
        {
            anim = GetComponent<Animator>();
            rb = GetComponent<Rigidbody2D>();
            allScripts = GetComponents<MonoBehaviour>();

            if (DistanceCullingManager.Instance != null)
            {
                DistanceCullingManager.Instance.RegisterEntity(this);
            }
        }

        private void OnDestroy()
        {
            if (DistanceCullingManager.Instance != null)
            {
                DistanceCullingManager.Instance.UnregisterEntity(this);
            }
        }

        public void SetCulledState(bool isCulled)
        {
            isActiveAndEnabled = !isCulled;

            // Freeze animation and physics
            if (anim != null) anim.enabled = !isCulled;
            if (rb != null)
            {
                if (isCulled)
                {
                    rb.Sleep(); // Reduces physics overhead completely
                }
                else
                {
                    rb.WakeUp();
                }
            }

            // Disable custom scripts (e.g. EnemyBrain logic)
            foreach (var script in allScripts)
            {
                if (script != this) // Don't disable the CullableEntity script itself
                {
                    script.enabled = !isCulled;
                }
            }
        }
    }
}
