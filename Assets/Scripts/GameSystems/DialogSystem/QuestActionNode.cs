using UnityEngine;

namespace GameSystems.DialogSystem {

    [CreateAssetMenu(fileName = "QuestActionNode", menuName = "Dialogs/QuestActionNode", order = 63)]
    public class QuestActionNode : DialogNode {

        [SerializeField] private int _acceptQuestAnswerIndex;

        public int AcceptQuestAnswerIndex => _acceptQuestAnswerIndex;

    }

}