using System;
using DG.Tweening;
using MarketParty.Characters;
using MarketParty.Interactables;
using MarketParty.Players.Pickables;
using UnityEngine;

namespace MarketParty.Players
{
    public class PlayerHands : MonoBehaviour
    {
        private const float PickJumpForce = 0.5f;
        private const int PickJumpsCount = 1;
        private const float PickDuration = 0.5f;

        [SerializeField]
        private Transform _handsPivot;

        private IPickable _emptyHands;

        public IPickable CurrentPickable { get; private set; }

        public Player Player { get; private set; }

        private void Awake()
        {
            _emptyHands = GetComponent<IPickable>();

            CurrentPickable = _emptyHands;

            Player = GetComponent<Player>();
        }

        public bool TryInteract(IInteractable interactable)
        {
            return interactable.Interact(this);
        }

        public bool TryLongInteract(ILongInteractable longInteractable)
        {
            return longInteractable.LongInteract(this);
        }

        public bool TryPick(IPickable pickable)
        {
            // TODO: Продумать, можно ли брать в руки предмет, когда руки заняты. (Думаю, что нет - return false).
            if (CurrentPickable != _emptyHands)
            {
                return false;
            }

            print(pickable.gameObject.name);

            CurrentPickable = pickable;

            pickable.gameObject.transform.SetParent(_handsPivot);
            // todo rotate to nearest (0, 180) for mirrored-z objects.
            pickable.gameObject.transform.DOLocalRotateQuaternion(Quaternion.identity, PickDuration);
            pickable.gameObject.transform.DOLocalJump(Vector3.zero, PickJumpForce, PickJumpsCount, PickDuration);

            return true;
        }

        // todo use
        public bool TryThrowCurrentPickable()
        {
            if (CurrentPickable == _emptyHands)
            {
                return false;
            }

            CurrentPickable.gameObject.transform.SetParent(null);
            CurrentPickable.gameObject.transform.DOJump(transform.position + transform.forward, PickJumpForce,
                PickJumpsCount, PickDuration);

            CurrentPickable = _emptyHands;

            return true;
        }
    }
}