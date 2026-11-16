using System;
using UnityEngine;

namespace Game.Gameplay
{
    public class PlayerStamina : MonoBehaviour
    {
        [SerializeField] private float _maxStamina = 100f;
        [SerializeField] private float _drainPerSecond = 20f;
        [SerializeField] private float _regenPerSecond = 15f;
        [SerializeField] private float _regenDelay = 1f;
        [SerializeField] private float _exhaustRecoverThreshold = 25f;

        [Header("Pantau saat Play")]
        [SerializeField] private float _current;

        private float _regenTimer;
        private bool _isSprinting;
        private bool _isExhausted;

        public float Current => _current;
        public float Max => _maxStamina;
        public bool CanSprint => !_isExhausted && _current > 0f;

        public event Action<float> OnStaminaChanged;

        private void Awake()
        {
            _current = _maxStamina;
        }

        private void Start()
        {
            OnStaminaChanged?.Invoke(_current / _maxStamina);
        }

        public void SetSprinting(bool isSprinting)
        {
            _isSprinting = isSprinting && CanSprint;
        }

        private void Update()
        {
            float previous = _current;

            if (_isSprinting)
            {
                _current = Mathf.Max(0f, _current - _drainPerSecond * Time.deltaTime);
                _regenTimer = _regenDelay;

                if (_current <= 0f)
                {
                    _isExhausted = true;
                    _isSprinting = false;
                }
            }
            else if (_regenTimer > 0f)
            {
                _regenTimer -= Time.deltaTime;
            }
            else
            {
                _current = Mathf.Min(_maxStamina, _current + _regenPerSecond * Time.deltaTime);
                if (_isExhausted && _current >= _exhaustRecoverThreshold)
                {
                    _isExhausted = false;
                }
            }

            if (!Mathf.Approximately(previous, _current))
            {
                OnStaminaChanged?.Invoke(_current / _maxStamina);
            }
        }
    }
}
