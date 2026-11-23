using System;
using UnityEngine;

namespace Game.Interaction
{
    public class PickupItem : MonoBehaviour, IInteractable
    {
        [SerializeField] private string _itemId = "kunci_pintu";
        [SerializeField] private string _displayName = "Kunci";

        public string InteractPrompt => "Ambil " + _displayName;

        public static event Action<string> PickedUp;

        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.SubsystemRegistration)]
        private static void ResetStatics()
        {
            PickedUp = null;
        }

        public void Interact(GameObject interactor)
        {
            var inventory = interactor.GetComponent<Inventory>();
            if (inventory == null) return;

            inventory.Add(_itemId, _displayName);
            PickedUp?.Invoke(_displayName);
            Destroy(gameObject);
        }
    }
}
