using System;
using NaughtyAttributes;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace _project.Scripts.Menus.LeaderboardEntry
{
    public class LetterSelectionDisplay : MonoBehaviour
    {
        [SerializeField] private Image _upImage;
        [SerializeField] private Image _downImage;
        [SerializeField] private TMP_Text _letterText;

        private bool _selected;
        private Color _selectedTextColor;
        private Color _baseTextColor;
        private int _minTextByte;
        private int _maxTextByte;
        public bool IsSelected => _selected;

        private void Start()
        {
            _baseTextColor = _letterText.color;

            _minTextByte = char.ConvertToUtf32("A", 0);
            _maxTextByte = char.ConvertToUtf32("Z", 0);
        }

        public void NextLetter()
        {
            string convertFromUtf32 = char.ConvertFromUtf32(Mathf.Clamp(char.ConvertToUtf32(_letterText.text, 0) + 1, _minTextByte,
                _maxTextByte));
            _letterText.text = convertFromUtf32;
        }

        public void PreviousLetter()
        {
            string convertFromUtf32 = char.ConvertFromUtf32(Mathf.Clamp(char.ConvertToUtf32(_letterText.text, 0) - 1, _minTextByte,
                _maxTextByte));
            _letterText.text = convertFromUtf32;
        }

        public void SetSelected(bool b)
        {
            _selected = b;
            _letterText.color = _selected ? _selectedTextColor : _baseTextColor;
        }

        public void Config(Color selectedColor)
        {
            _selectedTextColor = selectedColor;
        }
    }
}