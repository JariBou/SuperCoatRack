using UnityEngine;

public class QrCodeScanManager : MonoBehaviour
{
    void Update()
    {
        // Si l’utilisateur scanne, le texte se remplit automatiquement
        if (Input.GetKeyDown(KeyCode.Return))
        {
            Debug.Log($"Code scanné : {Input.GetKeyDown(KeyCode.Return)}");
        }
    }
}
