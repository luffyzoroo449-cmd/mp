using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using ShadowRace.Core;

namespace ShadowRace.World
{
    public class LevelTransition : MonoBehaviour
    {
        [Header("Transition Settings")]
        [Tooltip("The name of the next scene to load (e.g., Level_2)")]
        public string nextLevelName;
        public float delayBeforeLoad = 1.5f;

        [Header("UI Fader (Optional)")]
        [Tooltip("Assign an Animator that handles a black screen Fade Out")]
        public Animator fadeAnimator;

        private bool transitionStarted = false;

        private void OnTriggerEnter2D(Collider2D other)
        {
            if (other.CompareTag("Player") && !transitionStarted)
            {
                transitionStarted = true;
                StartCoroutine(TransitionRoutine());
            }
        }

        private IEnumerator TransitionRoutine()
        {
            Debug.Log($"Level Completed! Transitioning to {nextLevelName}...");
            
            // Trigger Fade Animation
            if (fadeAnimator != null)
            {
                fadeAnimator.SetTrigger("FadeOut");
            }
            
            // Increment the Level in GameManager
            if (GameManager.Instance != null)
            {
                GameManager.Instance.currentLevel++;
                GameManager.Instance.SaveGame();
            }

            // Wait for visual effect/juice
            yield return new WaitForSeconds(delayBeforeLoad);

            // Load next scene
            if (!string.IsNullOrEmpty(nextLevelName))
            {
                SceneManager.LoadScene(nextLevelName);
            }
            else
            {
                Debug.LogWarning("No next level name provided! Returning to Main Menu.");
                SceneManager.LoadScene("MainMenu");
            }
        }
    }
}
