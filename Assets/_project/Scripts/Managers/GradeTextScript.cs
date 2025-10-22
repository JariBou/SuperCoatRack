using _project.Scripts.Managers;
using DG.Tweening;
using UnityEngine;

public class GradeTextScript : MonoBehaviour
{
    void Start()
    {
        // Reset la taille au cas où
        transform.localScale = Vector3.zero;

        // Fait rebondir le texte
        transform
            .DOScale(1f, 0.2f)     // grossit vite
            .SetEase(Ease.InOutBounce)   // effet rebondé vers l’extérieur
            .OnComplete(() =>
            {
                transform
                    .DOScale(0f, 0.7f) // revient à la taille normale
                    .SetEase(Ease.InQuad)
                    .OnComplete(() =>
                        {
                            UIManager.Instance.ChangeClientSprite(false);
                            Destroy(this);
                        });
            });
    }
}
