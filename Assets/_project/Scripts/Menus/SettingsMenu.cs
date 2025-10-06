using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

namespace _project.Scripts.Menus
{
    public class SettingsMenu : MonoBehaviour
    {
        [SerializeField]
        private Slider _volumeSlider;

        [SerializeField] private AudioMixer _audioMixer;

        public void OnSliderValueChanged(float value)
        {
            Debug.Log("Volume changed to: " + value);
        }
    }
}