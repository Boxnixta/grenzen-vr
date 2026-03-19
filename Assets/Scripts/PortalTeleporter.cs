using UnityEngine;
using UnityEngine.UI; // Wichtig für das Image
using System.Collections;

public class PortalTeleporter : MonoBehaviour
{
    public Transform player;
    public Transform receiver;
    public Image fadeImage; // Hier das schwarze Bild reinziehen
    public float fadeDuration = 0.2f;

    private bool isTeleporting = false;

    private void OnTriggerEnter(Collider other)
    {
        if (!isTeleporting && (other.CompareTag("MainCamera") || other.GetComponentInParent<CharacterController>()))
        {
            StartCoroutine(TeleportRoutine());
        }
    }

    IEnumerator TeleportRoutine()
    {
        isTeleporting = true;

        // 1. Schwarz werden lassen
        float timer = 0;
        while (timer < fadeDuration)
        {
            timer += Time.deltaTime;
            fadeImage.color = new Color(0, 0, 0, timer / fadeDuration);
            yield return null;
        }

        // 2. Beamen
        player.position = receiver.position;
        player.rotation = receiver.rotation;

        // 3. Kurz warten (verhindert das Flackern)
        yield return new WaitForSeconds(0.1f);

        // 4. Wieder sichtig werden
        timer = fadeDuration;
        while (timer > 0)
        {
            timer -= Time.deltaTime;
            fadeImage.color = new Color(0, 0, 0, timer / fadeDuration);
            yield return null;
        }

        isTeleporting = false;
    }
}