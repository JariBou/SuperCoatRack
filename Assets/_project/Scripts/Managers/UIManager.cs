using System;
using _project.ScriptableObjects.Scripts;
using _project.Scripts;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    public Sprite[] iconSprites;
    public Image[] iconDisplayed;
    public Image[] iconDisplayedMirored;
    
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
}
