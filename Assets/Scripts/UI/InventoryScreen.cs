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

            var itemsCount = PlayerInventory.ItemsList.Count;
            
            // First item select
            if (itemsCount > 0) {
                var createdItem = Instantiate(ItemPrefab, ItemsGrid);
                createdItem.SetItem(PlayerInventory.ItemsList[0]);
                createdItem.SetParent(this);
                createdItem.OnClick();
                // TODO Highlight clicked item
                // TODO Highlight equipped items
            }
            
            for (var i = 1; i < itemsCount; i++) {
                var createdItem = Instantiate(ItemPrefab, ItemsGrid);
                createdItem.SetItem(PlayerInventory.ItemsList[i]);
                createdItem.SetParent(this);
            }

            for (var i = itemsCount; i < Constants.InventoryMaxSize; i++) {
                var createdItem = Instantiate(ItemPrefab, ItemsGrid);
                createdItem.SetEmpty();
                createdItem.SetParent(this);
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