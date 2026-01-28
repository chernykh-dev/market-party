using System.Threading.Tasks;
using MarketParty.Managers;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace MarketParty.UI
{
    public class FallScreen : MonoBehaviour
    {
        [SerializeField]
        private TMP_Text _waveFailed;

        [SerializeField]
        private TMP_Text _earnedMoney;

        [SerializeField]
        private TMP_Text _receivedExperience;

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

        public void Show(int currentWave, int earnedMoney, int receivedExperience)
        {
            if (!_isShowing)
            {
                EventSystem.current.SetSelectedGameObject(_tryAgainButton.gameObject);
                _isShowing = true;
            }

            _waveFailed.text = $"WAVE {currentWave} FAILED!";
            _earnedMoney.text = $"Earned $ {earnedMoney}";
            _receivedExperience.text = $"Received {receivedExperience} XP";
        }

        private void TryAgain()
        {
            StartCoroutine(GameManager.Instance.LoadCurrentSceneAsync(
                () => GameManager.Instance.ResetGame()));
        }

        private void Exit()
        {
            print("Exit");
        }
    }
}