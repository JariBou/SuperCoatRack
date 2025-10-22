using System;
using System.Collections.Generic;
using System.Linq;
using _project.Scripts.Managers;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

namespace _project.Scripts.Menus.LeaderboardEntry
{
    public class LeaderboardNameSelectionDisplay : MonoBehaviour
    {
        [SerializeField] private List<LetterSelectionDisplay> _letterDisplays;
        [SerializeField] private Color _unselectedColor = Color.white;
        [SerializeField] private Color _selectedColor;
        [SerializeField] private EndMenu _endMenu;
        
        [SerializeField] private LeaderboardDisplay _leaderboardDisplay;
        
        
        private LetterSelectionDisplay SelectedLetterDisplay => _letterDisplays.Find(display => display.IsSelected);
        private int _selectedIndex = 0;

        private void Start()
        {
            for (int i = 0; i < _letterDisplays.Count; i++)
            {
                _letterDisplays[i].Config(_unselectedColor, _selectedColor);
                _letterDisplays[i].SetSelected(i == 0);
            }
        }

        public void UpdateDisplay()
        {
            for (int i = 0; i < _letterDisplays.Count; i++)
            {
                // if (i < _selectedIndex)
                // {
                //     _letterDisplays[i].Lock();
                // }
                // else
                // {
                //     _letterDisplays[i].Unlock();
                // }
                _letterDisplays[i].SetSelected(i == _selectedIndex);
            }
        }

        public void OnJoystickNext(InputAction.CallbackContext ctx)
        {
            if (!ctx.performed) return;
            SelectedLetterDisplay.NextLetter();
        }
        
        public void OnJoystickPrevious(InputAction.CallbackContext ctx)
        {
            if (!ctx.performed) return;
            SelectedLetterDisplay.PreviousLetter();
        }
        
        public void OnJoystickBack(InputAction.CallbackContext ctx)
        {
            if (!ctx.performed || _selectedIndex >= _letterDisplays.Count) return;
            if (_selectedIndex == 0)
            {
                _endMenu.EnableNav();
                Destroy(gameObject);
            }
            else
            {
                _selectedIndex--;
                UpdateDisplay();
            }
        }

        private string GetPlayerEnteredName()
        {
            return _letterDisplays.Aggregate(string.Empty, (current, t) => current + t.GetLetter());
        }
        
        public void OnJoystickConfirm(InputAction.CallbackContext ctx)
        {
            if (!ctx.performed || _selectedIndex >= _letterDisplays.Count) return;
            _selectedIndex++;
            UpdateDisplay();
            if (_selectedIndex >= _letterDisplays.Count)
            {
                _leaderboardDisplay.Show(GetPlayerEnteredName());
                _endMenu.EnableNav();
                HideDisplays();
            }
        }

        private void HideDisplays()
        {
            foreach (LetterSelectionDisplay display in _letterDisplays)
            {
                display.gameObject.SetActive(false);
            }
        }
    }
}