using System;
using System.Collections;
using System.Collections.Generic;
using _project.Scripts;
using GraphicsLabor.Scripts.Attributes.LaborerAttributes.InspectedAttributes;
using GraphicsLabor.Scripts.Attributes.LaborerAttributes.ScriptableObjectAttributes;
using UnityEngine;
using UnityEngine.Serialization;

namespace _project.ScriptableObjects.Scripts
{
    [Manageable, Editable]
    public class TutorialData : ScriptableObject, IEnumerable<TutorialData.TutorialAction>
    {
        [FormerlySerializedAs("sequenceData")] public List<TutorialAction> TutorialSequence;
        
        [Serializable]
        public class TutorialAction : GameAction
        {
            // public int BeatDuration = -1;
            public string Text;
            public bool HasImage;
            [ShowIf(nameof(HasImage))]
            public Sprite ImageToDisplay;
        }

        public IEnumerator<TutorialAction> GetEnumerator()
        {
            return TutorialSequence.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
}