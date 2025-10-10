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

        public GameState CurrentState
        {
            get => _currentState;
            set => _currentState = value;
        }

        [SerializeField] private GameState _currentState;

        public event Action onBeatUnityEvent ;
        public event Action SequenceEvent ;

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
            _currentState = GameState.Menu;
        }
        

        [Button]
        public void EndGame()
        {
            SceneManager.LoadScene("EndScene");
            _currentState = GameState.Menu;
        }

        public void PlayLevelMusic()
        {
            AkUnitySoundEngine.PostEvent(
                LevelManager.Instance.CurrentLevelData.MusicToPlayEvent.Name,
                gameObject, 
                (uint)AkCallbackType.AK_MusicSyncUserCue | (uint)AkCallbackType.AK_EndOfEvent | (uint)AkCallbackType.AK_MusicSyncBeat,
                OnBeatEvent,
                null
            );
        }

        private void OnBeatEvent(object in_cookie, AkCallbackType in_type, AkCallbackInfo in_info)
        {
            if (((uint)in_type & (uint)AkCallbackType.AK_MusicSyncBeat) == (uint)AkCallbackType.AK_MusicSyncBeat)
            {    
                onBeatUnityEvent?.Invoke();
            }

            if (((uint)in_type & (uint)AkCallbackType.AK_EndOfEvent) == (uint)AkCallbackType.AK_EndOfEvent)
            {
                Debug.Log("End of beat");
                EndGame();
            }

            if (((uint)in_type & (uint)AkCallbackType.AK_MusicSyncUserCue) == (uint)AkCallbackType.AK_MusicSyncUserCue)
            {
                Debug.Log("Event Trigger");
                SequenceEvent?.Invoke();
            }
        }
    }

    public enum GameState
    {
        Menu,
        InGame,
        Score
    }
}