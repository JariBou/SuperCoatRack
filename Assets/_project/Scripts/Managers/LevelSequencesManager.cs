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
        public enum SequenceActionState
        {
            None = 0,
            Failed = 1,
            Succeeded = 2,
            Bad = 3,
            Good = 4,
            Perfect = 5
        }
        
        [Serializable]
        public class SequenceActionTimed
        {
            public SequenceData.SequenceAction SequenceAction;
            public float Timestamp;
            public SequenceActionState state = SequenceActionState.None;

            public SequenceActionTimed(SequenceData.SequenceAction actionOnBeat, float time)
            {
                SequenceAction = actionOnBeat;
                Timestamp = time;
            }

            public bool WasInputInTimeFrame(float lastInputTimestamp, out SequenceActionState state)
            {
                float timeDiff = Timestamp - lastInputTimestamp;
                if (!(SequenceAction.gracePeriod.x > timeDiff && timeDiff > -SequenceAction.gracePeriod.y))
                {
                    state = SequenceActionState.Failed;
                    return false;
                }

                float abs = Mathf.Abs(timeDiff);
                if (Mathf.Approximately(Mathf.Sign(timeDiff), 1))
                {
                    float dTime = abs / SequenceAction.gracePeriod.x;
                    CheckInputState(out state, dTime);
                }
                else
                {
                    float dTime = abs / SequenceAction.gracePeriod.y;
                    CheckInputState(out state, dTime);
                }
                return true;

                void CheckInputState(out SequenceActionState sequenceActionState, float dTime)
                {
                    if (dTime <= SequenceAction.PerfectTimePercent)
                    {
                        sequenceActionState = SequenceActionState.Perfect;
                        
                    } else if (dTime <= SequenceAction.GoodTimePercent)
                    {
                        sequenceActionState = SequenceActionState.Good;
                    }
                    else
                    {
                        sequenceActionState = SequenceActionState.Bad;
                    }
                }
            }

            public void SetState(SequenceActionState newState)
            {
                state = newState;
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
            
            GameManager.Instance.BeginLevel();

            if (_levelData.IsTutorial)
            {
                TutorialManager tutorialManager = gameObject.AddComponent<TutorialManager>();
                tutorialManager.Setup(_levelData);
                Destroy(this);
                return;
            }

            StartCoroutine(ClearInput());
        }

        private IEnumerator ClearInput()
        {
            yield return new WaitForSeconds(1);
            _lastInput = null;
        }

        public void OnBeat()
        {
            //Debug.Log("Beat");
            if (_currentSequence == null) return;
            UIManager.Instance.ClearDisplay();
            UIManager.Instance.ClearNextClotheDisplay();
            if (_currentSequence.DoBeat().HasActionOnBeat(out SequenceData.SequenceAction actionOnBeat))
            {
                Debug.Log("Sequence Has Action on beat");
                // UIManager.Instance.ChangeIconPosition(0, actionOnBeat);
                _lastSequenceAction = new SequenceActionTimed(actionOnBeat, Time.time);
                HandleInput();
            }

            for (int i = _peakAmount; i >= 0; i--)
            {
                if (_currentSequence.PeakBeat(i, out SequenceData.SequenceAction peakedAction))
                {
                    UIManager.Instance.ChangeClothIcon(peakedAction, (int)peakedAction.ClotheColor);
                    UIManager.Instance.ChangeIconPosition(i, peakedAction);
                }
            }
        }

        public void LoadNextSequence()
        {
            //TODO: increment _sequenceIndex where needed
            Debug.Log("LoadNextSequence");
            // // TEMP =========
            // if (_currentSequence != null) return;
            // // ==============
            _currentSequence = Sequence.FromSequenceData(_levelData[_sequenceIndex]);
        }

        public IEnumerator WaitForInput(float delay)
        {
            _waitingForInput = true;
            yield return new WaitForSeconds(delay);
            _waitingForInput = false;
            if (_lastSequenceAction is { state: SequenceActionState.None })
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
            if (_lastSequenceAction is null || _lastSequenceAction.state != SequenceActionState.None)
            {
                Debug.Log("_lastSequenceAction was null or already completed");
                return;
            }
            
            if (_lastInput is null)
            {
                Debug.Log("_lastInput was null");
                if (inputWaited)
                {
                    OnFail();
                }

                StartCoroutine(WaitForInput(_lastSequenceAction.SequenceAction.gracePeriod.y));
                return;
            }
            
            if (!_lastSequenceAction.WasInputInTimeFrame(_lastInput.Timestamp, out SequenceActionState actionState))
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
                            OnSuccess(actionState);    
                            return;
                        }
                        Debug.Log($"Clothe type was not the same {_lastInput.ClotheType} != {_lastSequenceAction.SequenceAction.ClotheType}");
                        break;
                    case SequenceConfig.ActionType.Scan:
                        if (_lastInput.ClotheType == _lastSequenceAction.SequenceAction.ClotheType &&
                            _lastInput.ClotheColor == _lastSequenceAction.SequenceAction.ClotheColor)
                        {
                            OnSuccess(actionState);
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

        private void OnSuccess(SequenceActionState actionState)
        {
            FinishSequenceAction(actionState);
        }

        private void OnFail()
        {
            FinishSequenceAction();
        }
        
        private void FinishSequenceAction(SequenceActionState actionState = SequenceActionState.Failed)
        {
            if (_lastSequenceAction?.state != SequenceActionState.None || _currentSequence is null) return;
            
            _lastSequenceAction.SetState(actionState);
            Debug.Log($"Finished Sequence Action with state: {actionState}");
            _lastInput = null;
            
            ScoreManager.SequenceActionFinished(_lastSequenceAction);
            
            if (_currentSequence.IsLastSequence(_lastSequenceAction))
            {
                _sequenceIndex++;
            }
        }

        private void OnEnable()
        {
            InputManager.LastInputChanged += InputManagerOnLastInputChanged;
            //WwiseManager.BeatEvent += OnBeat;
            GameManager.Instance.OnBeatUnityEvent += OnBeat;
            GameManager.Instance.SequenceEvent += LoadNextSequence;
        }
        
        private void OnDisable()
        {
            InputManager.LastInputChanged -= InputManagerOnLastInputChanged;
            //WwiseManager.BeatEvent -= OnBeat;
            GameManager.Instance.OnBeatUnityEvent -= OnBeat;
            GameManager.Instance.SequenceEvent -= LoadNextSequence;
        }
    }
}