using UnityEngine;
using UnityEngine.SceneManagement;

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
        
        private void Start()
        {
            _settingsPanel.SetActive(false);
            _selectionPanel.SetActive(false);
        }

        public void ToggleSelection()
        {
            _mainMenuPanel.SetActive(!_mainMenuPanel.activeSelf);
            _selectionPanel.SetActive(!_selectionPanel.activeSelf);
        }

        public void ToggleSettings()
        {
            _mainMenuPanel.SetActive(!_mainMenuPanel.activeSelf);
            _settingsPanel.SetActive(!_settingsPanel.activeSelf);
        }

        public void Quit()
        {
            Application.Quit();
        }
        
    }
}
