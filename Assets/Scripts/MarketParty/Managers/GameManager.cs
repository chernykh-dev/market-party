using System;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace MarketParty.Managers
{
    public class GameManager : GlobalSingleton<GameManager>, IInitializable
    {
        [SerializeField]
        private float _waveCompletedAddTime = 15f;

        private int _currentWave = 1;
        private float _addedTime = 0;

        public void Init()
        {
            MusicManager.Instance.PlayDefaultMusic();
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
        }

        public void ResetGame()
        {
            _currentWave = 1;
            _addedTime = 0f;

            MoneyManager.Instance.ResetMoney();
            ExperienceManager.Instance.ResetExperience();
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
    }
}