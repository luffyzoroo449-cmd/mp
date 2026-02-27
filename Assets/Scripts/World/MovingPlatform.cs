using UnityEngine;

namespace ShadowRace.World
{
    public class MovingPlatform : MonoBehaviour
    {
        [Header("Path Settings")]
        public Transform[] waypoints;
        public float speed = 2f;
        public float waitTime = 1f;
        
        private int currentTargetIndex = 0;
        private float currentWaitTime = 0f;
        
        [Header("Player Tracking")]
        [Tooltip("True if we want the player to stick to the platform when riding")]
        public bool makesPlayerSticky = true;

        private void Start()
        {
            if (waypoints.Length > 0)
            {
                transform.position = waypoints[0].position;
            }
        }

        private void Update()
        {
            if (waypoints.Length == 0) return;

            Transform target = waypoints[currentTargetIndex];
            
            if (Vector2.Distance(transform.position, target.position) < 0.1f)
            {
                currentWaitTime += Time.deltaTime;
                if (currentWaitTime >= waitTime)
                {
                    currentTargetIndex = (currentTargetIndex + 1) % waypoints.Length;
                    currentWaitTime = 0f;
                }
            }
            else
            {
                transform.position = Vector2.MoveTowards(transform.position, target.position, speed * Time.deltaTime);
            }
        }

        private void OnCollisionEnter2D(Collision2D collision)
        {
            if (makesPlayerSticky && collision.gameObject.CompareTag("Player"))
            {
                // Parent the player to the platform so they move with it
                collision.transform.SetParent(this.transform);
            }
        }

        private void OnCollisionExit2D(Collision2D collision)
        {
            if (makesPlayerSticky && collision.gameObject.CompareTag("Player"))
            {
                // Unparent the player when they jump off
                collision.transform.SetParent(null);
            }
        }

        private void OnDrawGizmos()
        {
            if (waypoints == null || waypoints.Length < 2) return;
            
            Gizmos.color = Color.green;
            for (int i = 0; i < waypoints.Length; i++)
            {
                Transform nextPoint = waypoints[(i + 1) % waypoints.Length];
                Gizmos.DrawLine(waypoints[i].position, nextPoint.position);
            }
        }
    }
}
