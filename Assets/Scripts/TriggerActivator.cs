using UnityEngine;

public class TriggerActivator : MonoBehaviour
{
    [Header("Ziel-Einstellungen")]
    public GameObject imageToActivate;

    private bool hasTriggered = false;

    private void OnTriggerEnter(Collider other)
    {
        if (hasTriggered) return;

        // Wir loggen, was uns berührt, damit wir sicher sind
        Debug.Log("Trigger berührt von: " + other.gameObject.name);

        // PRÜFUNG: Ist es die Kamera oder ein Teil des Spielers?
        // Wir suchen nach der Kamera-Komponente im Objekt oder seinen Eltern
        if (other.GetComponentInChildren<Camera>() != null || other.CompareTag("MainCamera"))
        {
            if (imageToActivate != null)
            {
                imageToActivate.SetActive(true);
                hasTriggered = true; 
                Debug.Log("!!! ERFOLG: Bild im Inspector aktiviert !!!");
            }
        }
    }
}