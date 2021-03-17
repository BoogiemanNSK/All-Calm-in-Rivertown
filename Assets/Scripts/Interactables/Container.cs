using UnityEngine;

namespace Interactables {

    public class Container : Interactable {

        [SerializeField] private Animation Animations;
        [SerializeField] private AnimationClip OpeningAnimation;
        [SerializeField] private AnimationClip ClosingAnimation;
        
        public override void Use() {
            throw new System.NotImplementedException();
        }

    }

}