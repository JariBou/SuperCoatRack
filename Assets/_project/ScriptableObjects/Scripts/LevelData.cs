using System.Collections.Generic;
using GraphicsLabor.Scripts.Attributes.LaborerAttributes.ScriptableObjectAttributes;
using UnityEngine;

namespace _project.ScriptableObjects.Scripts
{
    [Editable, Manageable]
    public class LevelData : ScriptableObject
    {
        public List<SequenceData> sequences = new();
        public Sprite Icon;

        public SequenceData this[int levelName] => sequences[levelName];

        public AK.Wwise.Event MusicToPlayEvent;
    }
}