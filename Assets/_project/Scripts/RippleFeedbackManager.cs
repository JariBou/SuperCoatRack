using UnityEngine;

namespace _project.Scripts
{
    public class RippleFeedbackManager : MonoBehaviour
    {
        private static RippleFeedbackManager _instance;
        
        [SerializeField]
        private ParticleSystem _particleSystem;
        

        private void Awake()
        {
            if (_instance is not null)
            {
                Destroy(gameObject);
                return;
            }
            _instance = this;
        }

        private RippleFeedbackManager ChangeColor(Color color)
        {
            ParticleSystem.MainModule particleSystemMain = _particleSystem.main;
            particleSystemMain.startColor = color;
            _particleSystem.GetComponent<Renderer>().material.color = color;
            return this;
        }

        public void Play()
        {
            _particleSystem.Play();
        }
        
        public static void PlayStatic()
        {
            _instance.Play();
        }
        
        public void Stop()
        {
            _particleSystem.Stop();
        }
        
        public static void StopStatic()
        {
            _instance.Stop();
        }

        public static RippleFeedbackManager ChangeColorStatic(Color color)
        {
            return _instance.ChangeColor(color);
        }
        
        
    }
}