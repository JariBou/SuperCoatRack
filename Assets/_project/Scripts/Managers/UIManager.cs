using System;
using _project.ScriptableObjects.Scripts;
using _project.Scripts;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    #region BeatIcons
    public Sprite[] iconSprites;
    public Image[] iconDisplayed;
    public Image[] iconDisplayedMirored;
    #endregion

    public Sprite[] coatToDisplay;
    public Sprite[] hatToDisplay;
    public Sprite[] shoesToDisplay;
    public Image slotForCloth;
    
    private static UIManager _instance;
    public static UIManager Instance => _instance;

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
        }
        foreach (var icon in iconDisplayedMirored)
        {
            icon.sprite = null;
        }
    }

    public void ChangeIconPosition(int iconPos, SequenceData.SequenceAction actionType)
    {
        iconDisplayed[iconPos].sprite = iconSprites[(int)actionType.ActionType];
        iconDisplayedMirored[iconPos].sprite = iconSprites[(int)actionType.ActionType];
    }

    public void ChangeClothIcon(SequenceData.SequenceAction action, int clothColor)
    {
        switch (action.ClotheType)
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
}
