using System;
using _project.Scripts.Managers;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

namespace _project.Scripts.Menus
{
    public class EndMenu : MonoBehaviour
    {
        [SerializeField] private TMP_Text _scoreDispaly;
        
        private void Start()
        {
            _scoreDispaly.text = $"Score: {ScoreManager.Instance.Score.ToString()}";
        }

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