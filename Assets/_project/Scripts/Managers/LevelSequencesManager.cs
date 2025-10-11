using System;
using System.Collections;
using _project.ScriptableObjects.Scripts;
using _project.Scripts.Managers.Inputs;
using JetBrains.Annotations;
using UnityEngine;

namespace _project.Scripts.Managers
{
    public class LevelSequencesManager : MonoBehaviour
    {
        [Serializable]
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
        private int _peakAmount = 4;
        
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
            //Debug.Log("Beat");
            if (_currentSequence == null) return;
            if (_currentSequence.DoBeat().HasActionOnBeat(out SequenceData.SequenceAction actionOnBeat))
            {
                Debug.Log("Sequence Has Action on beat");
                _lastSequenceAction = new SequenceActionTimed(actionOnBeat, Time.time);
                HandleInput();
            }

            for (int i = 1; i <= _peakAmount; i++)
            {
                if (_currentSequence.PeakBeat(i, out SequenceData.SequenceAction peakedAction))
                {
                    if (i == 1)
                    {
                        Debug.Log("§144NEXT Sequence Has Action on beat");
                    }
                }
            }
        }

        public void LoadNextSequence()
        {
            //TODO: increment _sequenceIndex where needed
            Debug.Log("LoadNextSequence");
            // TEMP =========
            if (_currentSequence != null) return;
            // ==============
            _currentSequence = Sequence.FromSequenceData(_levelData[_sequenceIndex]);
        }

        public IEnumerator WaitForInput(float delay)
        {
            _waitingForInput = true;
            yield return new WaitForSeconds(delay);
            _waitingForInput = false;
            if (_lastSequenceAction is { state: SequenceActionTimed.SequenceActionState.None })
            {
                Debug.Log("Failing after waiting....");
                OnFail();
            }
        }
        
        private void InputManagerOnLastInputChanged(InputTypeLink obj)
        {
            if (_lastInput is not null) return;
            _lastInput = obj;
            if (_waitingForInput)
            {
                Debug.Log("We were waiting for input !");
                // Blablabla
                HandleInput(true);
                return;
            }
            Debug.Log("Not waiting for input...");
        }

        private void HandleInput(bool inputWaited = false)
        {
            Debug.Log("=== HANDLEINPUT ===");
            if (_lastInput is null)
            {
                Debug.Log("_lastInput was null");
                if (inputWaited)
                {
                    OnFail();
                }

                if (_lastSequenceAction is not null)
                {
                    StartCoroutine(WaitForInput(_lastSequenceAction.SequenceAction.gracePeriod.y));
                }
                return;
            }
            if (_lastSequenceAction is null)
            {
                Debug.Log("_lastSequenceAction was null");
                return;
            }

            if (!_lastSequenceAction.WasInputInTimeFrame(_lastInput.Timestamp))
            {
                Debug.Log($"Input was out of out of time: {_lastSequenceAction.Timestamp - _lastInput.Timestamp}");
                OnFail();
                return;
            }

            if (_lastSequenceAction.SequenceAction.ActionType == _lastInput.ActionType)
            {
                Debug.Log($"Actions were of same type: {_lastSequenceAction.SequenceAction.ActionType}");
                switch (_lastInput.ActionType)
                {
                    case SequenceConfig.ActionType.Pickup:
                    case SequenceConfig.ActionType.Drop:
                        if (_lastInput.ClotheType == _lastSequenceAction.SequenceAction.ClotheType)
                        {
                            OnSuccess();    
                            return;
                        }
                        Debug.Log($"Clothe type was not the same {_lastInput.ClotheType} != {_lastSequenceAction.SequenceAction.ClotheType}");
                        break;
                    case SequenceConfig.ActionType.Scan:
                        if (_lastInput.ClotheType == _lastSequenceAction.SequenceAction.ClotheType &&
                            _lastInput.ClotheColor == _lastSequenceAction.SequenceAction.ClotheColor)
                        {
                            OnSuccess();
                            return;
                        }
                        Debug.Log($"Clothe type was not the same {_lastInput.ClotheType} != {_lastSequenceAction.SequenceAction.ClotheType} or Color was not same {_lastInput.ClotheColor} !=  {_lastSequenceAction.SequenceAction.ClotheColor}");
                        break;
                    default:
                        throw new ArgumentOutOfRangeException();
                }
            }

            if (inputWaited)
            {
                OnFail();
            } else {
                Debug.Log("Waiting for input...");
                StartCoroutine(WaitForInput(_lastSequenceAction.SequenceAction.gracePeriod.y));
            }
            // OnFail();
        }

        private void OnSuccess()
        {
            if (_lastSequenceAction?.state != SequenceActionTimed.SequenceActionState.None) return;
            
            _lastSequenceAction.SetState(SequenceActionTimed.SequenceActionState.Succeeded);
            _lastInput = null;
            
            if (_lastSequenceAction.SequenceAction.ActionType == SequenceConfig.ActionType.Bell)
            {
                _sequenceIndex++;
            }
            Debug.Log("Success");
        }

        private void OnFail()
        {
            if (_lastSequenceAction?.state != SequenceActionTimed.SequenceActionState.None) return;
            
            _lastSequenceAction!.SetState(SequenceActionTimed.SequenceActionState.Failed);
            _lastInput = null;
            
            if (_lastSequenceAction.SequenceAction.ActionType == SequenceConfig.ActionType.Bell)
            {
                _sequenceIndex++;
            }
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