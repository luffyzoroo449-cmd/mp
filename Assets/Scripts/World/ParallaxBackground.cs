using UnityEngine;

namespace ShadowRace.World
{
    public class ParallaxBackground : MonoBehaviour
    {
        [Header("Camera Reference")]
        public Transform mainCamera;
        
        [Header("Parallax Settings")]
        [Tooltip("Higher = further away. 0 = moves with camera. 1 = static background.")]
        public float parallaxFactor;
        
        [Tooltip("If true, the background will loop infinitely on the X axis.")]
        public bool isInfiniteLoop = true;

        private float startPosX;
        private float length;

        private void Start()
        {
            if (mainCamera == null)
            {
                mainCamera = Camera.main.transform;
            }

            startPosX = transform.position.x;
            
            // Assume the object has a SpriteRenderer to determine its length
            SpriteRenderer sprite = GetComponent<SpriteRenderer>();
            if (sprite != null)
            {
                length = sprite.bounds.size.x;
            }
        }

        private void Update()
        {
            if (mainCamera == null) return;

            // Calculate how far the camera has moved relative to the parallax factor
            float distance = mainCamera.position.x * parallaxFactor;
            
            // Move the background
            transform.position = new Vector3(startPosX + distance, transform.position.y, transform.position.z);

            if (isInfiniteLoop)
            {
                // Calculate how much of the camera's movement is *not* parallaxed
                float temp = mainCamera.position.x * (1 - parallaxFactor);

                // If the camera has moved past the right edge of the sprite
                if (temp > startPosX + length)
                {
                    startPosX += length;
                }
                // If the camera has moved past the left edge of the sprite
                else if (temp < startPosX - length)
                {
                    startPosX -= length;
                }
            }
        }
    }
}
