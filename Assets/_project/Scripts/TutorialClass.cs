using System.Collections.Generic;
using _project.ScriptableObjects.Scripts;

namespace _project.Scripts
{
    public class TutorialClass
    {
        private int _beatCounter;
        private List<TutorialData.TutorialAction> _tutorialDatas = new();

        private int _maxBeatAmount;
        
        private TutorialClass(TutorialData sequenceData)
        {
            _beatCounter--;
            _maxBeatAmount = 0;
            foreach (TutorialData.TutorialAction action in sequenceData)
            {
                _tutorialDatas.Add(action);
            }
            Initialize();
        }

        private void Initialize()
        {
            // foreach (TutorialData.TutorialAction action in _tutorialDatas)
            // {
            //     _maxBeatAmount += action.BeatDuration;
            // }
        }

        public TutorialClass DoBeat()
        {
            _beatCounter++;
            // Debug.Log($"Doing Beat {_beatCounter} of Sequence");
            return this;
        }

        public static TutorialClass FromTutorialData(TutorialData tutorial)
        {
            return new TutorialClass(tutorial);
        }
        

        public bool IsOver(int index)
        {
            return index >= _tutorialDatas.Count;
        }

        public TutorialData.TutorialAction this[int i] => _tutorialDatas[i];
    }
}