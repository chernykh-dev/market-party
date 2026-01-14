using System;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

namespace MarketParty.UI
{
    public class EmotePopUp : MonoBehaviour
    {
        private const float _fadeSpeed = 0.5f;
        private const float _fadeTime = 2f;

        [SerializeField]
        private Image _image;

        private void Start()
        {
            Destroy(gameObject, _fadeTime + 1f);
        }

        private void Update()
        {
            transform.position += transform.up * Time.deltaTime * _fadeSpeed;
        }

        private void LateUpdate()
        {
            transform.forward = Camera.main.transform.forward;
        }

        public void Show(string emoteName)
        {
            var sprite = Resources.Load<Sprite>("Emotes/emote_" + emoteName);

            _image.sprite = sprite;

            _image.DOFade(0f, _fadeTime);
        }
    }
}