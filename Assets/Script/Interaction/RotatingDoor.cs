using UnityEngine;

namespace Game.Interaction
{
    public class RotatingDoor : Door
    {
        [SerializeField] private Transform _leaf;
        [SerializeField] private float _openAngle = 90f;

        private Quaternion _closedRotation;
        private float _direction = 1f;

        protected override void CacheClosedPose()
        {
            if (_leaf == null) _leaf = transform;
            _closedRotation = _leaf.localRotation;
        }

        protected override void OnBeforeOpen(GameObject interactor)
        {
            Vector3 toInteractor = interactor.transform.position - _leaf.position;
            _direction = Vector3.Dot(toInteractor, transform.forward) > 0f ? -1f : 1f;
        }

        protected override void ApplyPose(float t)
        {
            _leaf.localRotation = _closedRotation * Quaternion.Euler(0f, _openAngle * _direction * t, 0f);
        }
    }
}
