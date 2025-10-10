using System;
using JetBrains.Annotations;
using UnityEngine;
using UnityEngine.InputSystem;

namespace _project.Scripts.Managers.Inputs
{
    public class InputManager : MonoBehaviour
    {
        public static event Action<InputTypeLink> LastInputChanged; 
        [CanBeNull] private InputTypeLink _lastInput;
        

        public InputTypeLink LastInput
        {
            get => _lastInput;
            private set
            {
                _lastInput = value;
                LastInputChanged?.Invoke(_lastInput);
            }
        }

        public void OnCoatInput(InputAction.CallbackContext ctx)
        {
            if (ctx.performed)
            {
                LastInput = new InputTypeLink(SequenceConfig.ActionType.Drop, SequenceConfig.ClotheType.Coat, Time.time);
            }
            else
            {
                LastInput = new InputTypeLink(SequenceConfig.ActionType.Pickup, SequenceConfig.ClotheType.Coat, Time.time);
            }
        }

        public void OnHatInput(InputAction.CallbackContext ctx)
        {
            if (ctx.performed)
            {
                LastInput = new InputTypeLink(SequenceConfig.ActionType.Drop, SequenceConfig.ClotheType.Hat, Time.time);
            }
            else
            {
                LastInput = new InputTypeLink(SequenceConfig.ActionType.Pickup, SequenceConfig.ClotheType.Hat, Time.time);
            }
        }
        
        public void OnShoeInput(InputAction.CallbackContext ctx)
        {
            if (ctx.performed)
            {
                LastInput = new InputTypeLink(SequenceConfig.ActionType.Drop, SequenceConfig.ClotheType.Shoe, Time.time);
            }
            else
            {
                LastInput = new InputTypeLink(SequenceConfig.ActionType.Pickup, SequenceConfig.ClotheType.Shoe, Time.time);
            }
        }
        
        public void OnScan()
        {
            SequenceConfig.ClotheType scannedClotheType = SequenceConfig.ClotheType.Coat;
            SequenceConfig.ClotheColor scannedClotheColor = SequenceConfig.ClotheColor.Blue;
            LastInput = new InputTypeLink(SequenceConfig.ActionType.Scan, scannedClotheType, scannedClotheColor, Time.time);
        }
    }

    [Serializable]
    public class InputTypeLink
    {
        public SequenceConfig.ActionType ActionType;
        public SequenceConfig.ClotheType ClotheType;
        public SequenceConfig.ClotheColor ClotheColor;

        public float Timestamp;

        public InputTypeLink(SequenceConfig.ActionType actionType, SequenceConfig.ClotheType clotheType, float timestamp)
        {
            ActionType = actionType;
            ClotheType = clotheType;
            Timestamp = timestamp;
        }

        public InputTypeLink(SequenceConfig.ActionType actionType, SequenceConfig.ClotheType clotheType, SequenceConfig.ClotheColor clotheColor, float time)
        {
            ActionType = actionType;
            ClotheType = clotheType;
            ClotheColor = clotheColor;
            Timestamp = time;
        }
    }
}