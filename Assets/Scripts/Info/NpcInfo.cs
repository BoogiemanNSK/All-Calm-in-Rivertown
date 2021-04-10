using UnityEngine;

namespace Info {
    [CreateAssetMenu(fileName = "NpcInfo", menuName = "Info/NPC", order = 72)]
    public class NpcInfo : CharInfo {

        // What group this NPC belongs to
        public ReputationInfo.Fractions NpcFraction;

    }
}