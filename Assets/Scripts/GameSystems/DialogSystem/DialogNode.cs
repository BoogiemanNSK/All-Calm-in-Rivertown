using UnityEngine;

namespace GameSystems.DialogSystem {

    [CreateAssetMenu(fileName = "DialogNode", menuName = "Dialogs/DialogNode", order = 61)]
    public class DialogNode : ScriptableObject {

        [SerializeField] private string _text;
        [SerializeField] private string[] _answers;
        [SerializeField] private DialogNode[] _nextNodes;

        public bool IsEndingNode => _nextNodes == null || _nextNodes.Length == 0;
        public string NodeText => _text;
        public string[] PossibleAnswers => _answers;
        public DialogNode[] NextNodes => _nextNodes;

    }

}