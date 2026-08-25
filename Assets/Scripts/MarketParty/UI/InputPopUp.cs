using System;
using UnityEngine;
using UnityEngine.UI;

namespace MarketParty.UI
{
    public class InputPopUp : MonoBehaviour
    {
        [SerializeField]
        private Image _image;

        [SerializeField]
        private Image _pressProgressBar;

        private float _timer = 0f;
        private Action _destroyCallback;

        private void Update()
        {
            _timer += Time.deltaTime;

            if (_timer >= 1f)
            {
                _destroyCallback.Invoke();
                Destroy(gameObject);
            }
        }

        private void LateUpdate()
        {
            transform.forward = Camera.main.transform.forward;
        }

        public void Show(string inputName, Action destroyCallback)
        {
            var sprite = Resources.Load<Sprite>("Input/" + inputName);

            _image.sprite = sprite;

            ResetProgress();

            ContinueShow();

            _destroyCallback = destroyCallback;
        }

        public void ContinueShow()
        {
            _timer = 0f;
        }

        public bool IsProgressReached(float step, float final)
        {
            _timer = 0f;
            var newFillAmount = _pressProgressBar.fillAmount;

            newFillAmount += step / final;

            print($"li progress: {newFillAmount}");

            _pressProgressBar.fillAmount = newFillAmount;
            if (newFillAmount >= 1f)
            {
                return true;
            }

            return false;
        }

        public void ResetProgress()
        {
            _pressProgressBar.fillAmount = 0f;
        }
    }
}