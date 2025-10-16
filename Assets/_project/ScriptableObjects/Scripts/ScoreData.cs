using GraphicsLabor.Scripts.Attributes.LaborerAttributes.ScriptableObjectAttributes;
using UnityEngine;

namespace _project.ScriptableObjects.Scripts
{
    [Manageable, Editable]
    public class ScoreData : ScriptableObject
    {
        public int PerfectRatingScore = 100;
        public int GoodRatingScore = 50;
        public int BadRatingScore = 25;
        
        [Range(0f, 1f)] public float PerfectTimePercent = .1f;
        [Range(0f, 1f)] public float GoodTimePercent = .6f;
    }
}