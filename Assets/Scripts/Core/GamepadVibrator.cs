using UnityEngine;
using UnityEngine.InputSystem;

namespace ShadowRace.Core
{
    public class GamepadVibrator : MonoBehaviour
    {
        public static GamepadVibrator Instance { get; private set; }

        private void Awake()
        {
            if (Instance == null) Instance = this;
        }

        public void Vibrate(float lowFrequency, float highFrequency, float duration)
        {
            Gamepad gamepad = Gamepad.current;
            if (gamepad != null)
            {
                gamepad.SetMotorSpeeds(lowFrequency, highFrequency);
                StartCoroutine(StopVibration(duration, gamepad));
            }
        }

        private System.Collections.IEnumerator StopVibration(float duration, Gamepad gamepad)
        {
            yield return new WaitForSecondsRealtime(duration);
            gamepad.SetMotorSpeeds(0f, 0f);
        }
        
        private void OnApplicationQuit()
        {
            Gamepad gamepad = Gamepad.current;
            if (gamepad != null)
            {
                gamepad.SetMotorSpeeds(0f, 0f); // Ensure motors turn off when exiting
            }
        }
    }
}
