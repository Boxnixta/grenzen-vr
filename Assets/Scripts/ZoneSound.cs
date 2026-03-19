using UnityEngine;
using System.Collections;

public class ZoneSoundFade : MonoBehaviour
{
    private AudioSource audioSource;
    public float maxVolume = 0.5f; 
    public float fadeTime = 1.5f;  

    void Awake() 
    { 
        audioSource = GetComponent<AudioSource>(); 
        if (audioSource != null) audioSource.volume = 0; 
    }

    private void OnTriggerEnter(Collider other)
    {
        // Wir prüfen auf den Tag ODER ob das Objekt eine Kamera ist
        if (other.CompareTag("MainCamera") || other.GetComponentInChildren<Camera>() != null)
        {
            Debug.Log($"[SOUND] Betrete Zone: {gameObject.name}");
            StopAllCoroutines();
            StartCoroutine(FadeSound(maxVolume));
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("MainCamera") || other.GetComponentInChildren<Camera>() != null)
        {
            Debug.Log($"[SOUND] Verlasse Zone: {gameObject.name}");
            StopAllCoroutines();
            StartCoroutine(FadeSound(0));
        }
    }

    IEnumerator FadeSound(float targetVolume)
    {
        if (audioSource == null) yield break;

        if (targetVolume > 0 && !audioSource.isPlaying) audioSource.Play();

        float startVolume = audioSource.volume;
        float timer = 0;

        while (timer < fadeTime)
        {
            timer += Time.deltaTime;
            audioSource.volume = Mathf.Lerp(startVolume, targetVolume, timer / fadeTime);
            yield return null;
        }

        audioSource.volume = targetVolume;
        if (targetVolume <= 0) audioSource.Stop();
    }
}