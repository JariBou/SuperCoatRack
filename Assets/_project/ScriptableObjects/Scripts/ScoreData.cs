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
    }
}