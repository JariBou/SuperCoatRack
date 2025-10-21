using System.Collections.Generic;

namespace _project.Scripts.Leaderboard
{
    public class Leaderboard
    {
        private static Leaderboard _instance;
        
        private Dictionary<string,  List<LeaderboardEntry>> _leaderboard = new();

        private static Leaderboard Instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = new Leaderboard();
                }
                return _instance;
            }
            set => _instance = value;
        }

        public static List<LeaderboardEntry> GetLeaderboardForMap(string mapName)
        {
            Instance._leaderboard.TryGetValue(mapName, out List<LeaderboardEntry> leaderboard);
            return leaderboard;
        }

        public static Dictionary<string, List<LeaderboardEntry>> GetLeaderboard()
        {
            return Instance._leaderboard;
        }

        public static void LoadLeaderboard(Dictionary<string, List<LeaderboardEntry>> leaderboardEntry)
        {
            Instance._leaderboard =  leaderboardEntry;
        }
    }
}