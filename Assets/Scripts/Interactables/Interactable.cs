using cakeslice;
using UnityEngine;

namespace Interactables {

    public abstract class Interactable : MonoBehaviour {

        [Header("Outline Shaders")]
        [SerializeField] private Outline[] _outlines;

        protected virtual void Awake() {
            OnDisableHighlight();
        }

        public abstract void Use();

        public void OnEnableHighlight() {
            foreach (var outline in _outlines) {
                outline.eraseRenderer = false;
            }
        }
        
        public void OnDisableHighlight() {
            foreach (var outline in _outlines) {
                outline.eraseRenderer = true;
            }
        }

    }

}