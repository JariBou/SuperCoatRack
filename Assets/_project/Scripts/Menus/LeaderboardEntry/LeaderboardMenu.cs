using _project.ScriptableObjects.Scripts;
using _project.Scripts.Managers;
using _project.Scripts.Utils;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

namespace _project.Scripts.Menus.LeaderboardEntry
{
    public class LeaderboardMenu : MonoBehaviour
    {
        [SerializeField]
        private LevelsData _levelsData;
        
        [SerializeField]
        private Image _selectionDisplay;
        [SerializeField]
        LeaderboardDisplay _leaderboardDisplay;
        
        private int _selectedLevelIndex = -1;

        private int SelectedLevelIndex
        {
            get => _selectedLevelIndex;
            set
            {
                int diff = value - _selectedLevelIndex;
                LevelData levelData = LevelManager.Instance.ListOfLevels[value];
                if (levelData.IsTutorial)
                {
                    SelectedLevelIndex = MathUtils.Mod(value+diff, LevelManager.Instance.ListOfLevels.Length);
                    return;
                }
                _selectedLevelIndex = value;
                _selectionDisplay.sprite = levelData.Icon;
                _leaderboardDisplay.Show(levelData.LevelName);
            }
        }
        
        private void Start()
        {
            SelectedLevelIndex = 0;
        }
        
        public void SelectNext()
        {
            SelectedLevelIndex = (SelectedLevelIndex + 1) % LevelManager.Instance.ListOfLevels.Length;
        }

        public void SelectPrevious()
        {
            SelectedLevelIndex = MathUtils.Mod(SelectedLevelIndex-1, LevelManager.Instance.ListOfLevels.Length);
        }

        public void JoystickNext(InputAction.CallbackContext ctx)
        {
            if (!ctx.performed) return;
            SelectNext();
        }
        
        public void JoystickPrevious(InputAction.CallbackContext ctx)
        {
            if (!ctx.performed) return;
            SelectPrevious();
        }
    }
}