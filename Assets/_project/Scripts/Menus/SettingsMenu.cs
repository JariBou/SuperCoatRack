 using System.Collections.Generic;
 using System.Linq;
 using TMPro;
 using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.Localization.Settings;
using UnityEngine.UI;

namespace _project.Scripts.Menus
{
    public class SettingsMenu : MonoBehaviour
    {
        [SerializeField]
        private Slider _volumeSlider;

        [SerializeField] private AudioMixer _audioMixer;
        [SerializeField] private TMP_Dropdown _languageSelector;

        private void Start()
        {
            float volumeValue = PlayerPrefs.GetFloat("Volume", 1f);
            _volumeSlider.value = volumeValue;
            UpdateMixerVolume(volumeValue);
            
            
            List<string> options = LocalizationSettings.AvailableLocales.Locales.Select(localCode => localCode.LocaleName).ToList();
            _languageSelector.AddOptions(options);

            string selectedLocalName = PlayerPrefs.GetString("SelectedLang", LocalizationSettings.SelectedLocale.LocaleName);

            int localeIndex = LocalizationSettings.AvailableLocales.Locales.FindIndex(l => l.LocaleName == selectedLocalName);
            LocalizationSettings.SelectedLocale = LocalizationSettings.AvailableLocales.Locales[localeIndex];
            _languageSelector.value = localeIndex;
        }

        public void OnSliderValueChanged(float value)
        {
            PlayerPrefs.SetFloat("Volume", value);
            Debug.Log(value);
            UpdateMixerVolume(value);
        }

        private void UpdateMixerVolume(float linearValue)
        {
            float decibelMixerValue = Mathf.Log10(linearValue) * 20f;
            AkUnitySoundEngine.SetRTPCValue("MainBusVolume", linearValue*100f);
            _audioMixer.SetFloat("MasterVolume", decibelMixerValue);
        }

        public void OnLanguageIndexChanged(int languageIndex)
        {
            LocalizationSettings.SelectedLocale = LocalizationSettings.AvailableLocales.Locales[languageIndex];
            PlayerPrefs.SetString("SelectedLang", LocalizationSettings.SelectedLocale.LocaleName);
        }
    }
}