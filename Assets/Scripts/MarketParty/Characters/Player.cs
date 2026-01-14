using System;
using MarketParty.Interactables;
using MarketParty.UI;
using UnityEngine;
using UnityEngine.InputSystem;

namespace MarketParty.Characters
{
    public class Player : MonoBehaviour
    {
        [SerializeField]
        private float _moveSpeed = 1.5f;

        [SerializeField]
        private float _moveSprintSpeed = 2.5f;

        [SerializeField]
        private float _interactDistance = 1f;

        private Rigidbody _rigidbody;
        private Camera _mainCamera;

        private Vector2 _moveDirection;
        private bool _isSprint;

        private InputPopUp _currentInputPopUp;
        private IInfoable _currentInfoable;
        private ISelectable _currentSelectable;

        public CharacterAnimator CharacterAnimator { get; private set; }

        private void Awake()
        {
            // not working ... :(
            /*
            var gamepad = (DualShockGamepad)Gamepad.current;
            gamepad.SetLightBarColor(Color.purple);
            */

            _rigidbody = GetComponent<Rigidbody>();
            _mainCamera = Camera.main;
            CharacterAnimator =  GetComponent<CharacterAnimator>();
        }

        public void OnMove(InputValue value)
        {
            _moveDirection = value.Get<Vector2>();
        }

        public void OnSprint(InputValue value)
        {
            _isSprint = value.isPressed;

            print(_isSprint);
        }

        public void OnInteract(InputValue value)
        {
            print("interact");

            CharacterAnimator.SetInteract();

            if (Physics.Raycast(transform.position, transform.TransformDirection(Vector3.forward), out var hit))
            {
                if (hit.collider.TryGetComponent(out IInteractable interactable))
                {
                    interactable.Interact(this);
                }
            }
        }

        public void OnLongInteract(InputValue value)
        {
            print("long interact");

            CharacterAnimator.SetInteract();

            if (Physics.Raycast(transform.position, transform.TransformDirection(Vector3.forward), out var hit))
            {
                if (hit.collider.TryGetComponent(out ILongInteractable longInteractable))
                {
                    longInteractable.LongInteract(this);
                }
            }
        }

        private void FixedUpdate()
        {
            if (Physics.Raycast(transform.position, transform.TransformDirection(Vector3.forward), out var hit, _interactDistance))
            {
                if (hit.collider.TryGetComponent<ISelectable>(out var selectable))
                {
                    if (_currentSelectable != selectable)
                    {
                        _currentSelectable?.Deselect();
                        _currentSelectable = selectable;
                    }

                    _currentSelectable.Select();
                }
                else
                {
                    _currentSelectable?.Deselect();
                    _currentSelectable = null;
                }


                if (hit.collider.TryGetComponent<IInfoable>(out var infoable))
                {
                    if (_currentInfoable != infoable)
                    {
                        _currentInfoable?.HideInfo();
                        _currentInfoable = infoable;
                    }

                    _currentInfoable.ShowInfo();
                }
                else
                {
                    _currentInfoable?.HideInfo();
                    _currentInfoable = null;
                }

                if (hit.collider.TryGetComponent<IInteractable>(out var interactable))
                {
                    InputPopUpsManager.Instance.ShowPlaystationCrossButton(transform);
                }

                if (hit.collider.TryGetComponent<ILongInteractable>(out var longInteractable))
                {
                    InputPopUpsManager.Instance.ShowPlaystationCrossButton(transform);
                }
            }
            else
            {
                _currentInfoable?.HideInfo();
                _currentInfoable = null;

                _currentSelectable?.Deselect();
                _currentSelectable = null;
            }

            if (_moveDirection.sqrMagnitude < 0.001f)
            {
                SetVelocity(Vector3.zero);

                return;
            }

            var camForward = _mainCamera.transform.forward;
            var camRight = _mainCamera.transform.right;

            camForward.y = 0f;
            camRight.y = 0f;

            camForward.Normalize();
            camRight.Normalize();

            var direction =
                camForward * _moveDirection.y +
                camRight * _moveDirection.x;

            var moveSpeed = _moveSpeed;

            if (_isSprint)
            {
                moveSpeed = _moveSprintSpeed;
            }

            SetVelocity(direction.normalized * moveSpeed);

            transform.forward = direction;
        }


        private void SetVelocity(Vector3 velocity)
        {
            _rigidbody.linearVelocity = velocity;

            CharacterAnimator.SetVelocity(_rigidbody.linearVelocity.sqrMagnitude);
            CharacterAnimator.SetSprint(_isSprint);
        }
    }
}