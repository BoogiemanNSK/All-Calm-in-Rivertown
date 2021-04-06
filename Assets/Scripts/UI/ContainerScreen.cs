using GameSystems;
using UnityEngine;

namespace UI {

    public class ContainerScreen : DescriptionScreen {

        [Header("Containers UI")]
        [SerializeField] private Transform PlayerItemsGrid;
        [SerializeField] private Transform ContainerItemsGrid;
        
        private Inventory _currentContainerInventory;

        // Open container screen for some specific container inventory
        public void OpenContainerScreen(Inventory otherContainer) {
            Screen.SetActive(true);
        }

        public void CloseContainerScreen() {
            Screen.SetActive(false);
        }

    }

}