using _project.Scripts.Managers;
using UnityEngine;

public class FeedbackManager : MonoBehaviour
{
    [SerializeField] private GameObject feedbackOnInputText;

    [SerializeField] private GameObject rippleObject;

    [SerializeField] private Transform[] listOfPosForRippleEffect;

    private Color currentColorForRipple;
    private Color currentColorForMiddleRipple;

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
        currentColorForRipple = Color.blue;
        currentColorForMiddleRipple = Color.magenta;
    }
    #endregion

    public FeedbackManager FeedbackTimingInput(LevelSequencesManager.SequenceActionState currentActionState)
    {
        if (listOfPosForRippleEffect.Length == 0) return this;
        int randomPos = Random.Range(0, 2);
        if (feedbackOnInputText)
        {
            var tmp = Instantiate(feedbackOnInputText, listOfPosForRippleEffect[randomPos].position, transform.rotation, transform);
            switch (currentActionState)
            {
                case LevelSequencesManager.SequenceActionState.Succeeded:
                    tmp.GetComponent<TextMesh>().text = "Succeeded";
                    break;
                case LevelSequencesManager.SequenceActionState.Failed:
                    tmp.GetComponent<TextMesh>().text = "Failed";
                    currentColorForRipple = Color.red;
                    currentColorForMiddleRipple = new Color(0.47f, 0.22f, 0.14f);
                    break;
                case LevelSequencesManager.SequenceActionState.Good:
                    tmp.GetComponent<TextMesh>().text = "Good";
                    currentColorForRipple = Color.green;
                    currentColorForMiddleRipple = new Color(0.91f, 0.64f, 1f);
                    break;
                case LevelSequencesManager.SequenceActionState.Bad:
                    tmp.GetComponent<TextMesh>().text = "Bad";
                    currentColorForRipple = new Color(0.86f, 0.54f, 0f);
                    currentColorForMiddleRipple = new Color(1f, 0.39f, 0.43f);
                    break;
                case LevelSequencesManager.SequenceActionState.Perfect:
                    tmp.GetComponent<TextMesh>().text = "Perfect";
                    currentColorForRipple = Color.yellow;
                    currentColorForMiddleRipple = new Color(1f, 0.95f, 0.84f);
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
                ParticleSystem.MainModule particleSystemMain = rippletmp.GetComponent<ParticleSystem>().main;
                particleSystemMain.startColor = currentColorForRipple;
                rippletmp.GetComponent<Renderer>().material.SetColor("_MiddleAccentColor", Color.magenta);
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
            currentColorForRipple = color;
        }
        return this;
    }

    public static FeedbackManager ChangeRippleColorStatic(Color color)
    {
        return _instance.ChangeRippleColor(color);
    }
}
