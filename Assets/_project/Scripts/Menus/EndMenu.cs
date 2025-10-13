using UnityEngine;
using UnityEngine.InputSystem;
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
        
        public void JoystickReturn(InputAction.CallbackContext ctx)
        {
            if(!ctx.performed) return;
            Retry();
        }

        public void JoystickConfirm(InputAction.CallbackContext ctx)
        {
            if(!ctx.performed) return;
            GoBackToMenu();
        }
    }
}