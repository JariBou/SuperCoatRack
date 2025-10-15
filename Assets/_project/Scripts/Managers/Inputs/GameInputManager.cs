using System;
using JetBrains.Annotations;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

namespace _project.Scripts.Managers.Inputs
{
    [RequireComponent(typeof(PlayerInput))]
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

        private void Awake()
        {
            foreach (InputAction inputAction in GetComponent<PlayerInput>().currentActionMap.actions)
            {
                if (inputAction.name.StartsWith("Scan"))
                {
                    inputAction.performed += OnScan;
                }
            }
        }

        public void Restart(InputAction.CallbackContext ctx)
        {
            if (ctx.performed)
            {
                SceneManager.LoadScene(SceneManager.GetActiveScene().name);
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

        public void OnScan(InputAction.CallbackContext ctx)
        {
            if (ctx.performed)
            {
                string[] strings = ctx.action.name.Split("_");
                SequenceConfig.ClotheType clotheType = (SequenceConfig.ClotheType)Enum.Parse(typeof(SequenceConfig.ClotheType), strings[1]);
                SequenceConfig.ClotheColor clotheColor = (SequenceConfig.ClotheColor)Enum.Parse(typeof(SequenceConfig.ClotheColor), strings[2]);
                LastInput = new InputTypeLink(SequenceConfig.ActionType.Scan, clotheType, clotheColor, Time.time);
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