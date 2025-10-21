using System;
using _project.ScriptableObjects.Scripts;
using UnityEngine;

namespace _project.Scripts.Managers
{
    public class ScoreManager : MonoBehaviour
    {
        public static ScoreManager Instance { get; private set; }

        private float maxPossibleScorePerLevel = 0;
        public static event Action<int> ScoreChanged;

        [SerializeField]
        private ScoreData _scoreData;

        private int _score;

        public int Score
        {
            get => _score;
            private set
            {
                _score = value;
                ScoreChanged?.Invoke(_score);
            }
        }

        public ScoreData Data => _scoreData;

        public float MaxPossibleScorePerLevel
        {
            get => maxPossibleScorePerLevel;
            set => maxPossibleScorePerLevel = value;
        }

        private void Awake()
        {
            if (Instance != null)
            {
                Destroy(gameObject);
                return;
            }
            
            Instance = this;
        }

        private int GetScoreOfState(LevelSequencesManager.SequenceActionState actionState)
        {
            switch (actionState)
            {
                case LevelSequencesManager.SequenceActionState.None:
                case LevelSequencesManager.SequenceActionState.Failed:
                case LevelSequencesManager.SequenceActionState.Succeeded:
                    break;
                case LevelSequencesManager.SequenceActionState.Bad:
                    return Data.BadRatingScore;
                case LevelSequencesManager.SequenceActionState.Good:
                    return Data.GoodRatingScore;
                case LevelSequencesManager.SequenceActionState.Perfect:
                    return Data.PerfectRatingScore;
                default:
                    throw new ArgumentOutOfRangeException(nameof(actionState), actionState, null);
            }
            return 0;
        }

        public float GetLevelMaxScoreNeeded(LevelData levelData)
        {
            maxPossibleScorePerLevel = levelData.sequences.Count * 100;
            return maxPossibleScorePerLevel;
        }

        public static void SequenceActionFinished(LevelSequencesManager.SequenceActionTimed lastSequenceAction)
        {
            Instance.AddScore(Instance.GetScoreOfState(lastSequenceAction.state));
        }

        private void GameManagerOnGameStart()
        {
            Score = 0;
        }

        public static void Reset()
        {
            Instance.Score = 0;
        }

        private void AddScore(int scoreAmount)
        {
            Score += scoreAmount;
            UIManager.Instance.ChangeScoreValue(Score);
        }

        private void OnEnable()
        {
            GameManager.GameStart += GameManagerOnGameStart;
        }

        private void OnDisable()
        {
            GameManager.GameStart -= GameManagerOnGameStart;
        }
    }
}