using _project.Scripts.Managers;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UIElements;

public class CounterImageScript : MonoBehaviour
{
    [SerializeField] private GameObject nextObject;
    
    public void JumpItself()
    {
        // Reset la taille au cas où
        transform.localScale = Vector3.zero;

        // Fait rebondir le texte
        transform
            .DOScale(1f, 0.5f)     // grossit vite
            .SetEase(Ease.InQuad)   // effet rebondé vers l’extérieur
            .OnComplete(() =>
            {
                transform
                    .DOScale(0f, 0.4f) // revient à la taille normale
                    .SetEase(Ease.OutQuad)
                    .OnComplete(() =>
                    {
                        if (nextObject != null)
                        {
                            nextObject.SetActive(true);
                            nextObject.GetComponent<CounterImageScript>().JumpItself();
                            gameObject.SetActive(false);
                        }
                        else
                        {
                            GameManager.Instance.BeginLevel();
                        }
                    });
            });
    }
}
