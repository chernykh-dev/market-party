using System;
using UnityEngine;

namespace MarketParty
{
    public class DoorTrigger : MonoBehaviour
    {
        private Animation _animation;

        private int _customersOnTrigger = 0;

        private void Awake()
        {
            _animation = GetComponentInParent<Animation>();
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag("Customer"))
            {
                if (_customersOnTrigger == 0)
                {
                    _animation.Play("open");
                }

                _customersOnTrigger++;
            }
        }

        private void OnTriggerExit(Collider other)
        {
            if (other.CompareTag("Customer"))
            {
                _customersOnTrigger--;

                if (_customersOnTrigger == 0)
                {
                    _animation.Play("close");
                }
            }
        }
    }
}