using System;
using UnityEngine;
using UnityEngine.InputSystem;
using GameInput = global::InputSystem;

namespace Game.Gameplay
{
    public class PlayerInputHandler : MonoBehaviour
    {
        private GameInput _input;

        public Vector2 Move { get; private set; }
        public Vector2 Look { get; private set; }
        public bool IsSprintHeld { get; private set; }

        public event Action OnJump;

        private void Awake()
        {
            _input = new GameInput();
        }

        private void OnEnable()
        {
            _input.Player.Enable();
            _input.Player.Jump.performed += HandleJump;
        }

        private void OnDisable()
        {
            _input.Player.Jump.performed -= HandleJump;
            _input.Player.Disable();
        }

        private void OnDestroy()
        {
            _input?.Dispose();
        }

        private void Update()
        {
            Move = _input.Player.Move.ReadValue<Vector2>();
            Look = _input.Player.Look.ReadValue<Vector2>();
            IsSprintHeld = _input.Player.Sprint.IsPressed();
        }

        private void HandleJump(InputAction.CallbackContext context)
        {
            OnJump?.Invoke();
        }
    }
}
