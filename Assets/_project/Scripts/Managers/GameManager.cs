using System;
using NaughtyAttributes;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace _project.Scripts.Managers
{
    public class GameManager : MonoBehaviour 
    {
        private static GameManager _instance;
        public static GameManager Instance => _instance;

        private void Awake()
        {
            if (_instance != null)
            {
                Destroy(gameObject);
            }
            else
            {
                _instance = this;
            }
            DontDestroyOnLoad(gameObject);
        }

        [Button]
        public void EndGame()
        {
            SceneManager.LoadScene("EndScene");
        }
    }
}