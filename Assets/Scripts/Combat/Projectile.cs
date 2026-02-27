using UnityEngine;

namespace ShadowRace.Combat
{
    [RequireComponent(typeof(Rigidbody2D))]
    public class Projectile : MonoBehaviour
    {
        public float speed = 20f;
        public float damage = 10f;
        public float lifetime = 3f;
        public LayerMask hitLayers;
        
        public ElementType elementType;

        private Rigidbody2D rb;

        private void Start()
        {
            rb = GetComponent<Rigidbody2D>();
            rb.velocity = transform.right * speed;
            Destroy(gameObject, lifetime);
        }

        private void OnTriggerEnter2D(Collider2D hitInfo)
        {
            if (((1 << hitInfo.gameObject.layer) & hitLayers) != 0)
            {
                Debug.Log($"Projectile hit {hitInfo.name} for {damage} {elementType} damage.");
                
                // If enemy, apply damage
                // EnemyStats enemy = hitInfo.GetComponent<EnemyStats>();
                // if (enemy != null) enemy.TakeDamage(damage, elementType);

                // Spawn impact VFX
                // Destroy Projectile
                Destroy(gameObject);
            }
        }

        public void Initialize(float pDamage, float pSpeed, ElementType pElement)
        {
            damage = pDamage;
            speed = pSpeed;
            elementType = pElement;
        }
    }
}
