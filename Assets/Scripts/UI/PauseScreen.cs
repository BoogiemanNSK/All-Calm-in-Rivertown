using Logic;
using UnityEngine;

namespace UI {

    public class PauseScreen : MonoBehaviour {

        [SerializeField] private GameObject Screen;
        
        private bool _isOpened;

        private void Awake() {
            _isOpened = false;
        }

        private void Start() {
            EventManager.StartListening(Constants.PauseKeyPressedEvent, PauseButtonPressed);
        }

        public void ClosePause() {
            _isOpened = false;
            Screen.SetActive(false);
            GameLogic.Instance.ResumeGame();
        }

        public void ExitToMenu() {
            GameLogic.Instance.ExitToMenu();
        }

        private void OpenPause() {
            if (GameLogic.Instance.GameIsPaused) return;
            GameLogic.Instance.PauseGame();

            Screen.SetActive(true);
            _isOpened = true;
        }
        
        private void PauseButtonPressed(string _) {
            if (_isOpened) ClosePause();
            else OpenPause();
        }

    }

}