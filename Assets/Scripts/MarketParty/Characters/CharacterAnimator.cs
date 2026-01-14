using System;
using UnityEngine;

namespace MarketParty.Characters
{
    public class CharacterAnimator : MonoBehaviour
    {
        private readonly int _animatorVelocity = Animator.StringToHash("velocity");
        private readonly int _animatorInteract = Animator.StringToHash("interact");
        private readonly int _animatorEmoteNo = Animator.StringToHash("emoteNo");
        private readonly int _animatorIsSprint = Animator.StringToHash("isSprint");
        private readonly int _animatorIsSit = Animator.StringToHash("isSit");

        private Animator _animator;

        private void Awake()
        {
            _animator = GetComponent<Animator>();
        }

        public void SetVelocity(float velocity)
        {
            _animator.SetFloat(_animatorVelocity, velocity);
        }

        public void SetInteract()
        {
            _animator.SetTrigger(_animatorInteract);
        }

        public void SetEmoteNo()
        {
            _animator.SetTrigger(_animatorEmoteNo);
        }

        public void SetSprint(bool isSprint)
        {
            _animator.SetBool(_animatorIsSprint, isSprint);
        }
    }
}