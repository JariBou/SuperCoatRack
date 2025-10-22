using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

namespace _project.Scripts.Menus.LeaderboardEntry
{
    public class LeaderboardNameSelectionDisplay : MonoBehaviour
    {
        [SerializeField] private List<LetterSelectionDisplay> _letterDisplays;
        [SerializeField] private Color _selectedColor;
        
        private LetterSelectionDisplay SelectedLetterDisplay => _letterDisplays.Find(display => display.IsSelected);
        private int _selectedIndex = 0;

        private void Start()
        {
            for (int i = 0; i < _letterDisplays.Count; i++)
            {
                _letterDisplays[i].SetSelected(i == 0);
                _letterDisplays[i].Config(_selectedColor);
            }
        }

        public void UpdateDisplay()
        {
            for (int i = 0; i < _letterDisplays.Count; i++)
            {
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
            if (!ctx.performed) return;
            if (_selectedIndex == 0)
            {
                
            }
        }
        
        public void OnJoystickConfirm(InputAction.CallbackContext ctx)
        {
            if (!ctx.performed) return;
            SelectedLetterDisplay.PreviousLetter();
        }
    }
}