using GameSystems;
using Info;
using UI;

namespace Interactables {

    public class Talkable : Interactable {

        [SerializeField] private NPCInfo SpeakerData;
        [SerializeField] private DialogNode[] PossibleStartingDialogNodes;
        private DialogScreen _dialogUI;

        protected override void Awake() {
            base.Awake();
            _dialogUI = FindObjectOfType<DialogScreen>();
        }
        
        public override void Use() {
            // Open dialog UI
            Random rnd = new Random();
            var nextNodeIndex = rnd.Next(0, PossibleStartingDialogNodes.Length);
            var startingNode = PossibleStartingDialogNodes[nextNodeIndex];
            _dialogUI.OnOpenDialog(SpeakerData);
        }

    }

}