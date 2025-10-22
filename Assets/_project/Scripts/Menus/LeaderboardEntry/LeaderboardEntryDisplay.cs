using TMPro;
using UnityEngine;

namespace _project.Scripts.Menus.LeaderboardEntry
{
    public class LeaderboardEntryDisplay : MonoBehaviour
    {
        [SerializeField] private TMP_Text _name;
        [SerializeField] private TMP_Text _score;

        [SerializeField] private Color _currentPlayerColor = Color.yellow;
        [SerializeField] private Color _defaultColor = Color.white;

        private void Config(string name, int score)
        {
            _name.text = name;
            _score.text = score.ToString();
        }

        public void Config(int i, Leaderboard.LeaderboardEntry leaderboardFor, bool selectedName)
        {
            Config($"{i}.{leaderboardFor.Name}", leaderboardFor.Score);
            _name.color = selectedName ? _currentPlayerColor : _defaultColor;
            _score.color = selectedName ? _currentPlayerColor : _defaultColor;
        }
    }
}