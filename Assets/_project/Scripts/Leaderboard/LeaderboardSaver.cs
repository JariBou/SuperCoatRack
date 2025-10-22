using System;
using System.Collections.Generic;
using System.Linq;
using GraphicsLabor.Scripts.Attributes.LaborerAttributes.InspectedAttributes;
using UnityEngine;

namespace _project.Scripts.Leaderboard
{
    public class LeaderboardSaver : MonoBehaviour
    {
        [SerializeField] private LeaderboardJsonWrapper _leaderboardJsonWrapperSaved;
        [SerializeField] private LeaderboardJsonWrapper _leaderboardJsonWrapperLoaded;

        private void Start()
        {
            SaveLeaderboardToFile();
            LoadLeaderboardFromFile();
        }

        public void SaveLeaderboardToFile()
        {
            Dictionary<string, List<LeaderboardEntry>> leaderboard = Leaderboard.GetLeaderboard();
            leaderboard.Add("test1", new List<LeaderboardEntry>(){new LeaderboardEntry("HahA", 56)});
            leaderboard.Add("test2", new List<LeaderboardEntry>(){new LeaderboardEntry("aaaaa", 64)});
            
            LeaderboardJsonWrapper jsonWrapper = new LeaderboardJsonWrapper(leaderboard);
            _leaderboardJsonWrapperSaved = jsonWrapper;
            string json = JsonUtility.ToJson(jsonWrapper, true);

            Debug.Log(json);
            if (!System.IO.Directory.Exists(Application.persistentDataPath + "/playerData"))
            {
                System.IO.Directory.CreateDirectory(Application.persistentDataPath + "/playerData");
            }
            System.IO.File.WriteAllText(Application.persistentDataPath + "/playerData/leaderboard.json", json);
            Debug.Log(Application.persistentDataPath + "/playerData/leaderboard.json");
        }

        public void LoadLeaderboardFromFile()
        {
            if (!System.IO.File.Exists(Application.persistentDataPath + "/playerData/leaderboard.json"))
            {
                return;
            }

            string json = System.IO.File.ReadAllText(Application.persistentDataPath + "/playerData/leaderboard.json");
            LeaderboardJsonWrapper leaderboardJsonWrapper = JsonUtility.FromJson<LeaderboardJsonWrapper>(json);
            _leaderboardJsonWrapperLoaded = leaderboardJsonWrapper;
            
            Leaderboard.LoadLeaderboard(leaderboardJsonWrapper.GetLeaderboardEntry());
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
}