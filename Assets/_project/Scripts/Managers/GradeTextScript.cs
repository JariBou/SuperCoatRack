using UnityEngine;

public class GradeTextScript : MonoBehaviour
{
    public float timeBeforeDestroy = 1f;
    void Start()
    {
        Destroy(gameObject, timeBeforeDestroy);
    }
    
}
