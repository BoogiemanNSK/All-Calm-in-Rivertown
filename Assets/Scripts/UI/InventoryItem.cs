using Items;
using UnityEngine;
using UnityEngine.UI;

namespace UI {

    public class InventoryItem : MonoBehaviour {

        public Item InternalItem { get; private set; }

        [SerializeField] private Image ItemIcon;
        private DescriptionScreen _parent;

        public void SetItem(Item item) {
            ItemIcon.color = new Color(255.0f, 255.0f, 255.0f, 1.0f);
            ItemIcon.sprite = item.Image;
            InternalItem = item;
        }

        public void SetEmpty() {
            ItemIcon.color = new Color(255.0f, 255.0f, 255.0f, 0.0f);
            InternalItem = null;
        }

        public void SetParent(DescriptionScreen parentScreen) {
            _parent = parentScreen;
        }

        public void OnClick() {
            if (!InternalItem) {
                Debug.Log("Trying to click on empty cell");
                return;
            }
            _parent.SelectItem(InternalItem, this);
        }

    }

}