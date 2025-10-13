using System;
using System.Collections;
using System.Collections.Generic;
using _project.Scripts;
using GraphicsLabor.Scripts.Attributes.LaborerAttributes.ScriptableObjectAttributes;
using UnityEngine;

namespace _project.ScriptableObjects.Scripts
{
    [Manageable, Editable]
    public class SequenceData : ScriptableObject, IEnumerable<SequenceData.SequenceAction>
    {
        public List<SequenceAction> sequenceData;
        
        [Serializable]
        public class SequenceAction
        {
            public int BeatDelayToPrevious = 4;
            public Vector2 gracePeriod = new Vector2(.5f, .1f);
            [Range(0f, 1f)] public float PerfectTimePercent = .1f;
            [Range(0f, 1f)] public float GoodTimePercent = .6f;
            public SequenceConfig.ActionType ActionType;
            public SequenceConfig.ClotheType ClotheType;
            public SequenceConfig.ClotheColor ClotheColor;
        }

        public IEnumerator<SequenceAction> GetEnumerator()
        {
            return sequenceData.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
}