using _project.Scripts.LeaderboardSripts;
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
        private bool _isNavEnabled = false;
        
        [SerializeField] private GameObject _retryButton;
        [SerializeField] private GameObject _gotoMenuButton;
        
        private void Start()
        {
            _retryButton.SetActive(false);
            _gotoMenuButton.SetActive(false);
            // int position = Leaderboard.GetPositionOfScoreInMap(300, "test");
            int position = Leaderboard.GetPositionOfScoreInMap(ScoreManager.GetScore(), LevelManager.CurrentLevel.LevelName);
            // _scoreDisplay.text = $"{position}. {300}";
            _scoreDisplay.text = $"{position}. {ScoreManager.Instance.Score.ToString()}";
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
            if(!ctx.performed || !_isNavEnabled) return;
            Retry();
        }

        public void JoystickConfirm(InputAction.CallbackContext ctx)
        {
            if(!ctx.performed || !_isNavEnabled) return;
            GoBackToMenu();
        }

        public void EnableNav()
        {
            _isNavEnabled = true;
            
            _retryButton.SetActive(true);
            _gotoMenuButton.SetActive(true);
        }
    }
}