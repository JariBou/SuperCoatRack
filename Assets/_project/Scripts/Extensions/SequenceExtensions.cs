using System;

namespace _project.Scripts.Extensions
{
    public static class SequenceExtensions
    {
        public class GameActionPack
        {
            public GameAction previousGameAction;
            public GameAction currentGameAction;
            public GameAction nextGameAction;
        }

        public static bool GetGameActionPack(this Sequence sequence, out GameActionPack result)
        {
            return sequence.GetGameActionPack(out result, DefaultAreActionsEqualComparer);
        }

        private static bool DefaultAreActionsEqualComparer(GameAction a, GameAction b)
        {
            return a.ActionType == b.ActionType;
        }

        public static bool GetGameActionPack(this Sequence sequence, out GameActionPack result, Func<GameAction, GameAction, bool> areActionsEqualComparer)
        {
            result = new GameActionPack();
            bool found = false;
            for (int i = 1; i <= sequence.BeatCounter; i++)
            {
                if (sequence.PeakBeat(-i, out GameAction previousGameAction))
                {
                    result.previousGameAction = previousGameAction;
                    found = true;
                }
            }

            for (int i = 0; i < sequence.MaxBeatCounter - sequence.BeatCounter; i++)
            {
                if (sequence.PeakBeat(i, out GameAction gameAction))
                {
                    if (result.currentGameAction is null)
                    {
                        result.currentGameAction = gameAction;
                        found = true;
                    }
                    else if (!areActionsEqualComparer.Invoke(result.currentGameAction, gameAction))
                    {
                        result.nextGameAction = gameAction;
                        found = true;
                        break;
                    }
                }
            }
            
            return found;
        }

        public static bool GetNextGameAction(this Sequence sequence, out GameAction result)
        {
            result = null;
            for (int i = 0; i < sequence.MaxBeatCounter - sequence.BeatCounter; i++)
            {
                if (sequence.PeakBeat(i, out GameAction gameAction))
                {
                    result = gameAction;
                    return true;
                }
            }
            return false;
        }
    }
}