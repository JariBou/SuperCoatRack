using _project.ScriptableObjects.Scripts;
using UnityEngine;

namespace _project.Scripts.Managers
{
    public class LevelManager : MonoBehaviour
    {
        private static LevelManager _instance;
        public static LevelManager Instance => _instance;

        [SerializeField] private LevelData[] listOfLevels;
    
        public LevelData[] ListOfLevels => listOfLevels;

        public LevelData CurrentLevelData
        {
            get => _currentLevelData;
            set => _currentLevelData = value;
        }

        public static LevelData CurrentLevel => _instance.CurrentLevelData;

        [SerializeField] private LevelData _currentLevelData;
    
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
    
    }
}
