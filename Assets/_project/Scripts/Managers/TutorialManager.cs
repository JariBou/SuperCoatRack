using System;
using _project.ScriptableObjects.Scripts;
using _project.Scripts.Managers.Inputs;
using UnityEngine;

namespace _project.Scripts.Managers
{
    public class TutorialManager : MonoBehaviour
    {
        private LevelData _levelData;
        private TutorialClass _tutorialData;
        private TutorialData.TutorialAction _currentaction;
        private InputTypeLink _lastInput;
        private int _tutrialIndex;

        public void Setup(LevelData levelData)
        {
            _levelData = levelData;
            _tutorialData = TutorialClass.FromTutorialData(levelData.TutorialData);
            _tutrialIndex = 0;
            UpdateDisplay();
        }
        
        private void InputManagerOnLastInputChanged(InputTypeLink obj)
        {
            _lastInput = obj;
            if (_currentaction.ActionType == SequenceConfig.ActionType.Null)
            {
                _tutrialIndex++;
                UpdateDisplay();
            }
            
            if (_currentaction.ActionType == _lastInput.ActionType)
            {
                Debug.Log($"Actions were of same type: {_currentaction.ActionType}");
                switch (_lastInput.ActionType)
                {
                    case SequenceConfig.ActionType.Pickup:
                    case SequenceConfig.ActionType.Drop:
                        if (_lastInput.ClotheType == _currentaction.ClotheType)
                        {
                            _tutrialIndex++;  
                            UpdateDisplay();
                            return;
                        }
                        Debug.Log($"Clothe type was not the same {_lastInput.ClotheType} != {_currentaction.ClotheType}");
                        break;
                    case SequenceConfig.ActionType.Scan:
                        if (_lastInput.ClotheType == _currentaction.ClotheType &&
                            _lastInput.ClotheColor == _currentaction.ClotheColor)
                        {
                            _tutrialIndex++;
                            UpdateDisplay();
                            return;
                        }
                        Debug.Log($"Clothe type was not the same {_lastInput.ClotheType} != {_currentaction.ClotheType} or Color was not same {_lastInput.ClotheColor} !=  {_currentaction.ClotheColor}");
                        break;
                    case SequenceConfig.ActionType.Bell:
                    case SequenceConfig.ActionType.Null:
                        break;
                    default:
                        throw new ArgumentOutOfRangeException();
                }
            }
        }

        private void UpdateDisplay()
        {
            if (_tutorialData.IsOver(_tutrialIndex))
            {
                GameManager.Instance.GoToMenu();
            }
            else
            {
                _currentaction = _tutorialData[_tutrialIndex];
                UIManager.Instance.UpdateTutorialDisplay(_currentaction);
            }
            
            
        }
        
        private void OnEnable()
        {
            InputManager.LastInputChanged += InputManagerOnLastInputChanged;
            // GameManager.Instance.onBeatUnityEvent += OnBeat;
        }
        

        private void OnDisable()
        {
            InputManager.LastInputChanged -= InputManagerOnLastInputChanged;
            // GameManager.Instance.onBeatUnityEvent -= OnBeat;
        }
    }
}