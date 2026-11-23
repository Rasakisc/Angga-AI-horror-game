using UnityEngine;

namespace Game.Interaction
{
    public class SlidingDoor : Door
    {
        [SerializeField] private Transform _leaf;
        [SerializeField] private Vector3 _slideOffset = new Vector3(-1f, 0f, 0f);

        private Vector3 _closedPosition;

        protected override void CacheClosedPose()
        {
            if (_leaf == null) _leaf = transform;
            _closedPosition = _leaf.localPosition;
        }

        protected override void ApplyPose(float t)
        {
            _leaf.localPosition = _closedPosition + _slideOffset * t;
        }
    }
}
