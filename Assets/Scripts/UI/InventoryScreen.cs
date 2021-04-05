using GameSystems;
using UnityEngine;
using UnityEngine.UI;

namespace UI {

    public class InventoryScreen : MonoBehaviour {

        [SerializeField] private GameObject Screen;
        [SerializeField] private Transform ItemsGrid;
        [SerializeField] private InventoryItem ItemPrefab;
        
        [Header("Item Description")]
        [SerializeField] private Text ChosenItemName;
        [SerializeField] private Text ChosenItemDesc;
        [SerializeField] private Text ChosenItemPrice;

        private Inventory _playerInventory;
        private bool _isOpened;
        
        public void SetDescription(string itemName, string desc, string price) {
            ChosenItemName.text = itemName;
            ChosenItemDesc.text = desc;
            ChosenItemPrice.text = price;
        }

        private void Awake() {
            _isOpened = false;
        }

        private void Start() {
            EventManager.StartListening(Constants.InventoryKeyPressedEvent, InventoryButtonPressed);
            _playerInventory = FindObjectOfType<Player>().Inventory;
        }

        private void OpenInventory() {
            Screen.SetActive(true);
            _isOpened = true;

            var itemsCount = _playerInventory.ItemsList.Count;
            
            // First item select
            if (itemsCount > 0) {
                var createdItem = Instantiate(ItemPrefab, ItemsGrid);
                createdItem.SetItem(_playerInventory.ItemsList[0]);
                createdItem.SetParent(this);
                createdItem.OnClick();
                // TODO Highlight clicked item
            }
            
            for (var i = 1; i < itemsCount; i++) {
                var createdItem = Instantiate(ItemPrefab, ItemsGrid);
                createdItem.SetItem(_playerInventory.ItemsList[i]);
                createdItem.SetParent(this);
            }

            for (var i = itemsCount; i < Constants.InventoryMaxSize; i++) {
                var createdItem = Instantiate(ItemPrefab, ItemsGrid);
                createdItem.SetEmpty();
                createdItem.SetParent(this);
            }
        }

        private void CloseInventory() {
            foreach (Transform item in ItemsGrid.transform) {
                Destroy(item);
            }

            _isOpened = false;
            Screen.SetActive(false);
        }

        private void InventoryButtonPressed(string _) {
            if (_isOpened) CloseInventory();
            else OpenInventory();
        }

    }

}