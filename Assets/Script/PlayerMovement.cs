using UnityEngine;

namespace Game.Gameplay
{
    [RequireComponent(typeof(CharacterController))]
    public class PlayerMovement : MonoBehaviour
    {
        [SerializeField] private PlayerInputHandler _input;
        [SerializeField] private PlayerStamina _stamina;
        [SerializeField] private float _walkSpeed = 3f;
        [SerializeField] private float _sprintSpeed = 6f;
        [SerializeField] private float _jumpHeight = 1.2f;
        [SerializeField] private float _gravity = -20f;

        private CharacterController _controller;
        private float _verticalVelocity;

        private void Awake()
        {
            _controller = GetComponent<CharacterController>();
        }

        private void OnEnable()
        {
            if (_input != null)
            {
                _input.OnJump += HandleJump;
            }
        }

        private void OnDisable()
        {
            if (_input != null)
            {
                _input.OnJump -= HandleJump;
            }
        }

        private void Update()
        {
            if (_input == null || _stamina == null) return;

            Vector2 move = _input.Move;
            bool isMoving = move.sqrMagnitude > 0.01f;

            bool isSprinting = _input.IsSprintHeld && isMoving && _stamina.CanSprint;
            _stamina.SetSprinting(isSprinting);

            float speed = isSprinting ? _sprintSpeed : _walkSpeed;
            Vector3 direction = transform.right * move.x + transform.forward * move.y;
            direction = Vector3.ClampMagnitude(direction, 1f);

            if (_controller.isGrounded && _verticalVelocity < 0f)
            {
                _verticalVelocity = -2f;
            }
            _verticalVelocity += _gravity * Time.deltaTime;

            Vector3 velocity = direction * speed + Vector3.up * _verticalVelocity;
            _controller.Move(velocity * Time.deltaTime);
        }

        private void HandleJump()
        {
            if (!_controller.isGrounded) return;

            _verticalVelocity = Mathf.Sqrt(_jumpHeight * -2f * _gravity);
        }
    }
}
