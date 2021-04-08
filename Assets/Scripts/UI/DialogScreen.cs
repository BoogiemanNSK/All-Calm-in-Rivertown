using Info;
using UnityEngine;

namespace UI {

    public class DialogScreen : MonoBehaviour {

        [SerializeField] protected GameObject Screen;

        [Header("Dialogs Screen Specs")]
        [SerializeField] private Text SpeakerName;
        [SerializeField] private Text PlayerName;
        [SerializeField] private Text SpeakerText;

        [SerializeField] private Transform AnswersGrid;
        [SerializeField] private PlayerSpeechLine LinePrefab;

        private DialogNode _currentNode;
        private bool _startTrading;

        public void OnOpenDialog(NPCInfo speakerData, DialogNode startingNode) {
            if (GameLogic.Instance.GameIsPaused) return;
            GameLogic.Instance.PauseGame();
            Screen.SetActive(true);

            _startTrading = false;
            _currentNode = startingNode;
            SetCurrentNodeToUI();        
        }

        private void SetCurrentNodeToUI() {
            // TODO Set player name
            // TODO Set NPC name
            SpeakerText.text = _currentNode.NodeText;

            // Clear old answers
            foreach (Transform answerUI in AnswersGrid) {
                Destroy(answerUI.gameObject);
            }

            int i = 0;
            foreach (var answer in _currentNode.PossibleAnswers) {
                var answerUI = Instantiate(LinePrefab, AnswersGrid);
                answerUI.SetSpeechLine(answer, i, this);
                i++;
            }   
        }

        private void OnCloseDialog() {
            Screen.SetActive(false);
            GameLogic.Instance.ResumeGame();
        }

        public void OnDialogLineClick(int index) {
            // Close dialog and start action if required
            if (_currentNode.IsEndingNode) {
                OnCloseDialog();
                
                if (_startTrading) {
                    // TODO Start trading
                }

                return;
            }

            if (_currentNode is TradeActionNode) {
                // Start trading action after dialog
                _startTrading = true;
            }

            // Choose next dialog node
            _currentNode = NextNodes[index];
            SetCurrentNodeToUI();     
        }

    }

}