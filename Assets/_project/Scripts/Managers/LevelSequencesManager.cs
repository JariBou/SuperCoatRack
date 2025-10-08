using _project.ScriptableObjects.Scripts;
using JetBrains.Annotations;
using UnityEngine;

namespace _project.Scripts.Managers
{
    public class LevelSequencesManager : MonoBehaviour
    {
        [SerializeField]
        private LevelData _levelData;
        
        private int _sequenceIndex;
        // qzdijqzui dhiuqzdiqzhjduik
        [CanBeNull] private Sequence _currentSequence;

        private object _lastInput;
        private float _lastInputTime;
        
        public void OnBeat()
        {
            if (_currentSequence?.DoBeat().HasActionOnBeat(out SequenceData.SequenceAction actionOnBeat) ?? false)
            {
                
            }
        }

        public void PlayNextSequence()
        {
            _currentSequence = Sequence.FromSequenceData(_levelData[_sequenceIndex]);
        }
    }
}