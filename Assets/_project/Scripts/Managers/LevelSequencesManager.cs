using System;
using System.Collections;
using _project.ScriptableObjects.Scripts;
using _project.Scripts.Extensions;
using _project.Scripts.Managers.Inputs;
using JetBrains.Annotations;
using UnityEngine;

namespace _project.Scripts.Managers
{
    public class LevelSequencesManager : MonoBehaviour
    {
        public static event Action<Sequence> CurrentSequenceChanged;
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
                // Debug.LogWarning($"TIMEDIFF: {timeDiff}");
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
                // Debug.LogWarning($"Input was in frame with diff: {timeDiff} and state {state}");
                return true;

                void CheckInputState(out SequenceActionState sequenceActionState, float dTime)
                {
                    if (dTime <= ScoreManager.Instance.Data.PerfectTimePercent)
                    {
                        sequenceActionState = SequenceActionState.Perfect;
                        
                    } else if (dTime <= ScoreManager.Instance.Data.GoodTimePercent)
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
        [CanBeNull] private SequenceActionTimed _currentSequenceAction;

        private InputTypeLink _lastInput;
        private float _lastInputTime;
        private bool _waitingForInput;

        private Sequence CurrentSequence
        {
            get => _currentSequence;
            set
            {
                _currentSequence = value;
                CurrentSequenceChanged?.Invoke(_currentSequence);
            }
        }

        private void Start()
        {
            _levelData = LevelManager.Instance.CurrentLevelData;

            UIManager.Instance.levelData = _levelData;
            UIManager.Instance.ChangeClientSprite(false);
            
            //GameManager.Instance.BeginLevel();
            AkUnitySoundEngine.PostEvent(
                "StopMusic",
                gameObject
            );

            if (_levelData.IsTutorial)
            {
                // TutorialManager tutorialManager = gameObject.AddComponent<TutorialManager>();
                // tutorialManager.Setup(_levelData);
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
            if (CurrentSequence == null) return;
            UIManager.Instance.ClearDisplay();
            // UIManager.Instance.ClearNextClotheDisplay();
            if (CurrentSequence.DoBeat().HasActionOnBeat(out SequenceData.SequenceAction actionOnBeat))
            {
                Debug.Log("Sequence Has Action on beat");
                // UIManager.Instance.ChangeIconPosition(0, actionOnBeat);
                _currentSequenceAction = new SequenceActionTimed(actionOnBeat, Time.time);
                if (CurrentSequence.GetGameActionPack(out SequenceExtensions.GameActionPack pack,
                        (a, b) => false))
                {
                    GameAction resultNextGameAction = pack.nextNextGameAction;
                    GameAction resultCurrentGameAction = pack.nextGameAction;
                    UIManager.Instance.UpdateNextClotheIcon(resultCurrentGameAction, resultNextGameAction);
                    // if (!resultNextGameAction.IsSameClothe(resultCurrentGameAction))
                    // {
                    // }
                }
                HandleInput();
            }

            for (int i = _peakAmount; i >= 0; i--)
            {
                if (CurrentSequence.PeakBeat(i, out SequenceData.SequenceAction peakedAction))
                {
                    UIManager.Instance.ChangeIconPosition(i, peakedAction);
                }
            }
            // if (CurrentSequence.GetNextGameAction(out GameAction gameAction))
            // {
            //     UIManager.Instance.ChangeClothIcon(gameAction, (int)gameAction.ClotheColor);
            // }
        }

        public void LoadNextSequence()
        {
            //TODO: increment _sequenceIndex where needed
            Debug.Log("LoadNextSequence");
            // // TEMP =========
            // if (_currentSequence != null) return;
            // // ==============
            CurrentSequence = Sequence.FromSequenceData(_levelData[_sequenceIndex]);
            if (CurrentSequence.GetGameActionPack(out SequenceExtensions.GameActionPack pack,
                    (a, b) => false))
            {
                if (_sequenceIndex == 0)
                {
                    UIManager.Instance.UpdateNextClotheIcon(pack.currentGameAction, pack.nextGameAction);
                }
                else
                {
                    UIManager.Instance.UpdateNextClotheIcon(null, pack.currentGameAction);
                }
                // UIManager.Instance.InitNextClotheIcon(pack.currentGameAction, pack.nextGameAction);
            }
        }

        public IEnumerator WaitForInput(float delay)
        {
            _waitingForInput = true;
            yield return new WaitForSeconds(delay);
            _waitingForInput = false;
            if (_currentSequenceAction is { state: SequenceActionState.None })
            {
                // Debug.Log("Failing after waiting....");
                OnFail();
            }
        }
        
        private void InputManagerOnLastInputChanged(InputTypeLink obj)
        {
            if (_lastInput is not null) return;
            // Debug.LogError($"ReceivedInput of action: {obj.ActionType}");
            _lastInput = obj;
            if (_waitingForInput)
            {
                // Debug.Log("We were waiting for input !");
                // Blablabla
                HandleInput(true);
                return;
            }
            // Debug.Log("Not waiting for input...");
        }

        private void HandleInput(bool inputWaited = false)
        {
            // Debug.Log("=== HANDLEINPUT ===");
            if (_currentSequenceAction is null || _currentSequenceAction.state != SequenceActionState.None)
            {
                // Debug.Log("_lastSequenceAction was null or already completed");
                return;
            }
            
            if (_lastInput is null)
            {
                // Debug.Log("_lastInput was null");
                if (inputWaited)
                {
                    OnFail();
                }

                StartCoroutine(WaitForInput(_currentSequenceAction.SequenceAction.gracePeriod.y));
                return;
            }
            
            if (!_currentSequenceAction.WasInputInTimeFrame(_lastInput.Timestamp, out SequenceActionState actionState))
            {
                // Debug.Log($"Input was out of out of time: {_currentSequenceAction.Timestamp - _lastInput.Timestamp}");
                OnFail();
                return;
            }

            if (_currentSequenceAction.SequenceAction.ActionType == _lastInput.ActionType)
            {
                // Debug.Log($"Actions were of same type: {_currentSequenceAction.SequenceAction.ActionType}");
                switch (_lastInput.ActionType)
                {
                    case SequenceConfig.ActionType.Pickup:
                    case SequenceConfig.ActionType.Drop:
                        if (_lastInput.ClotheType == _currentSequenceAction.SequenceAction.ClotheType)
                        {
                            OnSuccess(actionState);    
                            return;
                        }
                        // Debug.Log($"Clothe type was not the same {_lastInput.ClotheType} != {_currentSequenceAction.SequenceAction.ClotheType}");
                        break;
                    case SequenceConfig.ActionType.Scan:
                        if (_lastInput.ClotheType == _currentSequenceAction.SequenceAction.ClotheType &&
                            _lastInput.ClotheColor == _currentSequenceAction.SequenceAction.ClotheColor)
                        {
                            OnSuccess(actionState);
                            return;
                        }
                        // Debug.Log($"Clothe type was not the same {_lastInput.ClotheType} != {_currentSequenceAction.SequenceAction.ClotheType} or Color was not same {_lastInput.ClotheColor} !=  {_currentSequenceAction.SequenceAction.ClotheColor}");
                        break;
                    default:
                        throw new ArgumentOutOfRangeException();
                }
            }
            else
            {
                // Debug.LogWarning($"Actions weren't of same type: (Input){_lastInput.ActionType} != (SequenceAction){_currentSequenceAction.SequenceAction.ActionType}");
            }

            if (inputWaited)
            {
                OnFail();
            } else {
                // Debug.Log("Waiting for input...");
                StartCoroutine(WaitForInput(_currentSequenceAction.SequenceAction.gracePeriod.y));
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
            if (_currentSequenceAction?.state != SequenceActionState.None || CurrentSequence is null) return;
            
            _currentSequenceAction.SetState(actionState);
            
            FeedbackManager.FeedbackTimingInputStatic(actionState);
            FeedbackManager.ChangeRippleColorStatic(Color.blue);

            Debug.Log($"Finished Sequence Action with state: {actionState}");
            // _lastInput = null;
            StartCoroutine(WaitThenReleaseInput(1f));
            
            ScoreManager.SequenceActionFinished(_currentSequenceAction);
            
            if (CurrentSequence.IsLastSequence(_currentSequenceAction))
            {
                _sequenceIndex++;
            }
        }

        private IEnumerator WaitThenReleaseInput(float waitTime)
        {
            yield return new WaitForSeconds(waitTime);
            _lastInput = null;
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