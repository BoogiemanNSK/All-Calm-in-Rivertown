using Items;
using UI;
using UnityEngine;
using UnityEngine.UI;

namespace GameSystems {

    public class InventoryItem : MonoBehaviour {
        
        [SerializeField] private Image ItemIcon;
        private InventoryScreen _parent;
        private Item _currentItem;

        public void SetItem(Item item) {
            ItemIcon.color = new Color(255.0f, 255.0f, 255.0f, 1.0f);
            ItemIcon.sprite = item.Image;
            _currentItem = item;
        }

        public void SetEmpty() {
            ItemIcon.color = new Color(255.0f, 255.0f, 255.0f, 0.0f);
            _currentItem = null;
        }

        public void SetParent(InventoryScreen parentScreen) {
            _parent = parentScreen;
        }

        public void OnClick() {
            if (!_currentItem) return;
            _parent.SetDescription(
                _currentItem.Name,
            _currentItem.Description,
                _currentItem.Price.ToString());
        }

    }

}