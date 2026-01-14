using System;
using DG.Tweening;
using UnityEngine;

namespace MarketParty.Managers
{
    public class MusicManager : GlobalSingleton<MusicManager>, IInitializable
    {
        private AudioSource _audioSource;

        /*
        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
        private static void InitBeforeScene()
        {
            var gameObject = new GameObject("MusicManager");
            gameObject.AddComponent<AudioSource>();
            gameObject.AddComponent<MusicManager>();
        }
        */

        public void Init()
        {
            _audioSource = GetComponent<AudioSource>();
        }

        public void PlayDefaultMusic()
        {
            _audioSource.loop = true;
            _audioSource.Play();
        }

        public void SpeedUpMusic()
        {
            _audioSource.pitch = 1.1f;
        }

        public void SetDefaultMusicSpeed()
        {
            _audioSource.pitch = 1f;
        }
    }
}