using Game.Gameplay;
using UnityEngine;

namespace Game.Interaction
{
    public class Closet : MonoBehaviour, IInteractable
    {
        [SerializeField] private Transform _hidePoint;
        [SerializeField] private Transform _exitPoint;
        [SerializeField] private AudioSource _audioSource;
        [SerializeField] private AudioClip _clip;

        private GameObject _hiddenPlayer;

        public bool IsOccupied => _hiddenPlayer != null;

        public string InteractPrompt => IsOccupied ? "Keluar" : "Sembunyi";

        public void Interact(GameObject interactor)
        {
            if (IsOccupied)
            {
                var detector = _hiddenPlayer.GetComponent<InteractDetector>();
                if (detector != null) detector.ReleaseTarget();
                Move(_hiddenPlayer, _exitPoint, true);
                _hiddenPlayer = null;
            }
            else
            {
                _hiddenPlayer = interactor;
                Move(interactor, _hidePoint, false);
                var detector = interactor.GetComponent<InteractDetector>();
                if (detector != null) detector.LockTarget(this);
            }

            if (_audioSource != null && _clip != null)
            {
                _audioSource.PlayOneShot(_clip);
            }
        }

        private static void Move(GameObject player, Transform target, bool canMove)
        {
            if (player == null || target == null) return;

            var controller = player.GetComponent<CharacterController>();
            var movement = player.GetComponent<PlayerMovement>();

            if (controller != null) controller.enabled = false;
            if (movement != null) movement.enabled = false;

            player.transform.SetPositionAndRotation(target.position, Quaternion.Euler(0f, target.eulerAngles.y, 0f));

            if (canMove)
            {
                if (controller != null) controller.enabled = true;
                if (movement != null) movement.enabled = true;
            }
        }
    }
}
