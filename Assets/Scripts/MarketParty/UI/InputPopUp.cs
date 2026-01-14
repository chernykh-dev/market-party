using System;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

namespace MarketParty.UI
{
    public class InputPopUp : MonoBehaviour
    {
        [SerializeField]
        private Image _image;

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

            ContinueShow();

            _destroyCallback = destroyCallback;
        }

        public void ContinueShow()
        {
            _timer = 0f;
        }
    }
}