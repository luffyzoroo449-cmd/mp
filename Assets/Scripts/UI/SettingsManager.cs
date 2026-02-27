using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;
using TMPro;

namespace ShadowRace.UI
{
    public class SettingsManager : MonoBehaviour
    {
        [Header("Audio")]
        public AudioMixer mainMixer; // Assign an AudioMixer in Unity
        public Slider masterVolumeSlider;
        public Slider sfxVolumeSlider;
        public Slider musicVolumeSlider;

        [Header("Graphics")]
        public TMP_Dropdown resolutionDropdown;
        public Toggle fullscreenToggle;

        private Resolution[] resolutions;

        private void Start()
        {
            InitializeGraphicsSettings();
            LoadSettings();
        }

        private void InitializeGraphicsSettings()
        {
            if (resolutionDropdown == null) return;

            resolutions = Screen.resolutions;
            resolutionDropdown.ClearOptions();

            List<string> options = new List<string>();
            int currentResIndex = 0;

            for (int i = 0; i < resolutions.Length; i++)
            {
                string option = resolutions[i].width + " x " + resolutions[i].height;
                options.Add(option);

                if (resolutions[i].width == Screen.currentResolution.width &&
                    resolutions[i].height == Screen.currentResolution.height)
                {
                    currentResIndex = i;
                }
            }

            resolutionDropdown.AddOptions(options);
            resolutionDropdown.value = PlayerPrefs.GetInt("ResIndex", currentResIndex);
            resolutionDropdown.RefreshShownValue();
        }

        public void SetResolution(int resolutionIndex)
        {
            Resolution res = resolutions[resolutionIndex];
            Screen.SetResolution(res.width, res.height, Screen.fullScreen);
            PlayerPrefs.SetInt("ResIndex", resolutionIndex);
        }

        public void SetFullscreen(bool isFullscreen)
        {
            Screen.fullScreen = isFullscreen;
            PlayerPrefs.SetInt("Fullscreen", isFullscreen ? 1 : 0);
        }

        public void SetMasterVolume(float volume)
        {
            if (mainMixer != null) mainMixer.SetFloat("MasterVol", volume);
            PlayerPrefs.SetFloat("MasterVol", volume);
        }

        public void SetMusicVolume(float volume)
        {
            if (mainMixer != null) mainMixer.SetFloat("MusicVol", volume);
            PlayerPrefs.SetFloat("MusicVol", volume);
        }

        public void SetSFXVolume(float volume)
        {
            if (mainMixer != null) mainMixer.SetFloat("SFXVol", volume);
            PlayerPrefs.SetFloat("SFXVol", volume);
        }

        private void LoadSettings()
        {
            if (masterVolumeSlider != null) masterVolumeSlider.value = PlayerPrefs.GetFloat("MasterVol", 0f);
            if (musicVolumeSlider != null) musicVolumeSlider.value = PlayerPrefs.GetFloat("MusicVol", 0f);
            if (sfxVolumeSlider != null) sfxVolumeSlider.value = PlayerPrefs.GetFloat("SFXVol", 0f);
            
            if (fullscreenToggle != null) fullscreenToggle.isOn = PlayerPrefs.GetInt("Fullscreen", 1) == 1;
            SetFullscreen(PlayerPrefs.GetInt("Fullscreen", 1) == 1);
        }
    }
}
