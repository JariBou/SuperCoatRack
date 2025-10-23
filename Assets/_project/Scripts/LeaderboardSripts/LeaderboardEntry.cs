using System;

namespace _project.Scripts.LeaderboardSripts
{
    [Serializable]
    public class LeaderboardEntry
    {
        public string Name;
        public int Score;

        public LeaderboardEntry(string name, int score)
        {
            Name = name;
            Score = score;
        }
    }
}