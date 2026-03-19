using UnityEngine;

public class AudioDebugger : MonoBehaviour
{
    public AudioSource source;
    private bool wasPlaying = false;

    void Update()
    {
        if (source == null) return;

        if (wasPlaying && !source.isPlaying && source.time < source.clip.length - 0.1f)
        {
            Debug.LogError("!!! SABOTAGE GEFUNDEN !!! Der Sound wurde gestoppt bei Sekunde: " + source.time);
            // Wir schauen in den StackTrace der Console, um zu sehen, wer Stop() gerufen hat.
        }
        wasPlaying = source.isPlaying;

        // Einmal pro Sekunde den Status checken
        if (Time.frameCount % 60 == 0)
        {
             // Debug.Log("Status: " + (source.isPlaying ? "Spielt" : "Stumm") + " Zeit: " + source.time);
        }
    }
    
    // Test-Button zum Starten
    [ContextMenu("Manueller Start")]
    public void ForcePlay() { source.Play(); }
}