using UnityEngine;

namespace Game.Gameplay
{
    public class PlayerLook : MonoBehaviour
    {
        [SerializeField] private PlayerInputHandler _input;
        [SerializeField] private Transform _cameraTransform;
        [SerializeField] private float _sensitivity = 0.1f;
        [SerializeField] private float _maxPitch = 85f;

        private float _pitch;

        private void Start()
        {
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
        }

        private void Update()
        {
            if (_input == null || _cameraTransform == null) return;

            Vector2 look = _input.Look * _sensitivity;

            transform.Rotate(Vector3.up * look.x);

            _pitch = Mathf.Clamp(_pitch - look.y, -_maxPitch, _maxPitch);
            _cameraTransform.localRotation = Quaternion.Euler(_pitch, 0f, 0f);
        }
    }
}
