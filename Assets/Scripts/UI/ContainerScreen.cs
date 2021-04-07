using System.Collections;
using GameSystems;
using Items;
using Logic;
using UnityEngine;
using UnityEngine.UI;

namespace UI {

    public class ContainerScreen : DescriptionScreen {

        [Header("Containers UI")] 
        [SerializeField] private Text PlayerInventoryName;
        [SerializeField] private Text ContainerInventoryName;
        [SerializeField] private Transform PlayerItemsGrid;
        [SerializeField] private Transform ContainerItemsGrid;
        
        private Inventory _openedContainerInventory;

        public override void SelectItem(Item itemObject, InventoryItem uiObject) {
            base.SelectItem(itemObject, uiObject);
            
            UseButton.gameObject.SetActive(true);
            if (itemObject.Equipped) {
                UseButton.gameObject.SetActive(false);
            }
            else if (uiObject.transform.parent == PlayerItemsGrid) {
                ButtonText.text = Constants.ContainerStash;
            }
            else {
                ButtonText.text = Constants.ContainerTake;
            }
        }
        
        public void TransferItem() {
            var oldGrid = PlayerItemsGrid;
            var newGrid = ContainerItemsGrid;
            var transferredItem = SelectedItem.InternalItem;
            
            if (SelectedItem.transform.parent == PlayerItemsGrid) { 
                PlayerInventory.TransferItem(transferredItem, _openedContainerInventory);
            }
            else {
                _openedContainerInventory.TransferItem(transferredItem, PlayerInventory);
                oldGrid = ContainerItemsGrid;
                newGrid = PlayerItemsGrid;
            }
            
            // Destroy UI cell with old item place and insert empty cell 
            Destroy(SelectedItem.gameObject);
            var newCell = Instantiate(ItemPrefab, oldGrid);
            newCell.SetEmpty();
            newCell.SetParent(this);
            
            SelectedItem = InsertToFirstFreeCell(newGrid, transferredItem);
            SelectedItem.OnClick();
        }
        
        // Open container screen for some specific container inventory
        public void OpenContainerScreen(Inventory otherContainer) {
            if (GameLogic.Instance.GameIsPaused) return;
            GameLogic.Instance.PauseGame();
            
            _openedContainerInventory = otherContainer;
            Screen.SetActive(true);
            
            var itemsCount = PlayerInventory.ItemsList.Count;
            
            // First item select
            if (itemsCount > 0) {
                var createdItem = Instantiate(ItemPrefab, PlayerItemsGrid);
                createdItem.SetItem(PlayerInventory.ItemsList[0]);
                createdItem.SetParent(this);
                createdItem.OnClick();
                // TODO Highlight clicked item
                // TODO Highlight equipped items
            }
            
            for (var i = 1; i < itemsCount; i++) {
                var createdItem = Instantiate(ItemPrefab, PlayerItemsGrid);
                createdItem.SetItem(PlayerInventory.ItemsList[i]);
                createdItem.SetParent(this);
            }

            for (var i = itemsCount; i < Constants.InventoryMaxSize; i++) {
                var createdItem = Instantiate(ItemPrefab, PlayerItemsGrid);
                createdItem.SetEmpty();
                createdItem.SetParent(this);
            }

            _openedContainerInventory = otherContainer;
            itemsCount = otherContainer.ItemsList.Count;
            
            for (var i = 0; i < itemsCount; i++) {
                var createdItem = Instantiate(ItemPrefab, ContainerItemsGrid);
                createdItem.SetItem(otherContainer.ItemsList[i]);
                createdItem.SetParent(this);
            }

            for (var i = itemsCount; i < Constants.InventoryMaxSize; i++) {
                var createdItem = Instantiate(ItemPrefab, ContainerItemsGrid);
                createdItem.SetEmpty();
                createdItem.SetParent(this);
            }

            PlayerInventoryName.text = PlayerInventory.InventoryName;
            ContainerInventoryName.text = otherContainer.InventoryName;
        }

        public void CloseContainerScreen() {
            foreach (Transform item in PlayerItemsGrid) {
                Destroy(item.gameObject);
            }
            
            foreach (Transform item in ContainerItemsGrid) {
                Destroy(item.gameObject);
            }
            
            Screen.SetActive(false);
            GameLogic.Instance.ResumeGame();
        }

        private static InventoryItem InsertToFirstFreeCell(IEnumerable grid, Item item) {
            foreach (Transform cell in grid) {
                var itemCell = cell.gameObject.GetComponent<InventoryItem>();
                if (itemCell.InternalItem != null) continue;
                itemCell.SetItem(item);
                return itemCell;
            }
            return null;
        }

    }

}