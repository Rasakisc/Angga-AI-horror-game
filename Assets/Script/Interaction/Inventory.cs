using System;
using System.Collections.Generic;
using UnityEngine;

namespace Game.Interaction
{
    public class Inventory : MonoBehaviour
    {
        [Serializable]
        public struct Entry
        {
            public string Id;
            public string DisplayName;
        }

        [SerializeField] private List<Entry> _items = new List<Entry>();

        public IReadOnlyList<Entry> Items => _items;

        public event Action<Inventory> OnChanged;

        public void Add(string id, string displayName)
        {
            _items.Add(new Entry { Id = id, DisplayName = displayName });
            OnChanged?.Invoke(this);
        }

        public bool Has(string id)
        {
            return _items.FindIndex(e => e.Id == id) >= 0;
        }

        public bool Remove(string id)
        {
            int index = _items.FindIndex(e => e.Id == id);
            if (index < 0) return false;

            _items.RemoveAt(index);
            OnChanged?.Invoke(this);
            return true;
        }
    }
}
