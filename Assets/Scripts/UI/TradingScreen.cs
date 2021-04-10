using System.Collections.Generic;
using GameSystems;
using Items;
using Logic;
using UnityEngine;
using UnityEngine.UI;

namespace UI {

    public class TradingScreen : DescriptionScreen {

        [Header("Containers UI")] 
        [SerializeField] private Text PlayerInventoryName;
        [SerializeField] private Text TraderInventoryName;
        [SerializeField] private Transform PlayerItemsGrid;
        [SerializeField] private Transform TraderItemsGrid;

        [Header("Trading Info")] 
        [SerializeField] private Transform SellItemsGrid;
        [SerializeField] private Transform BuyItemsGrid;
        [SerializeField] private Text TradingSum;
        [SerializeField] private Button TradeButton;

        private Inventory _currentTraderInventory;
        private Inventory _sellInventory;
        private Inventory _buyInventory;
        private int _tradeSum;
            
        public override void SelectItem(Item itemObject, InventoryItem uiObject) {
            base.SelectItem(itemObject, uiObject);
            
            UseButton.gameObject.SetActive(true);
            if (itemObject.Equipped) {
                UseButton.gameObject.SetActive(false);
            }
            else if (uiObject.transform.parent == PlayerItemsGrid) {
                ButtonText.text = Constants.TradingSell;
            }
            else if (uiObject.transform.parent == TraderItemsGrid) {
                ButtonText.text = Constants.TradingBuy;
            }
            else if (uiObject.transform.parent == BuyItemsGrid) {
                ButtonText.text = Constants.TradingCancelBuy;
            }
            else {
                ButtonText.text = Constants.TradingCancelSell;
            }
        }
        
        public void OpenTradeMenu(Inventory traderInventory) {
            if (GameLogic.Instance.GameIsPaused) return;
            GameLogic.Instance.PauseGame();

            _tradeSum = 0;
            _currentTraderInventory = traderInventory;
            _sellInventory = new Inventory("Sell", new List<Item>());
            _buyInventory = new Inventory("Buy", new List<Item>());
            Screen.SetActive(true);

            TradingSum.text = _tradeSum.ToString();
            TradeButton.interactable = false;

            PlayerInventoryName.text = PlayerInventory.InventoryName;
            TraderInventoryName.text = traderInventory.InventoryName;
            
            // Fill player's inventory UI
            var firstItem = FillGridWithInventory(this, PlayerItemsGrid, PlayerInventory, ItemPrefab);
            if (firstItem) {
                firstItem.OnClick();
                // TODO Highlight clicked item
                // TODO Highlight equipped items
            }

            // Fill remaining grids UI
            FillGridWithInventory(this, TraderItemsGrid, traderInventory, ItemPrefab);
            FillGridWithInventory(this, SellItemsGrid, _sellInventory, ItemPrefab);
            FillGridWithInventory(this, BuyItemsGrid, _buyInventory, ItemPrefab);
        }

        public void DescButtonClicked() {
            var transferredItem = SelectedItem.InternalItem;
            Transform oldGrid, newGrid;
            Inventory oldInventory, newInventory;
            
            if (SelectedItem.transform.parent == PlayerItemsGrid) {
                oldGrid = PlayerItemsGrid;
                newGrid = SellItemsGrid;
                oldInventory = PlayerInventory;
                newInventory = _sellInventory;
                _tradeSum += SelectedItem.InternalItem.Price;
            }
            else if (SelectedItem.transform.parent == TraderItemsGrid) {
                oldGrid = TraderItemsGrid;
                newGrid = BuyItemsGrid;
                oldInventory = _currentTraderInventory;
                newInventory = _buyInventory;
                _tradeSum -= SelectedItem.InternalItem.Price;
            }
            else if (SelectedItem.transform.parent == BuyItemsGrid) {
                oldGrid = BuyItemsGrid;
                newGrid = TraderItemsGrid;
                oldInventory = _buyInventory;
                newInventory = _currentTraderInventory;
                _tradeSum += SelectedItem.InternalItem.Price;
            }
            else {
                oldGrid = SellItemsGrid;
                newGrid = PlayerItemsGrid;
                oldInventory = _sellInventory;
                newInventory = PlayerInventory;
                _tradeSum -= SelectedItem.InternalItem.Price;
            }

            // Update sum UI
            TradingSum.text = _tradeSum.ToString();
            
            // Logically transfer item to new inventory
            oldInventory.TransferItem(transferredItem, newInventory);
            
            // Destroy UI cell with old item place and insert empty cell 
            Destroy(SelectedItem.gameObject);
            var newCell = Instantiate(ItemPrefab, oldGrid);
            newCell.SetEmpty();
            newCell.SetParent(this);
            
            SelectedItem = InsertToFirstFreeCell(newGrid, transferredItem);
            SelectedItem.OnClick();

            // Check if trade can be performed
            TradeButton.interactable = _tradeSum >= 0 &&
                (_buyInventory.ItemsList.Count > 0 || _sellInventory.ItemsList.Count > 0);
        }

        public void PerformTrade() {
            foreach (Transform item in BuyItemsGrid) {
                var uiItem = item.gameObject.GetComponent<InventoryItem>();
                if (!uiItem.InternalItem) continue;
                TransferItem(uiItem, BuyItemsGrid, PlayerItemsGrid, _buyInventory, PlayerInventory);
            }
            foreach (Transform item in SellItemsGrid) {
                var uiItem = item.gameObject.GetComponent<InventoryItem>();
                if (!uiItem.InternalItem) continue;
                TransferItem(uiItem, SellItemsGrid, TraderItemsGrid, _sellInventory, _currentTraderInventory);
            }

            _tradeSum = 0;
            TradingSum.text = _tradeSum.ToString();
            TradeButton.interactable = false;
        }

        public void CloseTradingMenu() {
            // Cancel any transaction if any was planned
            foreach (Transform item in BuyItemsGrid) {
                var uiItem = item.gameObject.GetComponent<InventoryItem>();
                if (!uiItem.InternalItem) continue;
                TransferItem(uiItem, BuyItemsGrid, TraderItemsGrid, _buyInventory, _currentTraderInventory);
            }
            foreach (Transform item in SellItemsGrid) {
                var uiItem = item.gameObject.GetComponent<InventoryItem>();
                if (!uiItem.InternalItem) continue;
                TransferItem(uiItem, SellItemsGrid, PlayerItemsGrid, _sellInventory, PlayerInventory);
            }
            
            // Clear all grids
            foreach (Transform item in PlayerItemsGrid) { Destroy(item.gameObject); }
            foreach (Transform item in TraderItemsGrid) { Destroy(item.gameObject); }
            foreach (Transform item in SellItemsGrid) { Destroy(item.gameObject); }
            foreach (Transform item in BuyItemsGrid) { Destroy(item.gameObject); }
            
            Screen.SetActive(false);
            GameLogic.Instance.ResumeGame();
        }
        
        private void TransferItem(InventoryItem uiItem, Transform uiFrom, Transform uiTo, Inventory inventoryFrom, Inventory inventoryTo) {
            var transferredItem = uiItem.InternalItem;
            
            inventoryFrom.TransferItem(transferredItem, inventoryTo);
            
            // Destroy UI cell with old item place and insert empty cell 
            Destroy(uiItem.gameObject);
            var newCell = Instantiate(ItemPrefab, uiFrom);
            newCell.SetEmpty();
            newCell.SetParent(this);
            
            InsertToFirstFreeCell(uiTo, transferredItem);
        }

    }

}