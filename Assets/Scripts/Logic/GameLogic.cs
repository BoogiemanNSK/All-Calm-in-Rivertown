using UnityEngine;
using UnityEngine.SceneManagement;

namespace Logic {

    public class GameLogic : MonoBehaviour {

        public static bool GameIsPaused;

        private void Awake() {
            GameIsPaused = false;
            EventManager.InitDict();
        }

        public void ResumeGame() {
            Time.timeScale = 1.0f;
            GameIsPaused = false;
        }

        public void PauseGame() {
            Time.timeScale = 0.0f;
            GameIsPaused = true;
        }

        public void ExitToMenu() {
            SceneManager.LoadScene(Constants.SnMenu);
        }

    }

}