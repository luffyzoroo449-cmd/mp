using UnityEngine;

namespace ShadowRace.Audio
{
    public class AudioManager : MonoBehaviour
    {
        public static AudioManager Instance { get; private set; }

        [Header("Audio Sources")]
        public AudioSource bgmSource;
        public AudioSource sfxSource;

        [Header("Background Music Tracks")]
        public AudioClip villageTheme;
        public AudioClip bossTheme;
        public AudioClip lavaTheme;
        public AudioClip snowTheme;
        public AudioClip darkRealmTheme;

        [Header("Sound Effects")]
        public AudioClip weaponFire;
        public AudioClip weaponReload;
        public AudioClip enemyAlert;
        public AudioClip uiClick;
        public AudioClip footstepGrass;
        public AudioClip footstepStone;

        private void Awake()
        {
            if (Instance == null)
            {
                Instance = this;
                DontDestroyOnLoad(gameObject);
            }
            else
            {
                Destroy(gameObject);
            }
        }

        public void PlayBGM(AudioClip clip)
        {
            if (bgmSource.clip == clip) return; // Already playing

            bgmSource.Stop();
            bgmSource.clip = clip;
            bgmSource.loop = true;
            bgmSource.Play();
        }

        public void PlaySFX(AudioClip clip, float volume = 1f)
        {
            sfxSource.PlayOneShot(clip, volume);
        }

        // Helper methods for explicit themes
        public void PlayVillageTheme() => PlayBGM(villageTheme);
        public void PlayBossTheme() => PlayBGM(bossTheme);
        public void PlayLavaTheme() => PlayBGM(lavaTheme);

        // Helper methods for explicit SFX
        public void PlayWeaponFire() => PlaySFX(weaponFire);
        public void PlayEnemyAlert() => PlaySFX(enemyAlert);
        public void PlayUIClick() => PlaySFX(uiClick);
    }
}
