using GameSystems;
using GameSystems.DialogSystem;
using Info;
using Logic;
using QuestGenerator;
using UnityEngine;
using UnityEngine.UI;

namespace UI {
    public class DialogScreen : MonoBehaviour {

        [SerializeField] private GameObject Screen;
        [SerializeField] private TradingScreen TradingScreen;

        [Header("Dialogs Screen Specs")] 
        [SerializeField] private Text SpeakerName;
        [SerializeField] private Text PlayerName;
        [SerializeField] private Text SpeakerText;

        [SerializeField] private Transform AnswersGrid;
        [SerializeField] private PlayerSpeechLine LinePrefab;

        private Character _playerData;
        private Character _currentSpeaker;
        private DialogNode _currentNode;
        private Quest _lastGeneratedQuest;
        
        // Special dialog cases
        private bool _startTrading;
        private bool _generateQuest;

        public void OnOpenDialog(Character speakerData, DialogNode startingNode) {
            if (GameLogic.Instance.GameIsPaused) return;
            GameLogic.Instance.PauseGame();
            Screen.SetActive(true);

            _currentSpeaker = speakerData;
            _currentNode = startingNode;
            _startTrading = startingNode is TradeActionNode;
            SetCurrentNodeToUI();
            
            PlayerName.text = _playerData.Name;
            SpeakerName.text = _currentSpeaker.Name;
        }

        public void OnDialogLineClick(int index) {
            // Close dialog or start action if required
            if (_generateQuest && index == ((QuestActionNode) _currentNode).AcceptQuestAnswerIndex) {
                // TODO Add generated quest to player's journal
            }
            if (_currentNode.IsEndingNode) {
                OnCloseDialog();
                if (_startTrading) TradingScreen.OpenTradeMenu(_currentSpeaker.Inventory);
                return;
            }

            // Choose next dialog node
            _currentNode = _currentNode.NextNodes[index];
            _startTrading = _currentNode is TradeActionNode;
            _generateQuest = _currentNode is QuestActionNode;
            SetCurrentNodeToUI();
        }

        private void Awake() {
            var player = FindObjectOfType<PlayerController>();
            _playerData = player.gameObject.GetComponent<Character>();
        }

        private void SetCurrentNodeToUI() {
            if (_generateQuest) {
                var questGiverData = (NpcInfo) _currentSpeaker.CharacterData;
                var playerData = (PlayerInfo) _playerData.CharacterData;
                
                _lastGeneratedQuest = GameLogic.Instance.MainQuestGenerator.GenerateNewQuest(questGiverData, playerData);
                
                // TODO Fill UI according to generated quest
            }
            
            SpeakerText.text = _currentNode.NodeText;

            // Clear old answers
            foreach (Transform answerUI in AnswersGrid) {
                Destroy(answerUI.gameObject);
            }

            var i = 0;
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

    }
}