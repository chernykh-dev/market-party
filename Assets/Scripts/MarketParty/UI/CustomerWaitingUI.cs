using System;
using MarketParty.Characters;
using MarketParty.Characters.Customers;
using UnityEngine;
using UnityEngine.UI;

namespace MarketParty.UI
{
    public class CustomerWaitingUI : MonoBehaviour
    {
        [SerializeField]
        private Image _waitingImage;

        [SerializeField]
        private Color _startColor;

        [SerializeField]
        private Color _endColor;

        [SerializeField]
        private float _waitingTime = 10f;

        private float _timer = 0f;

        private Customer _customer;

        private void Update()
        {
            if (_timer >= _waitingTime)
            {
                NotWaiting();

                return;
            }

            _timer += Time.deltaTime;

            _waitingImage.fillAmount = 1f - _timer / _waitingTime;

            _waitingImage.color = Color.Lerp(_startColor, _endColor, _timer / _waitingTime);
        }

        private void LateUpdate()
        {
            transform.forward = Camera.main.transform.forward;
        }

        private void NotWaiting()
        {
            _customer.FailCustomer();

            Destroy(gameObject);
        }

        public void Setup(Customer customer)
        {
            _customer = customer;
        }
    }
}