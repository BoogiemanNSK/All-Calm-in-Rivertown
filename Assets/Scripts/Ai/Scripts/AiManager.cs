using System.Collections;
using UnityEngine;

namespace Ai.Scripts {

    public static class AiManager {

        private static RaycastHit _hit;
        private static GameObject _player;

        // Finds player on game start
        public static IEnumerator Ai_Lists() {
            try {
                _player = GameObject.FindGameObjectsWithTag(Constants.PlayerTag)[0];
            }
            catch (UnityException ex) {
                Debug.Log(ex.Message + "\n" +
                          " Please go to Edit > Project Settings > Tags and Layers\n" +
                          "Maximize the Tag section and add the proper Tags.\n" +
                          "Maximize the Layers section and add the proper Layers.\n" +
                          "You can find the right Tags and Layers in the ReadMe file\n" +
                          "Go to BreadcrumbAi > Ai > Documentation > ReadMe\n\n");
            }
            
            yield return null;
        }

        // Set up the layermasks to specific layers
        public static IEnumerator Ai_Layers(this Ai ai) {
            ai.playerLayer = 1 << LayerMask.NameToLayer(Constants.PlayerTag);
            ai.enemyLayer = 1 << LayerMask.NameToLayer(Constants.EnemyTag);

            yield return null;
        }
        
        // Check if player is in range
        public static Transform Ai_FindPlayer(this Ai ai) {
            // Is player in detection range?
            ai.Player = ai.InRange(_player.transform.position, ai.visionDistance) ? _player.transform : null;
            return ai.Player;
        }

    }

}