using Items;
using GameSystems;
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

        protected Inventory _playerInventory;
        protected InventoryItem _selectedItem
        
        public void SelectItem(Item itemObject, InventoryItem uiObject) {
            _selectedItem = itemObject;
            ChosenItemName.text = itemObject.Name;
            ChosenItemDesc.text = itemObject.Description;
            ChosenItemPrice.text = itemObject.Price;
        }

    }

}