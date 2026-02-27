using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace ShadowRace.World
{
    public class LevelChunkLoader : MonoBehaviour
    {
        [Header("Chunk Settings")]
        [Tooltip("The name of the Unity Scene to load when walking into this trigger.")]
        public string chunkSceneToLoad;
        [Tooltip("Optional: The name of the Unity Scene to unload when walking into this trigger.")]
        public string chunkSceneToUnload;

        private bool isLoading = false;
        private bool isUnloading = false;

        private void OnTriggerEnter2D(Collider2D other)
        {
            if (other.CompareTag("Player"))
            {
                // Unload old chunk behind the player
                if (!string.IsNullOrEmpty(chunkSceneToUnload) && !isUnloading)
                {
                    StartCoroutine(UnloadChunkAsync(chunkSceneToUnload));
                }

                // Load new chunk ahead of the player
                if (!string.IsNullOrEmpty(chunkSceneToLoad) && !isLoading)
                {
                    StartCoroutine(LoadChunkAsync(chunkSceneToLoad));
                }
            }
        }

        private IEnumerator LoadChunkAsync(string sceneName)
        {
            isLoading = true;
            // Additive so we don't wipe out the Managers or current Chunk
            AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(sceneName, LoadSceneMode.Additive);

            while (asyncLoad != null && !asyncLoad.isDone)
            {
                yield return null;
            }

            Debug.Log($"[ChunkLoader] Successfully streamed in {sceneName}");
        }

        private IEnumerator UnloadChunkAsync(string sceneName)
        {
            isUnloading = true;
            
            // Check if it is actually loaded before trying to unload
            Scene sceneToUnload = SceneManager.GetSceneByName(sceneName);
            if (sceneToUnload.isLoaded)
            {
                AsyncOperation asyncUnload = SceneManager.UnloadSceneAsync(sceneName);

                while (asyncUnload != null && !asyncUnload.isDone)
                {
                    yield return null;
                }
                Debug.Log($"[ChunkLoader] Successfully unloaded {sceneName}");
            }
            else
            {
                isUnloading = false; // Reset if it wasn't loaded
            }
        }
    }
}
