using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

namespace ZhukovskyGamesPlugin {
    public class Audio : MonoBehaviour {
        [SerializeField]
        private bool IsAutoMusic;

        [SerializeField]
        private AudioSource musicSource;

        [SerializeField]
        private AudioSource[] effectsSource;

        [SerializeField]
        private AudioSo _audioSo;

        private SerializableDictionary<Music, AudioClip> MusicClips => _audioSo._musicClips;
        private SerializableDictionary<Sounds, AudioClip> SoundClips => _audioSo._soundClips;

        private int _curSource;
        private bool _isInitialized;

        private void Update() {
            if (!musicSource.isPlaying && _isInitialized && IsAutoMusic && MusicClips.Count > 0) {
                PlayRandomMusic();
            }
        }

        public void Init() {
            _isInitialized = true;
        }

        public void ChangeVolume(float master, float music, float effects) {
            musicSource.volume = master * music;
            foreach (AudioSource source in effectsSource) source.volume = master * effects;
        }

        private void NextSource() {
            _curSource++;
            if (_curSource > effectsSource.Length - 1)
                _curSource = 0;
        }

        public void PlayRandomMusic() {
            musicSource.clip = MusicClips.Values[Random.Range(0, MusicClips.Values.Count)];
            musicSource.Play();
        }

        public void PlayMusic(Music music) {
            if (MusicClips.ContainsKey(music)) {
                musicSource.clip = MusicClips[music];
            }
        }

        public void PlaySound(Sounds sound) // 0 - click, 1 - clickWrong, 2 - clickButton
        {
            if (!SoundClips.ContainsKey(sound)) {
                return;
            }

            NextSource();

            AudioClip clip = SoundClips[sound];
            effectsSource[_curSource].clip = clip;
            effectsSource[_curSource].Play();
        }
    }

    [SerializeField]
    public class ClipsList {
        public List<AudioClip> List = new List<AudioClip>();
    }
}