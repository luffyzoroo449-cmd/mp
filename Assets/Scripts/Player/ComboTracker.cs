using UnityEngine;
using System;

namespace ShadowRace.Player
{
    public class ComboTracker : MonoBehaviour
    {
        public static ComboTracker Instance { get; private set; }

        [Header("Combo Settings")]
        public int currentCombo = 0;
        public int highestComboThisLevel = 0;
        public float comboTimeout = 4f; // Time before combo resets
        private float lastHitTime;

        public event Action<int> OnComboChanged;
        public event Action OnComboReset;

        private void Awake()
        {
            if (Instance == null) Instance = this;
            else Destroy(gameObject);
        }

        private void Update()
        {
            if (currentCombo > 0 && Time.time > lastHitTime + comboTimeout)
            {
                ResetCombo();
            }
        }

        public void AddHit()
        {
            currentCombo++;
            if (currentCombo > highestComboThisLevel)
            {
                highestComboThisLevel = currentCombo;
            }
            lastHitTime = Time.time;
            OnComboChanged?.Invoke(currentCombo);
            
            // Trigger visual feedback (e.g., UI juice)
            // if (UIManager.Instance != null) UIManager.Instance.AnimateCombo();
        }

        public void ResetCombo()
        {
            if (currentCombo > 0)
            {
                currentCombo = 0;
                OnComboReset?.Invoke();
            }
        }

        public void ResetForNewLevel()
        {
            currentCombo = 0;
            highestComboThisLevel = 0;
            OnComboReset?.Invoke();
        }
    }
}
