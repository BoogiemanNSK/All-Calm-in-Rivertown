using Items;
using Logic;
using UnityEngine;

namespace UI {
    public class InventoryScreen : DescriptionScreen {

        [Header("Inventory UI")] 
        [SerializeField] private Transform ItemsGrid;

        private bool _isOpened;

        private void Awake() {
            _isOpened = false;
        }

        protected override void Start() {
            base.Start();
            EventManager.StartListening(Constants.InventoryKeyPressedEvent, InventoryButtonPressed);
        }

        public override void SelectItem(Item itemObject, InventoryItem uiObject) {
            base.SelectItem(itemObject, uiObject);

            if (itemObject is Other || itemObject.Equipped) {
                UseButton.gameObject.SetActive(false);
            }
            else {
                UseButton.gameObject.SetActive(true);
                ButtonText.text = Constants.ItemTypeToUseBtnText[itemObject.GetType()];
            }
        }

        public void UseItem() {
            // TODO Delete item from inventory and UI if consumable
            SelectedItem.InternalItem.Use();
        }

        private void OpenInventory() {
            if (GameLogic.Instance.GameIsPaused) return;
            GameLogic.Instance.PauseGame();

            Screen.SetActive(true);
            _isOpened = true;

            var firstItem = FillGridWithInventory(this, ItemsGrid, PlayerInventory, ItemPrefab);
            if (firstItem) {
                firstItem.OnClick();
                // TODO Highlight clicked item
                // TODO Highlight equipped items
            }
        }

        private void CloseInventory() {
            foreach (Transform item in ItemsGrid) {
                Destroy(item.gameObject);
            }

            _isOpened = false;
            Screen.SetActive(false);
            GameLogic.Instance.ResumeGame();
        }

        private void InventoryButtonPressed(string _) {
            if (_isOpened) CloseInventory();
            else OpenInventory();
        }

    }
}