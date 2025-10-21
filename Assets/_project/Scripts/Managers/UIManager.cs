using System;
using _project.ScriptableObjects.Scripts;
using DG.Tweening;
using GraphicsLabor.Scripts.Attributes.LaborerAttributes.DrawerAttributes;
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
        [ShowMessage("Clothe Color List indexes:\nRed = 0\nBlue = 1\nYellow = 2\nBlack = 3\nBrownTalon = 4\nBrownShoes = 5\nBlackRed = 6")]
        public Image slotForCloth;
        [SerializeField] private Sprite _fallbackImage;
        
        [SerializeField] private RectTransform[] elevatorDifferentPoses;
        [SerializeField] private RectTransform elevatorPos;

        [SerializeField] private TextMeshProUGUI scoreText;
        
        [SerializeField] private TMP_Text _tutorialText;
        [SerializeField] private Image _tutorialImage;
        [SerializeField] private Image _tutorialBackground;
        
        [SerializeField] private CoatRackPlacementManager _coatRackPlacementManager;

        [SerializeField] private CounterImageScript FIRSTIMAGE;

        [SerializeField] private Image Client;
        [SerializeField] private Image Spirou;
        public LevelData levelData;

        [SerializeField] private Sprite[] ScoreSprites;
        
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
            ClearDisplay();
        }

        private void Start()
        {
            if (FIRSTIMAGE != null)
            {
                FIRSTIMAGE.JumpItself();
            }
        }

        public void ClearDisplay()
        {
            foreach (var icon in iconDisplayed)
            {
                icon.sprite = null;
                icon.enabled = false;
                // icon.rectTransform.localScale = Vector3.zero;

            }
            foreach (var icon in iconDisplayedMirored)
            {
                icon.sprite = null;
                icon.enabled = false;
                // icon.rectTransform.localScale = Vector3.zero;
            }
        }

        public void ChangeIconPosition(int iconPos, GameAction gameAction)
        {
            iconDisplayed[iconPos].sprite = iconSprites[(int)gameAction.ActionType];
            iconDisplayedMirored[iconPos].sprite = iconSprites[(int)gameAction.ActionType];
            Vector3 baseScale = iconDisplayedMirored[iconPos].rectTransform.localScale;
            iconDisplayed[iconPos].rectTransform.localScale = Vector3.zero;
            iconDisplayedMirored[iconPos].rectTransform.localScale = Vector3.zero;
            iconDisplayed[iconPos].transform.DOScale(baseScale, 0.2f).SetEase(Ease.OutBounce);
            iconDisplayedMirored[iconPos].transform.DOScale(baseScale, 0.2f).SetEase(Ease.OutBounce);
            Spirou.transform.DOScale(.8f, 0.2f).SetEase(Ease.OutQuad);
            Spirou.transform.DOScale(1f, 0.2f).SetEase(Ease.OutBounce);
            iconDisplayed[iconPos].enabled = true;
            iconDisplayedMirored[iconPos].enabled = true;
        }

        public void ChangeClothIcon(GameAction gameAction, int clothColor)
        {
            switch (gameAction.ClotheType)
            {
                case SequenceConfig.ClotheType.Coat:
                    NextClotheDisplayManager.ConfigNewClotheDisplay(coatToDisplay[clothColor],
                        coatToDisplay[clothColor]);

                    break;
                case SequenceConfig.ClotheType.Hat:
                    NextClotheDisplayManager.ConfigNewClotheDisplay(hatToDisplay[clothColor], hatToDisplay[clothColor]);

                    break;
                case SequenceConfig.ClotheType.Shoe:
                    NextClotheDisplayManager.ConfigNewClotheDisplay(shoesToDisplay[clothColor],
                        shoesToDisplay[clothColor]);

                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
            // try
            // {
            //     //Debug.Log("Changing cloth icon");
            //     if (clothColor >= coatToDisplay.Length)
            //     {
            //         Debug.LogWarning("Cloth icon is too small (no that's not the message rider but ok...)");
            //         slotForCloth.sprite = _fallbackImage;
            //         return;
            //     }
            //     switch (gameAction.ClotheType)
            //     {
            //         case SequenceConfig.ClotheType.Coat:
            //             slotForCloth.sprite = coatToDisplay[clothColor];
            //             break;
            //         case SequenceConfig.ClotheType.Hat:
            //             slotForCloth.sprite = hatToDisplay[clothColor];
            //             break;
            //         case SequenceConfig.ClotheType.Shoe:
            //             slotForCloth.sprite = shoesToDisplay[clothColor];
            //             break;
            //         default:
            //             throw new ArgumentOutOfRangeException();
            //     }
            //     slotForCloth.enabled = true;
            // }
            // catch (Exception _)
            // {
            //     Debug.LogWarning("Exception caught in ChangeClothIcon");
            //     // ignored
            // }
        }

        public void ClearNextClotheDisplay()
        {
            slotForCloth.enabled = false;
            slotForCloth.sprite = null;
        }

        public void UpdateTutorialDisplay(TutorialData.TutorialAction tutorialAction)
        {
            _tutorialText.text = tutorialAction.Text;

            if (tutorialAction.HasImage)
            {
                _tutorialImage.sprite = tutorialAction.ImageToDisplay;
                _tutorialImage.gameObject.SetActive(true);
            }
            else
            {
                _tutorialImage.gameObject.SetActive(false);
            }
            
            ClearDisplay();
            if (tutorialAction.ActionType == SequenceConfig.ActionType.Null) return;
            _coatRackPlacementManager.ShowPlacement(tutorialAction);

            ChangeIconPosition(0, tutorialAction);

            switch (tutorialAction.ActionType)
            {
                case SequenceConfig.ActionType.Pickup:
                case SequenceConfig.ActionType.Drop:
                case SequenceConfig.ActionType.Scan:
                    ChangeClothIcon(tutorialAction, (int)tutorialAction.ClotheColor);
                    break;
                case SequenceConfig.ActionType.Bell:
                    break;
                case SequenceConfig.ActionType.Null:
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        public void ChangeElevatorPosition(float elevatorAdvancement)
        {
            Vector3 tmpPos = elevatorPos.anchoredPosition;
            tmpPos.y = Mathf.Lerp(elevatorDifferentPoses[0].anchoredPosition.y, elevatorDifferentPoses[1].anchoredPosition.y, elevatorAdvancement);
            elevatorPos.anchoredPosition = tmpPos;
        }

        public void ChangeScoreValue(int score)
        {
            scoreText.text = "Score : " + score;
        }

        public Sprite GetGradeSprite()
        {
            if (ScoreSprites.Length > 0)
            {
                float maxScorePossible = ScoreManager.Instance.GetLevelMaxScoreNeeded(levelData);
                int currentScore = ScoreManager.Instance.Score;
                int scoreGradeLimit = ScoreSprites.Length;
                int finalGrade =  Mathf.FloorToInt(scoreGradeLimit * currentScore / maxScorePossible);
                return ScoreSprites[finalGrade];
            }
            return null;
        }

        public void EnableTutorialDisplay()
        {
            _tutorialText.gameObject.SetActive(true);
            _tutorialImage.gameObject.SetActive(true);
            _tutorialBackground.gameObject.SetActive(true);
        }
        
        public void DisableTutorialDisplay()
        {
            _tutorialText.gameObject.SetActive(false);
            _tutorialImage.gameObject.SetActive(false);
            _tutorialBackground.gameObject.SetActive(false);
        }

        public void UpdateNextClotheIcon(GameAction currentGameAction, GameAction nextGameAction)
        {
            Sprite currentSprite = null;
            if (currentGameAction != null)
            {
                if (currentGameAction.ActionType == SequenceConfig.ActionType.Bell)
                {
                    currentSprite = iconSprites[(int)SequenceConfig.ActionType.Bell];
                } else
                {
                    switch (currentGameAction.ClotheType)
                    {
                        case SequenceConfig.ClotheType.Coat:
                            currentSprite = coatToDisplay[(int)currentGameAction.ClotheColor];
                            break;
                        case SequenceConfig.ClotheType.Hat:
                            currentSprite = hatToDisplay[(int)currentGameAction.ClotheColor];
                            break;
                        case SequenceConfig.ClotheType.Shoe:
                            currentSprite = shoesToDisplay[(int)currentGameAction.ClotheColor];
                            break;
                        default:
                            throw new ArgumentOutOfRangeException();
                    }
                }
            }
            
            Sprite nextSprite = null;
            if (nextGameAction != null)
            {
                if (nextGameAction.ActionType == SequenceConfig.ActionType.Bell)
                {
                    nextSprite = iconSprites[(int)SequenceConfig.ActionType.Bell];
                } else
                {
                    switch (nextGameAction.ClotheType)
                    {
                        case SequenceConfig.ClotheType.Coat:
                            nextSprite = coatToDisplay[(int)nextGameAction.ClotheColor];
                            break;
                        case SequenceConfig.ClotheType.Hat:
                            nextSprite = hatToDisplay[(int)nextGameAction.ClotheColor];
                            break;
                        case SequenceConfig.ClotheType.Shoe:
                            nextSprite = shoesToDisplay[(int)nextGameAction.ClotheColor];
                            break;
                        default:
                            throw new ArgumentOutOfRangeException();
                    }
                }
            }
            _coatRackPlacementManager.ShowPlacement(currentGameAction);
            NextClotheDisplayManager.ConfigNewClotheDisplay(currentSprite, nextSprite);
        }

        public void InitNextClotheIcon(GameAction currentGameAction, GameAction nextGameAction)
        {
            Sprite currentSprite;
            if (currentGameAction.ActionType == SequenceConfig.ActionType.Bell)
            {
                currentSprite = iconSprites[(int)SequenceConfig.ActionType.Bell];
            } else
            {
                switch (currentGameAction.ClotheType)
                {
                    case SequenceConfig.ClotheType.Coat:
                        currentSprite = coatToDisplay[(int)currentGameAction.ClotheColor];
                        break;
                    case SequenceConfig.ClotheType.Hat:
                        currentSprite = hatToDisplay[(int)currentGameAction.ClotheColor];
                        break;
                    case SequenceConfig.ClotheType.Shoe:
                        currentSprite = shoesToDisplay[(int)currentGameAction.ClotheColor];
                        break;
                    default:
                        throw new ArgumentOutOfRangeException();
                }
            }
            
            Sprite nextSprite;
            if (nextGameAction.ActionType == SequenceConfig.ActionType.Bell)
            {
                nextSprite = iconSprites[(int)SequenceConfig.ActionType.Bell];
            } else
            {
                switch (nextGameAction.ClotheType)
                {
                    case SequenceConfig.ClotheType.Coat:
                        nextSprite = coatToDisplay[(int)nextGameAction.ClotheColor];
                        break;
                    case SequenceConfig.ClotheType.Hat:
                        nextSprite = hatToDisplay[(int)nextGameAction.ClotheColor];
                        break;
                    case SequenceConfig.ClotheType.Shoe:
                        nextSprite = shoesToDisplay[(int)nextGameAction.ClotheColor];
                        break;
                    default:
                        throw new ArgumentOutOfRangeException();
                }
            }
            NextClotheDisplayManager.InitClotheDisplay(currentSprite, nextSprite);
        }

        public void ChangeClientSprite(bool isHappy)
        {
            if (isHappy)
            {
                if (levelData.HappyClient )
                {
                    Client.sprite = levelData.HappyClient;
                } 
            }
            else
            {
                if (levelData.Client )
                {
                    Client.sprite = levelData.Client;
                }
            }
        }
    }
}
