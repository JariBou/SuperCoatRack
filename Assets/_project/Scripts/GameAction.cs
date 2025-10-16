using System;

namespace _project.Scripts
{
    [Serializable]
    public class GameAction
    {
        public SequenceConfig.ActionType ActionType;
        public SequenceConfig.ClotheType ClotheType;
        public SequenceConfig.ClotheColor ClotheColor;

        public override string ToString()
        {
            return $"GameAction(ActionType:{ActionType} ; ClotheType:{ClotheType} ; ClotheColor{ClotheColor}";
        }
    }
}