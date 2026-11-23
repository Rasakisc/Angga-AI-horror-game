using System.Text;
using Game.Interaction;
using TMPro;
using UnityEngine;

namespace Game.UI
{
    public class InventoryUI : MonoBehaviour
    {
        [SerializeField] private Inventory _inventory;
        [SerializeField] private TMP_Text _label;

        private readonly StringBuilder _builder = new StringBuilder();

        private void OnEnable()
        {
            if (_inventory != null)
            {
                _inventory.OnChanged += Refresh;
            }
        }

        private void OnDisable()
        {
            if (_inventory != null)
            {
                _inventory.OnChanged -= Refresh;
            }
        }

        private void Start()
        {
            Refresh(_inventory);
        }

        private void Refresh(Inventory inventory)
        {
            if (_label == null) return;

            _builder.Clear();
            _builder.Append("Inventory");
            if (inventory == null || inventory.Items.Count == 0)
            {
                _builder.Append("\n- (kosong)");
            }
            else
            {
                foreach (var item in inventory.Items)
                {
                    _builder.Append("\n- ").Append(item.DisplayName);
                }
            }

            _label.text = _builder.ToString();
        }
    }
}
