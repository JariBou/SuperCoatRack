using UnityEngine;

namespace _project.Scripts.LeaderboardSripts
{
    public class LeaderboardSaver : MonoBehaviour
    {

        private void Start()
        {
            Leaderboard.LoadLeaderboardFromFile();
        }

        private void OnDisable()
        {
            Leaderboard.SaveLeaderboardToFile();
        }

        // public void SaveLeaderboardToFile()
        // {
        //     Dictionary<string, List<LeaderboardEntry>> leaderboard = Leaderboard.GetLeaderboard();
        //     leaderboard.Add("test1", new List<LeaderboardEntry>(){new LeaderboardEntry("HahA", 56)});
        //     leaderboard.Add("test2", new List<LeaderboardEntry>(){new LeaderboardEntry("aaaaa", 64)});
        //     
        //     LeaderboardJsonWrapper jsonWrapper = new LeaderboardJsonWrapper(leaderboard);
        //     _leaderboardJsonWrapperSaved = jsonWrapper;
        //     string json = JsonUtility.ToJson(jsonWrapper, true);
        //
        //     Debug.Log(json);
        //     if (!System.IO.Directory.Exists(Application.persistentDataPath + "/playerData"))
        //     {
        //         System.IO.Directory.CreateDirectory(Application.persistentDataPath + "/playerData");
        //     }
        //     System.IO.File.WriteAllText(Application.persistentDataPath + "/playerData/leaderboard.json", json);
        //     Debug.Log(Application.persistentDataPath + "/playerData/leaderboard.json");
        // }

        // public void LoadLeaderboardFromFile()
        // {
        //     if (!System.IO.File.Exists(Application.persistentDataPath + "/playerData/leaderboard.json"))
        //     {
        //         return;
        //     }
        //
        //     string json = System.IO.File.ReadAllText(Application.persistentDataPath + "/playerData/leaderboard.json");
        //     LeaderboardJsonWrapper leaderboardJsonWrapper = JsonUtility.FromJson<LeaderboardJsonWrapper>(json);
        //     _leaderboardJsonWrapperLoaded = leaderboardJsonWrapper;
        //     
        //     Leaderboard.LoadLeaderboard(leaderboardJsonWrapper.GetLeaderboardEntry());
        // }

       
        
    }
}