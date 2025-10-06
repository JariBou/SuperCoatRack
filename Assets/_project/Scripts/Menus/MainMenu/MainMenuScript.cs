using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Serialization;

namespace _project.Scripts.Menus.MainMenu
{
    public class MainMenuScript : MonoBehaviour
    {
        [SerializeField] 
        private GameObject _mainMenuPanel;
        [SerializeField] 
        private GameObject _settingsPanel;
        
        private void Start()
        {
            _settingsPanel.SetActive(false);
        }

        public void Play()
        {
            SceneManager.LoadScene("GameScene");
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
