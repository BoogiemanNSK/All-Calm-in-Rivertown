using GameSystems;
using GameSystems.DialogSystem;
using UI;
using UnityEngine;

namespace Interactables {

    public class Talkable : Interactable {

        [SerializeField] private Character SpeakerData;
        [SerializeField] private DialogNode[] PossibleStartingDialogNodes;
        private DialogScreen _dialogUI;

        protected override void Awake() {
            base.Awake();
            _dialogUI = FindObjectOfType<DialogScreen>();
        }
        
        public override void Use() {
            // Open dialog UI
            var rnd = new System.Random();
            var nextNodeIndex = rnd.Next(0, PossibleStartingDialogNodes.Length);
            var startingNode = PossibleStartingDialogNodes[nextNodeIndex];
            _dialogUI.OnOpenDialog(SpeakerData, startingNode);
        }

    }

}