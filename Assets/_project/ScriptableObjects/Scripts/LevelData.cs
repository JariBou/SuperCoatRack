using System;
using System.Collections.Generic;
using _project.Scripts;
using GraphicsLabor.Scripts.Attributes.LaborerAttributes.DrawerAttributes;
using GraphicsLabor.Scripts.Attributes.LaborerAttributes.InspectedAttributes;
using GraphicsLabor.Scripts.Attributes.LaborerAttributes.ScriptableObjectAttributes;
using UnityEngine;

namespace _project.ScriptableObjects.Scripts
{
    [Editable, Manageable]
    public class LevelData : ScriptableObject
    {
        [HideIf(nameof(IsTutorial))]
        public List<SequenceData> sequences = new();
        public Sprite Icon;
        public bool IsTutorial;
        public int _songDurationInSeconds;
        [ShowIf(nameof(IsTutorial))]
        public TutorialData TutorialData;

        public SequenceData this[int levelName] => sequences[levelName];

        [HideIf(nameof(IsTutorial))]
        public AK.Wwise.Event MusicToPlayEvent;

        public Sprite Client;
        [HideIf(nameof(IsTutorial))] public Sprite HappyClient;
        
        [ShowMessage("If you want to set different grace periods for each level, here you go!")]
        public Vector2 PickupActionGracePeriod = new Vector2(.5f, .5f);
        public Vector2 DropActionGracePeriod = new Vector2(.5f, .5f);
        public Vector2 ScanActionGracePeriod = new Vector2(1f, 1f);
        public Vector2 BellActionGracePeriod = new Vector2(.5f, .5f);

        public Vector2 GetGracePeriodFor(SequenceConfig.ActionType sequenceActionActionType)
        {
            switch (sequenceActionActionType)
            {
                case SequenceConfig.ActionType.Pickup:
                    return PickupActionGracePeriod;
                case SequenceConfig.ActionType.Drop:
                    return DropActionGracePeriod;
                case SequenceConfig.ActionType.Scan:
                    return ScanActionGracePeriod;
                case SequenceConfig.ActionType.Bell:
                    return BellActionGracePeriod;
                case SequenceConfig.ActionType.Null:
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(sequenceActionActionType), sequenceActionActionType, null);
            }
            
            return Vector2.zero;
        }
    }
}