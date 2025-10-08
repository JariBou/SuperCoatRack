using System.Collections.Generic;
using UnityEngine;

namespace _project.ScriptableObjects.Scripts
{
    [CreateAssetMenu(fileName = "LevelsData", menuName = "ScriptableObjects/LevelsData")]
    public class LevelsData : ScriptableObject
    {
        public List<Sprite> selectionSprites; // TEMP

        public Sprite this[int selectedLevelIndex] => selectionSprites[selectedLevelIndex];
    }
}