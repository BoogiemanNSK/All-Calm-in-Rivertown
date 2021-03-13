using UnityEngine;
using UnityEngine.SceneManagement;

namespace Logic
{
    public class MenuLogic : MonoBehaviour
    {

        public void StartTheGame()
        {
            SceneManager.LoadScene(Constants.SnGame);
        }

        public void ExitTheGame()
        {
            Application.Quit();
        }
        
    }
}