using UnityEngine;
using UnityEngine.SceneManagement;

namespace _project.Scripts.Menus
{
    public class EndMenu : MonoBehaviour
    {
        public void Retry()
        {
            SceneManager.LoadScene("GameScene");
        }
        
        public void GoBackToMenu()
        {
            SceneManager.LoadScene("MainMenu");
        }
    }
}