using UnityEngine;
using UnityEngine.SceneManagement; // WICHTIG für den Szenenwechsel!

public class FinishTrigger : MonoBehaviour
{
    [Header("Einstellungen")]
    public string zielSzeneName = "EndScene"; // Name der Szene, in die gewechselt werden soll

    private void OnTriggerEnter(Collider other)
    {
        // Wir prüfen wieder, ob es die Kamera (der Spieler) ist
        if (other.CompareTag("MainCamera") || other.GetComponentInChildren<Camera>() != null)
        {
            Debug.Log("Ziel erreicht! Wechsel zu Szene: " + zielSzeneName);
            
            // Wechselt die Szene
            SceneManager.LoadScene(zielSzeneName);
        }
    }
}