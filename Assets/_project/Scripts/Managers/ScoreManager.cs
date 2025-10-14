using System;
using _project.ScriptableObjects.Scripts;
using UnityEngine;

namespace _project.Scripts.Managers
{
    public class ScoreManager : MonoBehaviour
    {
        public static ScoreManager Instance { get; private set; }

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
                    return _scoreData.BadRatingScore;
                case LevelSequencesManager.SequenceActionState.Good:
                    return _scoreData.GoodRatingScore;
                case LevelSequencesManager.SequenceActionState.Perfect:
                    return _scoreData.PerfectRatingScore;
                default:
                    throw new ArgumentOutOfRangeException(nameof(actionState), actionState, null);
            }
            return 0;
        }

        public static void SequenceActionFinished(LevelSequencesManager.SequenceActionTimed lastSequenceAction)
        {
            Instance.AddScore(Instance.GetScoreOfState(lastSequenceAction.state));
        }

        private void GameManagerOnGameStart()
        {
            Score = 0;
        }

        private void AddScore(int scoreAmount)
        {
            Score += scoreAmount;
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