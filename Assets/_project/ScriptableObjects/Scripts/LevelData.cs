using System.Collections.Generic;
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
    }
}