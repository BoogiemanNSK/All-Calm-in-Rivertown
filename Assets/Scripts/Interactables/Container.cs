using GameSystems;
using UI;
using UnityEngine;

namespace Interactables {

    public class Container : Interactable {

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
        }
        
        public override void Use() {
            Animations.clip = IsOpened ? ClosingAnimation : OpeningAnimation;
            Animations.Play();

            IsOpened = !IsOpened;

            // TODO Open or close container UI
        }

    }

}