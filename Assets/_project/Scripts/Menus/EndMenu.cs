using System;
using _project.Scripts.Managers;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using UnityEngine.Serialization;
using UnityEngine.UI;

namespace _project.Scripts.Menus
{
    public class EndMenu : MonoBehaviour
    {
        [SerializeField] private TMP_Text _scoreDisplay;
        [FormerlySerializedAs("_scoreNote")] [SerializeField] private Image _scoreGrade;
        
        private void Start()
        {
            _scoreDisplay.text = $"Score: {ScoreManager.Instance.Score.ToString()}";
            _scoreGrade.sprite = UIManager.Instance.GetGradeSprite();
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