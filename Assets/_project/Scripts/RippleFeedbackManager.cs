using System;
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
            return this;
        }
        
        // public static 
        
        
    }
}