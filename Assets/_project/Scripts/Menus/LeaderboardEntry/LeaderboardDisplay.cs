using System;
using System.Collections.Generic;
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
        private Leaderboard.LeaderboardEntry _leaderboardEntry;

        public void Show(string playerEnteredName)
        {
            _selectedName = playerEnteredName;
            _leaderboardEntry = new Leaderboard.LeaderboardEntry(playerEnteredName, 500000);
            Leaderboard.Leaderboard.AddLeaderboardEntry("test", _leaderboardEntry);
            // Leaderboard.Leaderboard.AddLeaderboardEntry(LevelManager.CurrentLevel.LevelName, new Leaderboard.LeaderboardEntry(playerEnteredName, ScoreManager.GetScore()));
            LoadLeaderboard();
            gameObject.SetActive(true);
        }

        private void Start()
        {
            Leaderboard.Leaderboard.AddLeaderboardEntry("test", new Leaderboard.LeaderboardEntry("TOM", 5000));
            // Leaderboard.Leaderboard.AddLeaderboardEntry(LevelManager.CurrentLevel.LevelName, new Leaderboard.LeaderboardEntry("TOM", 5000));
            gameObject.SetActive(false);
        }

        private void LoadLeaderboard()
        {
            List<Leaderboard.LeaderboardEntry> leaderboardForMap = Leaderboard.Leaderboard.GetLeaderboardForMap("test");
            // List<Leaderboard.LeaderboardEntry> leaderboardForMap = Leaderboard.Leaderboard.GetLeaderboardForMap(LevelManager.CurrentLevel.LevelName);
            for (int i = 0; i < Math.Min(_displayedEntriesCount, leaderboardForMap.Count); i++)
            {
                LeaderboardEntryDisplay leaderboardEntryDisplay = Instantiate(_leaderboardEntryPrefab, _leaderboardEntryContainer.transform).GetComponent<LeaderboardEntryDisplay>();
                leaderboardEntryDisplay.Config(i+1, leaderboardForMap[i], leaderboardForMap[i] == _leaderboardEntry);
            }
        }
    }
}