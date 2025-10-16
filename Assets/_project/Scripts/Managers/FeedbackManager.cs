using System;
using _project.Scripts.Managers;
using UnityEngine;

public class FeedbackManager : MonoBehaviour
{
    [SerializeField] private GameObject feedbackOnInputText;

    [SerializeField] private GameObject rippleObject;

    #region Init
    
    private static FeedbackManager _instance;
    public static FeedbackManager Instance => _instance;

    private void Start()
    {
        if (Instance != null){
            Destroy(gameObject);
        }
        else
        {
            _instance = this;
        }
    }

    #endregion

    public FeedbackManager FeedbackTimingInput(LevelSequencesManager.SequenceActionState currentActionState)
    {
        if (feedbackOnInputText)
        {
            var tmp = Instantiate(feedbackOnInputText, transform.position, transform.rotation, transform);
            switch (currentActionState)
            {
                case LevelSequencesManager.SequenceActionState.Succeeded:
                    tmp.GetComponent<TextMesh>().text = "Succeeded";
                    break;
                case LevelSequencesManager.SequenceActionState.Failed:
                    tmp.GetComponent<TextMesh>().text = "Failed";
                    break;
                case LevelSequencesManager.SequenceActionState.Good:
                    tmp.GetComponent<TextMesh>().text = "Good";
                    break;
                case LevelSequencesManager.SequenceActionState.Bad:
                    tmp.GetComponent<TextMesh>().text = "Bad";
                    break;
                case LevelSequencesManager.SequenceActionState.Perfect:
                    tmp.GetComponent<TextMesh>().text = "Perfect";
                    break;
                case LevelSequencesManager.SequenceActionState.None:
                    tmp.GetComponent<TextMesh>().text = "ERROR";
                    return this;
                default:
                    break;
            }
            if (rippleObject)
            {
                var rippletmp = Instantiate(rippleObject, tmp.transform.position, transform.rotation, tmp.transform);
                rippletmp.GetComponent<ParticleSystem>().Play();
            }
        }
        return this;
    }
    
    public static FeedbackManager FeedbackTimingInputStatic(LevelSequencesManager.SequenceActionState currentActionState)
    {
        return _instance.FeedbackTimingInput(currentActionState);
    }
    
    private FeedbackManager ChangeRippleColor(Color color)
    {
        if (rippleObject)
        {
            ParticleSystem.MainModule particleSystemMain = rippleObject.GetComponent<ParticleSystem>().main;
            particleSystemMain.startColor = color;
            rippleObject.GetComponent<Renderer>().material.color = color;
        }
        return this;
    }

    public static FeedbackManager ChangeRippleColorStatic(Color color)
    {
        return _instance.ChangeRippleColor(color);
    }
}
