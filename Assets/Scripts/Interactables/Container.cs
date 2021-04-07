using GameSystems;
using Items;
using UI;
using UnityEngine;

namespace Interactables {

    public class Container : Interactable {

        [SerializeField] private string ContainerName;
        [SerializeField] private Item[] InitialItemsList;
        
        [Header("Open/Close Animations")]
        [SerializeField] private Animation Animations;
        [SerializeField] private AnimationClip OpeningAnimation;
        [SerializeField] private AnimationClip ClosingAnimation;

        private Inventory _inventory;
        private ContainerScreen _containerUI;
        
        public bool IsOpened { get; private set; }

        protected override void Awake() {
            base.Awake();
            IsOpened = false;
            _inventory = new Inventory(ContainerName, InitialItemsList);
            _containerUI = FindObjectOfType<ContainerScreen>();
        }
        
        public override void Use() {
            Animations.clip = IsOpened ? ClosingAnimation : OpeningAnimation;
            Animations.Play();
            
            // Open or close container UI
            if (IsOpened) _containerUI.CloseContainerScreen();
            else _containerUI.OpenContainerScreen(_inventory);
            
            IsOpened = !IsOpened;
        }

    }

}