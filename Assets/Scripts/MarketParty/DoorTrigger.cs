using System;
using JetBrains.Annotations;
using UnityEngine;

namespace MarketParty
{
    public class DoorTrigger : MonoBehaviour
    {
        [SerializeField]
        private CharacterTag _tagForTrigger;

        [SerializeField]
        [CanBeNull]
        private Collider _colliderForDisable;

        private Animation _animation;

        private int _charactersOnTrigger = 0;

        private void Awake()
        {
            _animation = GetComponentInParent<Animation>();
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag(_tagForTrigger.ToString()))
            {
                if (_charactersOnTrigger == 0)
                {
                    _animation.Play("open");

                    if (_colliderForDisable)
                    {
                        _colliderForDisable.enabled = false;
                    }
                }

                _charactersOnTrigger++;
            }
        }

        private void OnTriggerExit(Collider other)
        {
            if (other.CompareTag(_tagForTrigger.ToString()))
            {
                _charactersOnTrigger--;

                if (_charactersOnTrigger == 0)
                {
                    _animation.Play("close");

                    if (_colliderForDisable)
                    {
                        _colliderForDisable.enabled = true;
                    }
                }
            }
        }
    }
}