using System.Collections.Generic;
using UnityEngine;

namespace ShadowRace.Core
{
    public class DistanceCullingManager : MonoBehaviour
    {
        public static DistanceCullingManager Instance { get; private set; }

        [Header("Culling Settings")]
        public Transform playerTarget;
        [Tooltip("Distance at which objects are completely disabled (e.g., 200 units)")]
        public float cullingDistance = 200f;
        [Tooltip("How often to check distances (in seconds) to save CPU. 0.5s is usually fine.")]
        public float checkInterval = 0.5f;

        private float nextCheckTime = 0f;

        // Using a HashSet for fast O(1) removal
        private HashSet<CullableEntity> cullableEntities = new HashSet<CullableEntity>();

        private void Awake()
        {
            if (Instance == null) Instance = this;
            else Destroy(gameObject);
        }

        private void Start()
        {
            if (playerTarget == null)
            {
                GameObject p = GameObject.FindGameObjectWithTag("Player");
                if (p != null) playerTarget = p.transform;
            }
        }

        private void Update()
        {
            if (Time.time >= nextCheckTime)
            {
                PerformCulling();
                nextCheckTime = Time.time + checkInterval;
            }
        }

        public void RegisterEntity(CullableEntity entity)
        {
            cullableEntities.Add(entity);
        }

        public void UnregisterEntity(CullableEntity entity)
        {
            cullableEntities.Remove(entity);
        }

        private void PerformCulling()
        {
            if (playerTarget == null) return;

            Vector2 playerPos = playerTarget.position;
            float sqrCullingDistance = cullingDistance * cullingDistance;

            // To avoid modifying collection while iterating, we remove nulls manually via a list if needed, 
            // but for performance we just rely on UnregisterEntity.
            foreach (var entity in cullableEntities)
            {
                if (entity == null || entity.gameObject == null) continue; // Safety check

                float sqrDist = (playerPos - (Vector2)entity.transform.position).sqrMagnitude;
                
                if (sqrDist > sqrCullingDistance && entity.isActiveAndEnabled)
                {
                    entity.SetCulledState(true); // Too far, freeze it
                }
                else if (sqrDist <= sqrCullingDistance && !entity.isActiveAndEnabled)
                {
                    entity.SetCulledState(false); // Close enough, wake it up
                }
            }
        }
    }
}
