using System.Collections.ObjectModel;
using Interactables;
using UnityEngine;

namespace Logic {

    public class PlayerController : MonoBehaviour {

        private Collection<Interactable> _interactablesInRange;
        private Interactable _nextToInteractWith;

        private bool _inGame;
        
        private void Awake() {
            _nextToInteractWith = null;
            _interactablesInRange = new Collection<Interactable>();
        }

        private void Update() {
            
            // Interaction
            if (Input.GetKeyDown(KeyCode.E) && _nextToInteractWith) {
                Interact();
            }
            
            // Inventory
            if (Input.GetKeyDown(KeyCode.I)) {
                EventManager.TriggerEvent(Constants.InventoryKeyPressedEvent, "");
            }
            
            // Pause Menu
            if (Input.GetKeyDown(KeyCode.Escape)) {
                EventManager.TriggerEvent(Constants.PauseKeyPressedEvent, "");
            }
            
        }

        private void Interact() {
            _nextToInteractWith.Use();

            // If not destroyed after interaction
            if (_nextToInteractWith) {
                EventManager.TriggerEvent(Constants.ShowTipEvent,
                    InteractableToUseString(_nextToInteractWith));
            }
            else {
                _interactablesInRange.Remove(_nextToInteractWith);
                EventManager.TriggerEvent(Constants.HideTipEvent, "");
            }
        }

        private void OnTriggerEnter(Collider other) {
            var inter = other.GetComponent<Interactable>();
            if (!inter) return;

            _interactablesInRange.Add(inter);
            inter.OnEnableHighlight();

            // Update current
            if (_nextToInteractWith == null) {
                _nextToInteractWith = inter;
            }

            // Update UI
            EventManager.TriggerEvent(Constants.ShowTipEvent,
                InteractableToUseString(_nextToInteractWith));
        }

        private void OnTriggerExit(Collider other) {
            var inter = other.GetComponent<Interactable>();
            if (!inter) return;

            _interactablesInRange.Remove(inter);
            inter.OnDisableHighlight();

            // Update current
            if (_nextToInteractWith == inter) {
                _nextToInteractWith = _interactablesInRange.Count > 0 ? _interactablesInRange[0] : null;
            }

            // Update UI
            EventManager.TriggerEvent(_nextToInteractWith ? Constants.ShowTipEvent : Constants.HideTipEvent,
                InteractableToUseString(_nextToInteractWith));
        }

        private static string InteractableToUseString(Interactable inter) {
            switch (inter) {
                case Container container:
                    return container.IsOpened ? Constants.CloseContainerText : Constants.OpenContainerText;
                case Pickable _:
                    return Constants.CollectItemText;
                case Usable _:
                    return Constants.InteractWithText;
                default:
                    return "";
            }
        }

    }

}