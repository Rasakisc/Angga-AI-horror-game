using System;
using UnityEngine;
using UnityEngine.InputSystem;
using GameInput = global::InputSystem;

namespace Game.Gameplay
{
    public class PlayerInputHandler : MonoBehaviour
    {
        private GameInput _input;
        private InputAction _flashlightAction;

        public Vector2 Move { get; private set; }
        public Vector2 Look { get; private set; }
        public bool IsSprintHeld { get; private set; }

        public event Action OnJump;
        public event Action OnInteract;
        public event Action OnFlashlightToggle;

        private void Awake()
        {
            _input = new GameInput();
            _flashlightAction = new InputAction("Flashlight", InputActionType.Button, "<Keyboard>/f");
            _flashlightAction.AddBinding("<Gamepad>/rightShoulder");
        }

        private void OnEnable()
        {
            _input.Player.Enable();
            _input.Player.Jump.performed += HandleJump;
            _flashlightAction.Enable();
            _flashlightAction.performed += HandleFlashlight;
        }

        private void OnDisable()
        {
            _input.Player.Jump.performed -= HandleJump;
            _input.Player.Disable();
            _flashlightAction.performed -= HandleFlashlight;
            _flashlightAction.Disable();
        }

        private void OnDestroy()
        {
            _input?.Dispose();
            _flashlightAction?.Dispose();
        }

        private void HandleFlashlight(InputAction.CallbackContext context)
        {
            OnFlashlightToggle?.Invoke();
        }

        private void Update()
        {
            Move = _input.Player.Move.ReadValue<Vector2>();
            Look = _input.Player.Look.ReadValue<Vector2>();
            IsSprintHeld = _input.Player.Sprint.IsPressed();

            if (_input.Player.Interact.WasPressedThisFrame())
            {
                OnInteract?.Invoke();
            }
        }

        private void HandleJump(InputAction.CallbackContext context)
        {
            OnJump?.Invoke();
        }
    }
}
