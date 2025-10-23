using System;
using _project.ScriptableObjects.Scripts;
using NaughtyAttributes;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace _project.Scripts.Managers
{
    public class GameManager : MonoBehaviour 
    {
        public static event Action GameStart;
        public event Action OnBeatUnityEvent ;
        public event Action SequenceEvent ;
        
        #region Init
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
            _currentState = GameState.Menu;
        }
        #endregion
        
        public GameState CurrentState
        {
            get => _currentState;
            set => _currentState = value;
        }

        [SerializeField] private GameState _currentState;

        private uint playingId; //Unused for wwise
        private LevelData _levelData;

        public float Timer { get; private set; } = 0f;
        private float songAdvancement;
        
        private void Update()
        {
            if (!_levelData) return;
            if(Timer >= _levelData._songDurationInSeconds || UIManager.Instance == null) return;
            Timer += Time.deltaTime;
            songAdvancement = Mathf.Clamp01(Timer / _levelData._songDurationInSeconds);
            UIManager.Instance.ChangeElevatorPosition(songAdvancement);
        }
        [Button]
        public void EndGame()
        {
            SceneManager.LoadScene("EndScene");
            _currentState = GameState.Score;
        }
        
        [Button]
        public void GoToMenu()
        {
            SceneManager.LoadScene("MainMenu");
            _currentState = GameState.Menu;
        }

        public void PlayLevelMusic()
        {
            if (!LevelManager.Instance.CurrentLevelData.MusicToPlayEvent.IsValid()) return;
            playingId = AkUnitySoundEngine.PostEvent(
                LevelManager.Instance.CurrentLevelData.MusicToPlayEvent.Name,
                gameObject, 
                (uint)AkCallbackType.AK_MusicSyncUserCue | (uint)AkCallbackType.AK_EndOfEvent | (uint)AkCallbackType.AK_MusicSyncBeat,
                OnBeatEvent,
                null
            );
            _levelData = LevelManager.Instance.CurrentLevelData;
            Debug.Log(_levelData.name);
        }

        private void OnBeatEvent(object in_cookie, AkCallbackType in_type, AkCallbackInfo in_info)
        {
            if (((uint)in_type & (uint)AkCallbackType.AK_MusicSyncBeat) == (uint)AkCallbackType.AK_MusicSyncBeat)
            {   
                //On Beat
                OnBeatUnityEvent?.Invoke();
            }

            if (((uint)in_type & (uint)AkCallbackType.AK_EndOfEvent) == (uint)AkCallbackType.AK_EndOfEvent)
            {
                //On End Beat
                Debug.Log("End of beat");
                EndGame();
            }

            if (((uint)in_type & (uint)AkCallbackType.AK_MusicSyncUserCue) == (uint)AkCallbackType.AK_MusicSyncUserCue)
            {
                //On Cue 
                Debug.Log("Event Trigger");
                SequenceEvent?.Invoke();
            }
        }

        public void BeginLevel()
        {
            AkUnitySoundEngine.PostEvent("Play_Start_Sound", gameObject); // TEMP ALED TOME AAAAAAAAAAA https://www.youtube.com/watch?v=Z5910kZl3Rk
            ScoreManager.Reset();
            PlayLevelMusic();
            GameStart?.Invoke();
        }
    }

    public enum GameState
    {
        Menu,
        InGame,
        Score
    }
}