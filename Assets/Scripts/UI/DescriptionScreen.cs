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

        [Header("Description Screen Button")] 
        [SerializeField] protected Button UseButton;
        [SerializeField] protected Text ButtonText;
        
        protected Inventory PlayerInventory;
        protected InventoryItem SelectedItem;

        protected virtual void Start() {
            PlayerInventory = FindObjectOfType<Player>().Inventory;
        }

        public virtual void SelectItem(Item itemObject, InventoryItem uiObject) {
            SelectedItem = uiObject;
            ChosenItemName.text = itemObject.Name;
            ChosenItemDesc.text = itemObject.Description;
            ChosenItemPrice.text = itemObject.Price.ToString();
        }

    }

}