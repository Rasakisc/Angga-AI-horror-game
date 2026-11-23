using System;
using UnityEngine;

namespace Game.Interaction
{
    public abstract class Door : MonoBehaviour, IInteractable
    {
        [SerializeField] private bool _isLocked;
        [SerializeField] private string _requiredKeyId;
        [SerializeField] private string _lockedMessage = "Pintu terkunci. Cari kuncinya";
        [SerializeField] private float _speed = 2.5f;
        [SerializeField] private AudioSource _audioSource;
        [SerializeField] private AudioClip _openClip;
        [SerializeField] private AudioClip _closeClip;
        [SerializeField] private AudioClip _lockedClip;

        private bool _isOpen;
        private float _progress;

        public bool IsOpen => _isOpen;
        public bool IsLocked => _isLocked;
        public string RequiredKeyId => _requiredKeyId;
        public string LockedMessage => _lockedMessage;

        public string InteractPrompt => _isLocked ? "Terkunci" : (_isOpen ? "Tutup" : "Buka");

        public static event Action<Door> LockedInteracted;
        public static event Action<Door> UnlockedWithKey;

        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.SubsystemRegistration)]
        private static void ResetStatics()
        {
            LockedInteracted = null;
            UnlockedWithKey = null;
        }

        protected virtual void Awake()
        {
            CacheClosedPose();
            ApplyPose(0f);
        }

        public void Interact(GameObject interactor)
        {
            if (_isLocked)
            {
                var inventory = interactor.GetComponent<Inventory>();
                if (inventory != null && !string.IsNullOrEmpty(_requiredKeyId) && inventory.Remove(_requiredKeyId))
                {
                    _isLocked = false;
                    UnlockedWithKey?.Invoke(this);
                }
                else
                {
                    Play(_lockedClip);
                    LockedInteracted?.Invoke(this);
                    return;
                }
            }

            if (!_isOpen)
            {
                OnBeforeOpen(interactor);
            }

            _isOpen = !_isOpen;
            Play(_isOpen ? _openClip : _closeClip);
        }

        public bool TryUnlock(string keyId)
        {
            if (!_isLocked || string.IsNullOrEmpty(_requiredKeyId) || keyId != _requiredKeyId) return false;

            _isLocked = false;
            return true;
        }

        private void Update()
        {
            float target = _isOpen ? 1f : 0f;
            if (Mathf.Approximately(_progress, target)) return;

            _progress = Mathf.MoveTowards(_progress, target, _speed * Time.deltaTime);
            ApplyPose(Mathf.SmoothStep(0f, 1f, _progress));
        }

        protected virtual void OnBeforeOpen(GameObject interactor)
        {
        }

        protected abstract void CacheClosedPose();

        protected abstract void ApplyPose(float t);

        private void Play(AudioClip clip)
        {
            if (_audioSource != null && clip != null)
            {
                _audioSource.PlayOneShot(clip);
            }
        }
    }
}
