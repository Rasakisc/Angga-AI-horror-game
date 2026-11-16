using Game.Gameplay;
using UnityEngine;

namespace Game.UI
{
    public class StaminaBarUI : MonoBehaviour
    {
        [SerializeField] private PlayerStamina _stamina;
        [SerializeField] private RectTransform _fill;

        private void OnEnable()
        {
            if (_stamina != null)
            {
                _stamina.OnStaminaChanged += UpdateBar;
            }
        }

        private void OnDisable()
        {
            if (_stamina != null)
            {
                _stamina.OnStaminaChanged -= UpdateBar;
            }
        }

        private void UpdateBar(float normalized)
        {
            if (_fill == null) return;

            _fill.anchorMax = new Vector2(Mathf.Clamp01(normalized), 1f);
        }
    }
}
