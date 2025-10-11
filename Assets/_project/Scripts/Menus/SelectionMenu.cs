using _project.ScriptableObjects.Scripts;
using _project.Scripts.Managers;
using _project.Scripts.Utils;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace _project.Scripts.Menus
{
    public class SelectionMenu : MonoBehaviour
    {
        [SerializeField]
        private LevelsData _levelsData;
        
        [SerializeField]
        private Image _selectionDisplay;
        
        private int _selectedLevelIndex;

        private int SelectedLevelIndex
        {
            get => _selectedLevelIndex;
            set
            {
                _selectedLevelIndex = value;
                _selectionDisplay.sprite = LevelManager.Instance.ListOfLevels[_selectedLevelIndex].Icon;
            }
        }

        private void Start()
        {
            SelectedLevelIndex = 0;
        }
        
        public void Play()
        {
            LevelManager.Instance.CurrentLevelData = LevelManager.Instance.ListOfLevels[SelectedLevelIndex];
            GameManager.Instance.CurrentState = GameState.InGame;
            SceneManager.LoadScene("GameScene");
        }

        public void SelectNext()
        {
            SelectedLevelIndex = (SelectedLevelIndex + 1) % LevelManager.Instance.ListOfLevels.Length;
        }

        public void SelectPrevious()
        {
            SelectedLevelIndex = MathUtils.Mod(SelectedLevelIndex+1, LevelManager.Instance.ListOfLevels.Length); ;
        }
        
    }
}