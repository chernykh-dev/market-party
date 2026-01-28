using System;
using DG.Tweening;
using MarketParty.Characters;
using MarketParty.UI;
using TMPro;
using UnityEngine;

namespace MarketParty.Managers
{
    public class LevelManager : Singleton<LevelManager>, IInitializable
    {
        private const float START_TIMER = 60f;

        [SerializeField]
        private FinishScreen _finishScreen;

        [SerializeField]
        private FallScreen _fallScreen;

        [SerializeField]
        private TMP_Text _timerText;

        private int _currentWave = 1;

        private int _servedCustomers;
        private int _failedCustomers;

        private int _visitedCustomers => _servedCustomers + _failedCustomers;
        private int _expectedCustomers;

        private int _currentProducts;
        private int _expectedProducts;

        private int _earnedMoney = 0;
        private int _receivedExperience = 0;

        private float _timer = START_TIMER;

        private bool _musicSpeedUpped = false;
        private bool _finished = false;

        public void Init()
        {
            _timer = START_TIMER;

            MusicManager.Instance.SetDefaultMusicSpeed();
        }

        public void SetWave(int currentWave, float timeForAdd)
        {
            _timer += timeForAdd;
            _currentWave = currentWave;
        }

        private void Update()
        {
            if (_finished)
            {
                return;
            }

            if (_timer <= 0.01f)
            {
                CalculateLevelPoints();

                _timerText.text = "0 : 0";

                return;
            }

            if (_timer <= 31f && !_musicSpeedUpped)
            {
                MusicManager.Instance.SpeedUpMusic();
                _musicSpeedUpped = true;
            }

            _timer -= Time.deltaTime;
            _timerText.text = $"{Mathf.FloorToInt(_timer / 60f)} : {Mathf.FloorToInt(_timer % 60f)}";
        }

        public void AddCustomer(Customer customer)
        {
            _expectedCustomers++;
            _expectedProducts += customer.ExpectedProducts;
        }

        public void ServeCustomer(Customer customer)
        {
            AddEarnedMoney(customer.TakedProducts * 10);

            _servedCustomers++;
            _currentProducts += customer.TakedProducts;

            if (_visitedCustomers == _expectedCustomers)
            {
                CalculateLevelPoints();
            }
        }

        public void FailCustomer(Customer customer)
        {
            AddEarnedMoney(-customer.TakedProducts * 10 / 2);

            _failedCustomers++;

            if (_visitedCustomers == _expectedCustomers)
            {
                CalculateLevelPoints();
            }
        }

        public void AddEarnedMoney(int earnedMoney)
        {
            MoneyManager.Instance.AddMoney(earnedMoney);

            _earnedMoney += earnedMoney;
        }

        public void AddReceivedExperience(int receivedExperience)
        {
            ExperienceManager.Instance.AddExperience(receivedExperience);

            _receivedExperience += receivedExperience;
        }

        private void CalculateLevelPoints()
        {
            _finished = true;

            var efficientRatio = (float)_currentProducts / _expectedProducts;

            var starsCount = 0;
            var productsToNextStar = new int?();

            if (efficientRatio >= 0.5f)
                starsCount++;
            else
                productsToNextStar = Mathf.CeilToInt(_expectedProducts * 0.5f);

            if (efficientRatio >= 0.75f)
                starsCount++;
            else if (!productsToNextStar.HasValue)
                productsToNextStar = Mathf.CeilToInt(_expectedProducts * 0.75f);

            if (efficientRatio >= 0.95f)
                starsCount++;
            else if (!productsToNextStar.HasValue)
                productsToNextStar = Mathf.CeilToInt(_expectedProducts * 0.95f);

            if (starsCount > 0)
            {
                ShowFinishScreen(starsCount, productsToNextStar);
            }
            else
            {
                ShowFallScreen();
            }

            PlayersManager.Instance.DisablePlayerInputs();
        }

        private void ShowFinishScreen(int starsCount, int? productsToNextStar)
        {
            _finishScreen.gameObject.SetActive(true);
            _finishScreen.Show(_currentWave, starsCount, _currentProducts, _earnedMoney, _receivedExperience, productsToNextStar);
        }

        private void ShowFallScreen()
        {
            _finished = true;

            _fallScreen.gameObject.SetActive(true);
            _fallScreen.Show(_currentWave, _earnedMoney, _receivedExperience);

            PlayersManager.Instance.DisablePlayerInputs();
        }
    }
}