using System.Collections;
using JetBrains.Annotations;
using MarketParty.Characters.Players.Pickables;
using MarketParty.Interactables;
using MarketParty.UI;
using UnityEngine;
using UnityEngine.InputSystem;

namespace MarketParty.Characters.Players
{
    public class Player : MonoBehaviour
    {
        private int _interactablesLayerMask;

        [SerializeField]
        private float _moveSpeed = 1.5f;

        [SerializeField]
        private float _moveSprintSpeed = 2.5f;

        [SerializeField]
        private float _interactDistance = 1f;

        private PlayerHands _playerHands;
        private Rigidbody _rigidbody;
        private Camera _mainCamera;
        private PlayerInput _playerInput;

        private Vector2 _moveDirection;
        private bool _isSprint;

        private InputPopUp _currentInputPopUp;
        private IInfoable _currentInfoable;
        private ISelectable _currentSelectable;

        public CharacterAnimator CharacterAnimator { get; private set; }

        private void Awake()
        {
            _interactablesLayerMask = LayerMask.GetMask("Interactables");

            // not working ... :(
            /*
            var gamepad = (DualShockGamepad)Gamepad.current;
            gamepad.SetLightBarColor(Color.purple);
            */

            _playerHands =  GetComponent<PlayerHands>();
            _rigidbody = GetComponent<Rigidbody>();
            _mainCamera = Camera.main;
            CharacterAnimator =  GetComponent<CharacterAnimator>();
            _playerInput = GetComponent<PlayerInput>();

            var characterActions = _playerInput.actions.FindActionMap("Character");
            var action = characterActions.FindAction("LongInteract");

            action.started += OnLongInteractStarted;
            action.canceled += OnLongInteractCanceled;
        }

        public void OnMove(InputValue value)
        {
            _moveDirection = value.Get<Vector2>();
        }

        public void OnSprint(InputValue value)
        {
            _isSprint = value.isPressed;
        }

        public void OnInteract(InputValue value)
        {
            if (Physics.Raycast(transform.position, transform.TransformDirection(Vector3.forward),
                    out var hit, _interactDistance, _interactablesLayerMask))
            {
                if (hit.collider.TryGetComponent(out IInteractable interactable))
                {
                    if (_playerHands.TryInteract(interactable))
                    {
                        CharacterAnimator.SetInteract();

                        return;
                    }
                }
                // todo: возможно лучше сделать через interactable.
                else if (hit.collider.TryGetComponent(out IPickable pickable))
                {
                    if (_playerHands.TryPick(pickable))
                    {
                        CharacterAnimator.SetPickUp();

                        return;
                    }
                }
            }

            CharacterAnimator.SetEmoteNo();
        }

        /*public void OnLongInteract(InputValue value)
        {
            if (Physics.Raycast(transform.position, transform.TransformDirection(Vector3.forward),
                    out var hit, _interactDistance, _interactablesLayerMask))
            {
                if (hit.collider.TryGetComponent(out ILongInteractable longInteractable))
                {
                    if (_playerHands.TryLongInteract(longInteractable))
                    {
                        CharacterAnimator.SetInteract();

                        return;
                    }
                }
            }

            CharacterAnimator.SetEmoteNo();
        }*/

        private ILongInteractable _currentInteractable = null;

        public void OnLongInteractStarted(InputAction.CallbackContext ctx)
        {
            print("li started");

            if (Physics.Raycast(transform.position, transform.TransformDirection(Vector3.forward),
                    out var hit, _interactDistance, _interactablesLayerMask))
            {
                if (hit.collider.TryGetComponent(out ILongInteractable longInteractable))
                {
                    _currentInteractable = longInteractable;

                    print("li progress");

                    StartCoroutine(ProgressLongInteractable());
                }
            }

            CharacterAnimator.SetEmoteNo();
        }

        public void OnLongInteractCanceled(InputAction.CallbackContext ctx)
        {
            _currentInteractable = null;
        }

        private IEnumerator ProgressLongInteractable()
        {
            while (_currentInteractable != null)
            {
                if (InputPopUpsManager.Instance.CurrentInputPopUp.IsProgressReached(Time.deltaTime,
                        _currentInteractable.TimeForInteract))
                {
                    if (_playerHands.TryLongInteract(_currentInteractable))
                    {
                        CharacterAnimator.SetInteract();

                        InputPopUpsManager.Instance.CurrentInputPopUp.ResetProgress();
                        _currentInteractable = null;
                    }
                }

                yield return null;
            }

            InputPopUpsManager.Instance.CurrentInputPopUp.ResetProgress();
        }

        public void OnThrow(InputValue value)
        {
            if (_playerHands.TryThrowCurrentPickable())
            {
                CharacterAnimator.SetThrow();

                return;
            }

            CharacterAnimator.SetEmoteNo();
        }

        private void FixedUpdate()
        {
            if (Physics.Raycast(transform.position, transform.TransformDirection(Vector3.forward),
                    out var hit, _interactDistance, _interactablesLayerMask))
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
                    InputPopUpsManager.Instance.ShowPlaystationSquareButton(transform);
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