namespace GameSystems {

    [CreateAssetMenu(fileName = "DialogNode", menuName = "Dialogs/DialogNode", order = 61)]
    public class DialogNode : ScriptableObject {

        [SerializeField] private string _text;
        [SerializeField] private string[] _answers;
        [SerializeField] private DialogNode[] _nextNodes;

        public bool IsEndingNode => _nextNodes == null;
        public string NodeText => _text;
        public string[] PossibleAnswers => _nextNodes;
        public DialogNode[] NextNodes => _nextNodes;

    }

}