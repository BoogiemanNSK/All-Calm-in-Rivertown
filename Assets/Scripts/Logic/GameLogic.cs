using QuestGenerator;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Logic {

    public class GameLogic : MonoBehaviour {

        public static GameLogic Instance;
        
        public bool GameIsPaused { get; private set; }
        public Generator MainQuestGenerator { get; private set; }
        
        private void Awake() {
            Instance = this;
            ResumeGame();
            EventManager.InitDict();
            MainQuestGenerator = new Generator();
        }

        public void ResumeGame() {
            Cursor.visible = false;
            GameIsPaused = false;
        }

        // TODO Forbid player movement during pause
        public void PauseGame() {
            Cursor.visible = true;
            GameIsPaused = true;
        }

        public void ExitToMenu() {
            SceneManager.LoadScene(Constants.SnMenu);
        }

    }

}