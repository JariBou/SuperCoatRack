using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace _project.Scripts.LeaderboardSripts
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

        public static List<LeaderboardEntry> GetLeaderboardForMap(string mapName, bool sorted = true)
        {
            if (!Instance._leaderboard.TryGetValue(mapName, out List<LeaderboardEntry> leaderboard))
            {
                return new List<LeaderboardEntry>();
            }

            if (sorted)
            {
                leaderboard.Sort((entry, leaderboardEntry) => entry.Score > leaderboardEntry.Score ? -1 : 1);
            }
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
        
        public static void SaveLeaderboardToFile()
        {
            Dictionary<string, List<LeaderboardEntry>> leaderboard = GetLeaderboard();
            
            LeaderboardJsonWrapper jsonWrapper = new LeaderboardJsonWrapper(leaderboard);
            string json = JsonUtility.ToJson(jsonWrapper, true);

            Debug.Log(json);
            if (!System.IO.Directory.Exists(Application.persistentDataPath + "/playerData"))
            {
                System.IO.Directory.CreateDirectory(Application.persistentDataPath + "/playerData");
            }
            System.IO.File.WriteAllText(Application.persistentDataPath + "/playerData/leaderboard.json", json);
            Debug.Log(Application.persistentDataPath + "/playerData/leaderboard.json");
        }
        
        public static void LoadLeaderboardFromFile()
        {
            if (!System.IO.File.Exists(Application.persistentDataPath + "/playerData/leaderboard.json"))
            {
                return;
            }

            string json = System.IO.File.ReadAllText(Application.persistentDataPath + "/playerData/leaderboard.json");
            LeaderboardJsonWrapper leaderboardJsonWrapper = JsonUtility.FromJson<LeaderboardJsonWrapper>(json);
            
            LoadLeaderboard(leaderboardJsonWrapper.GetLeaderboardEntry());
        }
        
        [Serializable]
        public class LeaderboardJsonWrapper
        {
            [SerializeField] private List<SerializedKeyValuePair<string, List<LeaderboardEntry>>> _leaderboard;
            

            public LeaderboardJsonWrapper(List<SerializedKeyValuePair<string, List<LeaderboardEntry>>> leaderboard)
            {
                _leaderboard = leaderboard;
            }
            
            public LeaderboardJsonWrapper(Dictionary<string, List<LeaderboardEntry>> leaderboard)
            {
                List<KeyValuePair<string, List<LeaderboardEntry>>> keyValuePairs = leaderboard.ToArray().ToList();
                _leaderboard = keyValuePairs.Select(kvp => new SerializedKeyValuePair<string, List<LeaderboardEntry>>(kvp.Key, kvp.Value)).ToList();
            }

            public Dictionary<string, List<LeaderboardEntry>> GetLeaderboardEntry()
            {
                return _leaderboard.ToDictionary(entry => entry.Key, entry => entry.Value);
            }
        }

        public static void AddLeaderboardEntry(string levelName, LeaderboardEntry leaderboardEntry, bool autoSave = true)
        {
            Instance.AddEntry(levelName, leaderboardEntry, autoSave);
        }

        private void AddEntry(string levelName, LeaderboardEntry leaderboardEntry, bool autoSave = true)
        {
            if (_leaderboard.TryGetValue(levelName, out List<LeaderboardEntry> leaderboard))
            {
                leaderboard.Add(leaderboardEntry);
            }
            else
            {
                _leaderboard.Add(levelName, new List<LeaderboardEntry>(){leaderboardEntry});
            }

            if (autoSave)
            {
                SaveLeaderboardToFile();
            }
        }

        public static int GetPositionOfScoreInMap(int getScore, string levelName)
        {
            List<LeaderboardEntry> leaderboardForMap = GetLeaderboardForMap(levelName, true);
            for (var i = 0; i < leaderboardForMap.Count; i++)
            {
                LeaderboardEntry entry = leaderboardForMap[i];
                if (entry.Score < getScore)
                {
                    return i+1;
                }
            }
            return leaderboardForMap.Count+1;
        }
    }
    
    
    [Serializable]
    public class SerializedKeyValuePair<TKey, TValue>
    {
        [SerializeField] public TKey Key;
        [SerializeField] public TValue Value;

        public SerializedKeyValuePair(TKey key, TValue value)
        {
            Key = key;
            Value = value;
        }
    }
}