using System;
using _project.Scripts.Menus.LeaderboardHelpers;
using UnityEngine;
using UnityEngine.InputSystem;

namespace _project.Scripts.Menus.MainMenu
{
    public class MainMenuScript : MonoBehaviour
    {
        [SerializeField] 
        private GameObject _mainMenuPanel;
        [SerializeField] 
        private GameObject _settingsPanel;
        
        [SerializeField] 
        private GameObject _selectionPanel;
        
        [SerializeField]
        private SelectionMenu _selectionMenu;
        
        [SerializeField]
        private LeaderboardMenu _leaderboardMenu;

        private void Awake()
        {
            _settingsPanel.SetActive(true);
        }
        
        private void Start()
        {
            _settingsPanel.SetActive(false);
            _selectionPanel.SetActive(false);
            _leaderboardMenu.gameObject.SetActive(false);
        }

        public void OnJoystickConfirm(InputAction.CallbackContext ctx)
        {
            if (!ctx.performed) return;
            if (_selectionPanel.activeSelf)
            {
                _selectionMenu.Play();
            } else if (_leaderboardMenu.gameObject.activeSelf)
            {
                
            }
            else
            {
                ToggleSelection();
            }
        }
        
        public void OnJoystickReturn(InputAction.CallbackContext ctx)
        {
            if (!ctx.performed) return;
            if (_selectionPanel.activeSelf)
            {
                ToggleSelection();
            }
            else if (_leaderboardMenu.gameObject.activeSelf || _mainMenuPanel.activeSelf)
            {
                ToggleLeaderboard();    
            }
        }

        public void ToggleSelection()
        {
            _mainMenuPanel.SetActive(!_mainMenuPanel.activeSelf);
            _selectionPanel.SetActive(!_selectionPanel.activeSelf);
            // AkUnitySoundEngine.PostEvent(
            //     "StopMusic",
            //     gameObject
            // );
            // AkUnitySoundEngine.PostEvent(
            //     _mainMenuPanel.activeSelf? "PlayWiiMusic" : "PlayTestMusic",
            //     gameObject
            // );
        }

        public void ToggleSettings()
        {
            _mainMenuPanel.SetActive(!_mainMenuPanel.activeSelf);
            _settingsPanel.SetActive(!_settingsPanel.activeSelf);
            // AkUnitySoundEngine.PostEvent(
            //     "StopMusic",
            //     gameObject
            // );
            // AkUnitySoundEngine.PostEvent(
            //     _settingsPanel.activeSelf? "PlayDaschlatt" : "PlayWiiMusic",
            //     gameObject
            // );
        }

        public void ToggleLeaderboard()
        {
            _mainMenuPanel.SetActive(!_mainMenuPanel.activeSelf);
            _leaderboardMenu.gameObject.SetActive(!_leaderboardMenu.gameObject.activeSelf);
        }
        

        public void Quit()
        {
            Application.Quit();
        }
        
    }
}
