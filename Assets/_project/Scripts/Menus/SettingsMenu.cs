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

        private void Start()
        {
            float volumeValue = PlayerPrefs.GetFloat("Volume", 1f);
            _volumeSlider.value = volumeValue;
            UpdateMixerVolume(volumeValue);
        }

        public void OnSliderValueChanged(float value)
        {
            PlayerPrefs.SetFloat("Volume", value);
            UpdateMixerVolume(value);
        }

        private void UpdateMixerVolume(float linearValue)
        {
            float decibelMixerValue = Mathf.Log10(linearValue) * 20f;
            _audioMixer.SetFloat("MasterVolume", decibelMixerValue);
        }
    }
}