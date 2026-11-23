using System;
using Game.Gameplay;
using UnityEngine;

namespace Game.Interaction
{
    public class InteractDetector : MonoBehaviour
    {
        [SerializeField] private PlayerInputHandler _input;
        [SerializeField] private Transform _cameraTransform;
        [SerializeField] private float _range = 2.5f;
        [SerializeField] private LayerMask _layerMask = ~0;

        private IInteractable _lockedTarget;

        public IInteractable Current { get; private set; }

        public void LockTarget(IInteractable target)
        {
            _lockedTarget = target;
        }

        public void ReleaseTarget()
        {
            _lockedTarget = null;
        }

        public event Action<IInteractable> OnTargetChanged;

        private void OnEnable()
        {
            if (_input != null)
            {
                _input.OnInteract += HandleInteract;
            }
        }

        private void OnDisable()
        {
            if (_input != null)
            {
                _input.OnInteract -= HandleInteract;
            }
        }

        private void Update()
        {
            if (_cameraTransform == null) return;

            IInteractable found = _lockedTarget;
            var ray = new Ray(_cameraTransform.position, _cameraTransform.forward);
            if (found == null && Physics.Raycast(ray, out RaycastHit hit, _range, _layerMask, QueryTriggerInteraction.Collide))
            {
                found = hit.collider.GetComponentInParent<IInteractable>();
            }

            if (found != Current)
            {
                Current = found;
                OnTargetChanged?.Invoke(Current);
            }
        }

        private void HandleInteract()
        {
            if (Current == null) return;

            Current.Interact(gameObject);
            OnTargetChanged?.Invoke(Current);
        }
    }
}
