using UnityEngine;

namespace Info {
    [CreateAssetMenu(fileName = "PlayerInfo", menuName = "Info/Player", order = 71)]
    public class PlayerInfo : CharInfo {

        public ReputationInfo Reputation;

        public enum PlayerTypes {

            Trader = 0,
            Fighter = 1,
            Collector = 2

        }

        private void OnEnable() {
            Reputation = new ReputationInfo();
        }

    }
}