using System;
using _project.ScriptableObjects.Scripts;
using UnityEngine;

namespace _project.Scripts.Managers
{
    public class ScoreManager : MonoBehaviour
    {
        public static ScoreManager Instance => _instance;
        private static ScoreManager _instance;

        [SerializeField]
        private ScoreData _scoreData;

        private int _score;
        public int Score => _score;
        
        private void Awake()
        {
            if (_instance != null)
            {
                Destroy(gameObject);
                return;
            }
            
            _instance = this;
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

        private void AddScore(int scoreAmount)
        {
            _score += scoreAmount;
        }
    }
}