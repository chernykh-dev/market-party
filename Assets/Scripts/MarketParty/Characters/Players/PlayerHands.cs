using System;
using DG.Tweening;
using MarketParty.Characters.Players.Pickables;
using MarketParty.Interactables;
using UnityEngine;

namespace MarketParty.Characters.Players
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

        Vector3 prevPos;

        public Vector3 Velocity { get; private set; }

        private void FixedUpdate()
        {
            if (CurrentPickable == _emptyHands)
            {
                return;
            }

            var rb = CurrentPickable.gameObject.GetComponent<Rigidbody>();

            var t = 1f - Mathf.Exp(-15f * Time.fixedDeltaTime);

            var pos = Vector3.Lerp(rb.position, _handsPivot.position, t);
            var rot = Quaternion.Slerp(rb.rotation, _handsPivot.rotation, t);

            rb.MovePosition(pos);
            rb.MoveRotation(rot);

            Velocity = (pos - prevPos) / Time.fixedDeltaTime;
            prevPos = pos;
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

            CurrentPickable = pickable;

            pickable.gameObject.transform.SetParent(_handsPivot);
            // todo rotate to nearest (0, 180) for mirrored-z objects.
            pickable.gameObject.transform
                .DOLocalRotateQuaternion(Quaternion.identity, PickDuration);
            pickable.gameObject.transform
                .DOLocalJump(Vector3.zero, PickJumpForce, PickJumpsCount, PickDuration)
                .OnComplete(() =>
                {
                    pickable.gameObject.transform.SetParent(null);
                    prevPos = pickable.gameObject.GetComponent<Rigidbody>().position;
                });

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