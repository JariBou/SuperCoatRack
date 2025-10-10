using System;
using UnityEngine;

namespace _project.Scripts.Managers.Inputs
{
    public class GlobalInputManager : MonoBehaviour
    {
        private GlobalInputManager _instance;
        
        private PlayerInputsMap _playerInputsMap;

        private void Awake()
        {
            if (_instance != null)
            {
                Destroy(gameObject);
                return;
            }
            _instance = this;
            DontDestroyOnLoad(gameObject);
            
            _playerInputsMap = new PlayerInputsMap();
            _playerInputsMap.Enable();
        }
        
        public void EnableMenuInputs()
        {
            _playerInputsMap.InGameMap.Disable();
            _playerInputsMap.MenuMap.Enable();
        }
        
        public void EnableInGameInputs()
        {
            _playerInputsMap.MenuMap.Disable();
            _playerInputsMap.InGameMap.Enable();
        }
        
        public void EnableInGameInputsStatic()
        {
            _instance.EnableInGameInputs();
        }
        
        public void EnableMenuInputsStatic()
        {
            _instance.EnableMenuInputs();
        }

        private void OnDisable()
        {
            _playerInputsMap.Disable();
        }
    }
}