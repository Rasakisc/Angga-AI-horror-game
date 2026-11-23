using System.Collections;
using Game.Interaction;
using TMPro;
using UnityEngine;

namespace Game.UI
{
    public class MessageUI : MonoBehaviour
    {
        [SerializeField] private CanvasGroup _group;
        [SerializeField] private TMP_Text _label;
        [SerializeField] private float _showDuration = 1.8f;
        [SerializeField] private float _fadeDuration = 0.4f;

        private Coroutine _routine;

        private void OnEnable()
        {
            Door.LockedInteracted += HandleLockedDoor;
            Door.UnlockedWithKey += HandleUnlocked;
            PickupItem.PickedUp += HandlePickedUp;
        }

        private void OnDisable()
        {
            Door.LockedInteracted -= HandleLockedDoor;
            Door.UnlockedWithKey -= HandleUnlocked;
            PickupItem.PickedUp -= HandlePickedUp;
        }

        private void HandleUnlocked(Door door)
        {
            Show("Pintu terbuka dengan kunci");
        }

        private void HandlePickedUp(string displayName)
        {
            Show(displayName + " masuk ke inventory");
        }

        private void Start()
        {
            if (_group != null)
            {
                _group.alpha = 0f;
            }
        }

        public void Show(string message)
        {
            if (_group == null || _label == null) return;

            _label.text = message;
            if (_routine != null)
            {
                StopCoroutine(_routine);
            }
            _routine = StartCoroutine(ShowRoutine());
        }

        private void HandleLockedDoor(Door door)
        {
            Show(door.LockedMessage);
        }

        private IEnumerator ShowRoutine()
        {
            _group.alpha = 1f;
            yield return new WaitForSeconds(_showDuration);

            float time = 0f;
            while (time < _fadeDuration)
            {
                time += Time.deltaTime;
                _group.alpha = 1f - time / _fadeDuration;
                yield return null;
            }

            _group.alpha = 0f;
            _routine = null;
        }
    }
}
