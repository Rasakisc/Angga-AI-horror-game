using Game.Interaction;
using TMPro;
using UnityEngine;

namespace Game.UI
{
    public class InteractPromptUI : MonoBehaviour
    {
        [SerializeField] private InteractDetector _detector;
        [SerializeField] private GameObject _root;
        [SerializeField] private TMP_Text _label;

        private void OnEnable()
        {
            if (_detector != null)
            {
                _detector.OnTargetChanged += HandleTargetChanged;
            }
        }

        private void OnDisable()
        {
            if (_detector != null)
            {
                _detector.OnTargetChanged -= HandleTargetChanged;
            }
        }

        private void Start()
        {
            HandleTargetChanged(_detector != null ? _detector.Current : null);
        }

        private void HandleTargetChanged(IInteractable target)
        {
            if (_root != null)
            {
                _root.SetActive(target != null);
            }

            if (target != null && _label != null)
            {
                _label.text = target.InteractPrompt;
            }
        }
    }
}
