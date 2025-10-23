using System;
using System.Collections.Generic;
using _project.Scripts.LeaderboardSripts;
using _project.Scripts.Managers;
using UnityEngine;

namespace _project.Scripts.Menus.LeaderboardEntry
{
    public class LeaderboardDisplay : MonoBehaviour
    {
        [SerializeField] private GameObject _leaderboardEntryPrefab;
        [SerializeField] private GameObject _leaderboardEntryContainer;
        [SerializeField] private int _displayedEntriesCount = 4;
        private string _selectedName;
        private LeaderboardSripts.LeaderboardEntry _leaderboardEntry;

        public void ShowAndAdd(string playerEnteredName)
        {
            _selectedName = playerEnteredName;
            _leaderboardEntry = new LeaderboardSripts.LeaderboardEntry(playerEnteredName, 500000);
            Leaderboard.AddLeaderboardEntry("test", _leaderboardEntry);
            // Leaderboard.Leaderboard.AddLeaderboardEntry(LevelManager.CurrentLevel.LevelName, new Leaderboard.LeaderboardEntry(playerEnteredName, ScoreManager.GetScore()));
            Show();
        }

        public void Show(string levelName = null)
        {
            foreach (Transform o in _leaderboardEntryContainer.transform)
            {
                Destroy(o.gameObject);
            }
            LoadLeaderboard(levelName);
            gameObject.SetActive(true);
        }

        private void Start()
        {
            Leaderboard.AddLeaderboardEntry("test", new LeaderboardSripts.LeaderboardEntry("TOM", 5000));
            // Leaderboard.Leaderboard.AddLeaderboardEntry(LevelManager.CurrentLevel.LevelName, new Leaderboard.LeaderboardEntry("TOM", 5000));
            gameObject.SetActive(false);
        }

        private void LoadLeaderboard(string levelName = null)
        {
            // List<LeaderboardSripts.LeaderboardEntry> leaderboardForMap = Leaderboard.GetLeaderboardForMap("test");
            string leaderboardLevelName = levelName ?? LevelManager.CurrentLevel.LevelName;
            List<LeaderboardSripts.LeaderboardEntry> leaderboardForMap = LeaderboardSripts.Leaderboard.GetLeaderboardForMap(leaderboardLevelName);
            if (leaderboardForMap.Count == 0)
            {
                LeaderboardEntryDisplay leaderboardEntryDisplay = Instantiate(_leaderboardEntryPrefab, _leaderboardEntryContainer.transform).GetComponent<LeaderboardEntryDisplay>();
                leaderboardEntryDisplay.Config("No entries for level", leaderboardLevelName);
                return;
            }
            for (int i = 0; i < Math.Min(_displayedEntriesCount, leaderboardForMap.Count); i++)
            {
                LeaderboardEntryDisplay leaderboardEntryDisplay = Instantiate(_leaderboardEntryPrefab, _leaderboardEntryContainer.transform).GetComponent<LeaderboardEntryDisplay>();
                leaderboardEntryDisplay.Config(i+1, leaderboardForMap[i], leaderboardForMap[i] == _leaderboardEntry);
            }
        }
    }
}