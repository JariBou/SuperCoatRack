using System;
using UnityEngine;

namespace _project.Scripts.Managers
{
    public class WwiseManager : MonoBehaviour
    {
        private AKRESULT result;

        [SerializeField] private AK.Wwise.Event playMusicEvent;
        public int NbOfBar { get; private set; }
        public static event Action<int> BeatEvent;

        private void Start()
        {
            AkUnitySoundEngine.PostEvent(
                playMusicEvent.Name,
                gameObject,
                (uint)AkCallbackType.AK_MusicSyncBeat | (uint)AkCallbackType.AK_EndOfEvent,
                OnBeatEvent,
                null
            );
        }

        private void OnBeatEvent(object in_cookie, AkCallbackType in_type, AkCallbackInfo in_info)
        {
            if (in_type == AkCallbackType.AK_MusicSyncBeat)
            {
                NbOfBar++;
                // Debug.Log("Beat détecté !");
                BeatEvent?.Invoke(NbOfBar);
            }
            if (in_type == AkCallbackType.AK_EndOfEvent)
            {
                Debug.Log("End of beat");
            }
        }
    }
}
