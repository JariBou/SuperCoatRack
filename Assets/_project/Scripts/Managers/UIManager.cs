using System;
using _project.ScriptableObjects.Scripts;
using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace _project.Scripts.Managers
{
    public class UIManager : MonoBehaviour
    {
        private static UIManager _instance;
        public static UIManager Instance => _instance;

    
        #region BeatIcons
        public Sprite[] iconSprites;
        public Image[] iconDisplayed;
        public Image[] iconDisplayedMirored;
        #endregion

        public Sprite[] coatToDisplay;
        public Sprite[] hatToDisplay;
        public Sprite[] shoesToDisplay;
        public Image slotForCloth;

        public TMP_Text _tutorialText;
        
        private void Awake()
        {
            if (_instance != null)
            {
                Destroy(gameObject);
            }
            else
            {
                _instance = this;
            }
            DontDestroyOnLoad(gameObject);
        }

        public void ClearDisplay()
        {
            foreach (var icon in iconDisplayed)
            {
                icon.sprite = null;
                icon.rectTransform.localScale = Vector3.zero;

            }
            foreach (var icon in iconDisplayedMirored)
            {
                icon.sprite = null;
                icon.rectTransform.localScale = Vector3.zero;
            }
        }

        public void ChangeIconPosition(int iconPos, GameAction gameAction)
        {
            iconDisplayed[iconPos].sprite = iconSprites[(int)gameAction.ActionType];
            iconDisplayedMirored[iconPos].sprite = iconSprites[(int)gameAction.ActionType];
            iconDisplayed[iconPos].rectTransform.localScale = Vector3.zero;
            iconDisplayedMirored[iconPos].rectTransform.localScale = Vector3.zero;
            iconDisplayed[iconPos].transform.DOScale(1f, 0.2f).SetEase(Ease.OutBounce);
            iconDisplayedMirored[iconPos].transform.DOScale(1f, 0.2f).SetEase(Ease.OutBounce);
        }

        public void ChangeClothIcon(GameAction gameAction, int clothColor)
        {
            try
            {
                switch (gameAction.ClotheType)
                {
                    case SequenceConfig.ClotheType.Coat:
                        slotForCloth.sprite = coatToDisplay[clothColor];
                        break;
                    case SequenceConfig.ClotheType.Hat:
                        slotForCloth.sprite = hatToDisplay[clothColor];
                        break;
                    case SequenceConfig.ClotheType.Shoe:
                        slotForCloth.sprite = shoesToDisplay[clothColor];
                        break;
                    default:
                        throw new ArgumentOutOfRangeException();
                }
            }
            catch (Exception _)
            {
                // ignored
            }
        }

        public void UpdateTutorialDisplay(TutorialData.TutorialAction tutorialAction)
        {
            _tutorialText.text = tutorialAction.Text;
            
            ClearDisplay();
            if (tutorialAction.ActionType == SequenceConfig.ActionType.Null) return;
            
            ChangeIconPosition(0, tutorialAction);

            switch (tutorialAction.ActionType)
            {
                case SequenceConfig.ActionType.Pickup:
                case SequenceConfig.ActionType.Drop:
                    ChangeClothIcon(tutorialAction, (int)tutorialAction.ClotheColor);
                    break;
                case SequenceConfig.ActionType.Scan:
                    break;
                case SequenceConfig.ActionType.Bell:
                    break;
                case SequenceConfig.ActionType.Null:
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }
    }
}
