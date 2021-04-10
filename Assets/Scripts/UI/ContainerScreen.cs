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

            // Fill player's inventory UI
            var firstItem = FillGridWithInventory(this, PlayerItemsGrid, PlayerInventory, ItemPrefab);
            if (firstItem) {
                firstItem.OnClick();
                // TODO Highlight clicked item
                // TODO Highlight equipped items
            }

            // Fill container's inventory UI
            FillGridWithInventory(this, ContainerItemsGrid, otherContainer, ItemPrefab);

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

    }

}