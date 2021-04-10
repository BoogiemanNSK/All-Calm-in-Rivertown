using System.Collections;
using Items;
using GameSystems;
using Logic;
using UnityEngine;
using UnityEngine.UI;

namespace UI {
    public class DescriptionScreen : MonoBehaviour {

        [Header("Screen UI Specs")] 
        [SerializeField] protected GameObject Screen;
        [SerializeField] protected InventoryItem ItemPrefab;

        [Header("Item Description")] 
        [SerializeField] protected Text ChosenItemName;
        [SerializeField] protected Text ChosenItemDesc;
        [SerializeField] protected Text ChosenItemPrice;

        [Header("Description Screen Button")] 
        [SerializeField] protected Button UseButton;
        [SerializeField] protected Text ButtonText;

        protected Inventory PlayerInventory;
        protected InventoryItem SelectedItem;

        protected virtual void Start() {
            var player = FindObjectOfType<PlayerController>();
            PlayerInventory = player.gameObject.GetComponent<Character>().Inventory;
        }

        public virtual void SelectItem(Item itemObject, InventoryItem uiObject) {
            SelectedItem = uiObject;
            ChosenItemName.text = itemObject.Name;
            ChosenItemDesc.text = itemObject.Description;
            ChosenItemPrice.text = itemObject.Price.ToString();
        }

        protected static InventoryItem InsertToFirstFreeCell(IEnumerable grid, Item item) {
            foreach (Transform cell in grid) {
                var itemCell = cell.gameObject.GetComponent<InventoryItem>();
                if (itemCell.InternalItem != null) continue;
                itemCell.SetItem(item);
                return itemCell;
            }

            return null;
        }

        protected static InventoryItem FillGridWithInventory(DescriptionScreen parent, Transform grid,
            Inventory inventory, InventoryItem uiPrefab) {
            InventoryItem firstItem = null;
            var itemsCount = inventory.ItemsList.Count;
            
            // First item select
            if (itemsCount > 0) {
                firstItem = Instantiate(uiPrefab, grid);
                firstItem.SetItem(inventory.ItemsList[0]);
                firstItem.SetParent(parent);
            }
            
            for (var i = 1; i < itemsCount; i++) {
                var createdItem = Instantiate(uiPrefab, grid);
                createdItem.SetItem(inventory.ItemsList[i]);
                createdItem.SetParent(parent);
            }

            for (var i = itemsCount; i < Constants.InventoryMaxSize; i++) {
                var createdItem = Instantiate(uiPrefab, grid);
                createdItem.SetEmpty();
                createdItem.SetParent(parent);
            }

            return firstItem;
        }

    }
}