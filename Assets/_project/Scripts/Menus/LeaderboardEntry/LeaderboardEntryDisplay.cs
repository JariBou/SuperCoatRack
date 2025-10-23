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

        public void Config(string name, string score)
        {
            _name.text = name;
            _score.text = score;
        }
        
        public void Config(string name, int score)
        {
            Config(name, score.ToString());
        }

        public void Config(int i, LeaderboardSripts.LeaderboardEntry leaderboardFor, bool selectedName = false)
        {
            Config($"{i}.{leaderboardFor.Name}", leaderboardFor.Score);
            _name.color = selectedName ? _currentPlayerColor : _defaultColor;
            _score.color = selectedName ? _currentPlayerColor : _defaultColor;
        }
    }
}