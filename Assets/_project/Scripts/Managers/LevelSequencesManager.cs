using System;
using System.Collections;
using _project.ScriptableObjects.Scripts;
using JetBrains.Annotations;
using UnityEngine;

namespace _project.Scripts.Managers
{
    public class LevelSequencesManager : MonoBehaviour
    {
        private class SequenceActionTimed
        {
            public SequenceData.SequenceAction SequenceAction;
            public float Timestamp;
            public SequenceActionState state = SequenceActionState.None;

            public SequenceActionTimed(SequenceData.SequenceAction actionOnBeat, float time)
            {
                SequenceAction = actionOnBeat;
                Timestamp = time;
            }

            public bool WasInputInTimeFrame(float lastInputTimestamp)
            {
                float timeDiff = Timestamp - lastInputTimestamp;
                return SequenceAction.gracePeriod.x > timeDiff && timeDiff > -SequenceAction.gracePeriod.y;
            }

            public void SetState(SequenceActionState newState)
            {
                state = newState;
            }

            public enum SequenceActionState
            {
                None,
                Failed,
                Succeeded,
            }
        }
        [SerializeField]
        private LevelData _levelData;
        
        private int _sequenceIndex;
        [SerializeField]
        private int _peakAmount;
        
        [CanBeNull] private Sequence _currentSequence;
        [CanBeNull] private SequenceActionTimed _lastSequenceAction;

        private InputTypeLink _lastInput;
        private float _lastInputTime;
        private bool _waitingForInput;

        private void Start()
        {
            _levelData = LevelManager.Instance.CurrentLevelData;
            AkUnitySoundEngine.PostEvent(
                "StopMusic",
                gameObject
            );
            GameManager.Instance.PlayLevelMusic();
        }

        public void OnBeat()
        {
            Debug.Log("Beat");
            if (_currentSequence == null) return;
            if (_currentSequence.DoBeat().HasActionOnBeat(out SequenceData.SequenceAction actionOnBeat))
            {
                _lastSequenceAction = new SequenceActionTimed(actionOnBeat, Time.time);
                HandleInput();
            }

            for (int i = 1; i <= _peakAmount; i++)
            {
                if (_currentSequence.PeakBeat(i, out SequenceData.SequenceAction peakedAction))
                {
                    
                }
            }
        }

        public void LoadNextSequence()
        {
            //TODO: increment _sequenceIndex where needed
            Debug.Log("LoadNextSequence");
            _currentSequence = Sequence.FromSequenceData(_levelData[_sequenceIndex]);
        }

        public IEnumerable WaitForInput(float delay)
        {
            _waitingForInput = true;
            yield return new WaitForSeconds(delay);
            _waitingForInput = false;
            if (_lastSequenceAction is { state: SequenceActionTimed.SequenceActionState.None })
            {
                OnFail();
            }
        }
        
        private void InputManagerOnLastInputChanged(InputTypeLink obj)
        {
            if (_lastInput is not null) return;
            if (_waitingForInput)
            {
                // Blablabla
                HandleInput();
            }
            _lastInput = obj;
        }

        private void HandleInput()
        {
            if (_lastInput is null || _lastSequenceAction is null) return;

            if (!_lastSequenceAction.WasInputInTimeFrame(_lastInput.Timestamp))
            {
                OnFail();
                return;
            }

            if (_lastSequenceAction.SequenceAction.ActionType == _lastInput.ActionType)
            {
                switch (_lastInput.ActionType)
                {
                    case SequenceConfig.ActionType.Pickup:
                    case SequenceConfig.ActionType.Drop:
                        if (_lastInput.ClotheType == _lastSequenceAction.SequenceAction.ClotheType)
                        {
                            OnSuccess();    
                            return;
                        }
                        break;
                    case SequenceConfig.ActionType.Scan:
                        if (_lastInput.ClotheType == _lastSequenceAction.SequenceAction.ClotheType &&
                            _lastInput.ClotheColor == _lastSequenceAction.SequenceAction.ClotheColor)
                        {
                            OnSuccess();
                            return;
                        }
                        break;
                    default:
                        throw new ArgumentOutOfRangeException();
                }
            }
            OnFail();
        }

        private void OnSuccess()
        {
            _lastSequenceAction!.SetState(SequenceActionTimed.SequenceActionState.Succeeded);
            _lastInput = null;
            Debug.Log("Success");
        }

        private void OnFail()
        {
            _lastSequenceAction!.SetState(SequenceActionTimed.SequenceActionState.Failed);
            _currentSequence = null;
            _lastInput = null;
            Debug.Log("You failed");
        }

        private void OnEnable()
        {
            InputManager.LastInputChanged += InputManagerOnLastInputChanged;
            //WwiseManager.BeatEvent += OnBeat;
            GameManager.Instance.onBeatUnityEvent += OnBeat;
            GameManager.Instance.SequenceEvent += LoadNextSequence;
        }
        
        private void OnDisable()
        {
            InputManager.LastInputChanged -= InputManagerOnLastInputChanged;
            //WwiseManager.BeatEvent -= OnBeat;
            GameManager.Instance.onBeatUnityEvent -= OnBeat;
            GameManager.Instance.SequenceEvent -= LoadNextSequence;

        }
    }
}