using UnityEngine;

namespace ShadowRace.Core
{
    public class LevelManager : MonoBehaviour
    {
        [Header("Level Data")]
        public int levelID;
        public string regionName;
        public bool isBossLevel;

        [Header("Spawn Points")]
        public Transform startPos;
        public Transform currentCheckpoint;

        private GameObject playerInstance;

        private void Start()
        {
            // Spawn or reposition player at start
            playerInstance = GameObject.FindGameObjectWithTag("Player");
            
            if (playerInstance != null && startPos != null)
            {
                playerInstance.transform.position = startPos.position;
                currentCheckpoint = startPos;
            }

            Debug.Log($"Started Level {levelID} - Region: {regionName}");
        }

        public void SetCheckpoint(Transform newCheckpoint)
        {
            currentCheckpoint = newCheckpoint;
            Debug.Log("Checkpoint Reached!");
        }

        public void RespawnPlayer()
        {
            if (playerInstance != null && currentCheckpoint != null)
            {
                playerInstance.transform.position = currentCheckpoint.position;
                
                // Reset player HP/MP stats here
                // ShadowRace.Player.PlayerStats stats = playerInstance.GetComponent<ShadowRace.Player.PlayerStats>();
                // if (stats != null) stats.Heal(9999);
                
                Debug.Log("Player Respawned at Checkpoint.");
            }
        }

        public void FinishLevel()
        {
            // Trigger UI or transition
            Debug.Log("Triggering Level Finish sequence...");
            if (GameManager.Instance != null)
            {
                GameManager.Instance.CompleteLevel();
            }
        }
    }
}
