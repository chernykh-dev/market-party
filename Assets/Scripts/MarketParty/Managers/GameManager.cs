using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using MarketParty.DeadlineScripts;
using UnityEngine;
using UnityEngine.SceneManagement;
using Random = UnityEngine.Random;

namespace MarketParty.Managers
{
    public class GameManager : GlobalSingleton<GameManager>, IInitializable
    {
        [SerializeField]
        private float _waveCompletedAddTime = 15f;

        [SerializeField]
        private bool _tutorialWavesEnabled;

        [SerializeField]
        private List<float> _waveRatios;

        private int _currentWave = 1;
        private float _addedTime = 0;
        private Dictionary<int, List<WaveObjectActivator>> _waveObjects = new Dictionary<int, List<WaveObjectActivator>>();

        public void Init()
        {
            MusicManager.Instance.PlayDefaultMusic();

            ActivateWave(1);

            if (!_tutorialWavesEnabled)
            {
                StandsManager.Instance.FillStands(1);
            }
        }

        /*
        private void Awake()
        {
            var _ = Instance;
        }
        */

        public void NextWave()
        {
            _currentWave++;
            _addedTime += _waveCompletedAddTime;

            print($"{_currentWave}, {_addedTime}");

            LevelManager.Instance.SetWave(_currentWave, _addedTime);

            // TODO    Сначала добавляем посетителей, а потом корректируем время.
            CustomersSpawner.Instance.UpDifficult(_currentWave - 1);

            ActivateWave(_currentWave);

            if (!_tutorialWavesEnabled)
            {
                StandsManager.Instance.FillStands(1);
            }
        }

        public void ResetGame()
        {
            _currentWave = 1;
            _addedTime = 0f;

            MoneyManager.Instance.ResetMoney();
            ExperienceManager.Instance.ResetExperience();

            DeactivateAllWaves();
            ActivateWave(1);

            if (!_tutorialWavesEnabled)
            {
                StandsManager.Instance.FillStands(1);
            }
        }

        public IEnumerator LoadCurrentSceneAsync(Action onComplete)
        {
            // Start loading the scene in the background
            var asyncLoad = SceneManager.LoadSceneAsync(SceneManager.GetActiveScene().name);

            // Optional: prevent the scene from activating immediately
            // This is useful for creating a loading screen that waits for a user action or an animation
            // asyncLoad.allowSceneActivation = false;

            // Wait until the asynchronous scene fully loads (reaches 90%)
            while (!asyncLoad.isDone)
            {
                // You can use asyncLoad.progress to update a loading bar
                // float progress = Mathf.Clamp01(asyncLoad.progress / 0.9f);
                // Debug.Log("Loading progress: " + progress * 100 + "%");

                yield return null;
            }

            asyncLoad.allowSceneActivation = true;

            yield return null;

            onComplete?.Invoke();

            // If allowSceneActivation was set to false, set it to true when you want the scene to switch
            // asyncLoad.allowSceneActivation = true;
        }

        private void ActivateWave(int wave)
        {
            if (!_tutorialWavesEnabled)
            {
                return;
            }

            LoadWaveActivators();

            if (!_waveObjects.TryGetValue(wave, out var waveObjects))
            {
                return;
            }

            foreach (var obj in waveObjects)
            {
                obj.gameObject.SetActive(obj.active);
            }

            var ratio = wave - 1 >= _waveRatios.Count ? Random.Range(0f, 1f) : _waveRatios[wave - 1];
            StandsManager.Instance.FillStands(ratio);
        }

        private void DeactivateAllWaves()
        {
            if (!_tutorialWavesEnabled)
            {
                return;
            }

            LoadWaveActivators();

            foreach (var wave in _waveObjects)
            {
                foreach (var obj in wave.Value)
                {
                    obj.gameObject.SetActive(!obj.active);
                }
            }
        }

        private void LoadWaveActivators()
        {
            _waveObjects.Clear();

            var waveActivators = FindObjectsByType(typeof(WaveActivator), FindObjectsInactive.Include,
                FindObjectsSortMode.InstanceID).Cast<WaveActivator>();

            foreach (var waveActivator in waveActivators)
            {
                if (!_waveObjects.ContainsKey(waveActivator.Wave))
                {
                    _waveObjects.Add(waveActivator.Wave, new List<WaveObjectActivator>());
                }

                _waveObjects[waveActivator.Wave].Add(new WaveObjectActivator()
                {
                    active = waveActivator.Active,
                    gameObject = waveActivator.gameObject
                });
            }
        }
    }

    [System.Serializable]
    public class WaveObjectActivator
    {
        public bool active;

        public GameObject gameObject;
    }
}