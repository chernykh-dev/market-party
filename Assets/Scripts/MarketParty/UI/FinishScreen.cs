using System;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace MarketParty.UI
{
    public class FinishScreen : MonoBehaviour
    {
        [SerializeField]
        private GameObject[] _stars;

        [SerializeField]
        private TMP_Text _soldProducts;

        [SerializeField]
        private TMP_Text _earnedMoney;

        [SerializeField]
        private TMP_Text _receivedExperience;

        [SerializeField]
        private TMP_Text _productsToNextStar;

        [SerializeField]
        private Button _tryAgainButton;

        [SerializeField]
        private Button _exitButton;

        private bool _isShowing;

        private void OnEnable()
        {
            _tryAgainButton.onClick.AddListener(TryAgain);

            _exitButton.onClick.AddListener(Exit);
        }

        private void OnDisable()
        {
            _tryAgainButton.onClick.RemoveAllListeners();

            _exitButton.onClick.RemoveAllListeners();
        }

        public void Show(int starsCount, int soldProducts, int earnedMoney, int receivedExperience, int? productsToNextStar)
        {
            if (!_isShowing)
            {
                EventSystem.current.SetSelectedGameObject(_tryAgainButton.gameObject);
                _isShowing = true;
            }

            for (var i = 0; i < starsCount; i++)
            {
                _stars[i].SetActive(true);
            }

            _soldProducts.text = $"Sold {soldProducts} products";
            _earnedMoney.text = $"Earned $ {earnedMoney}";
            _receivedExperience.text = $"Received {receivedExperience} XP";

            _productsToNextStar.text = productsToNextStar.HasValue
                ? $"Products to Next Star: {productsToNextStar}"
                : "";
        }

        private void TryAgain()
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }

        private void Exit()
        {
            print("Exit");
        }
    }
}