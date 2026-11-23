using UnityEngine;

namespace Game.Gameplay
{
    public class FlashlightController : MonoBehaviour
    {
        [SerializeField] private PlayerInputHandler _input;
        [SerializeField] private Light _light;
        [SerializeField] private AudioSource _audioSource;
        [SerializeField] private AudioClip _toggleClip;
        [SerializeField] private bool _startOn;

        public bool IsOn => _light != null && _light.enabled;

        private void Start()
        {
            if (_light != null)
            {
                _light.enabled = _startOn;
            }
        }

        private void OnEnable()
        {
            if (_input != null)
            {
                _input.OnFlashlightToggle += Toggle;
            }
        }

        private void OnDisable()
        {
            if (_input != null)
            {
                _input.OnFlashlightToggle -= Toggle;
            }
        }

        public void Toggle()
        {
            if (_light == null) return;

            _light.enabled = !_light.enabled;
            if (_audioSource != null && _toggleClip != null)
            {
                _audioSource.PlayOneShot(_toggleClip);
            }
        }
    }
}
