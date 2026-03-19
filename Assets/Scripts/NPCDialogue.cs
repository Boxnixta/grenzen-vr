using UnityEngine;

public class NPCDialogue : MonoBehaviour
{
    public AudioSource audioSource; // Die MP3 Datei
    public Animator animator;      // Für die Gesichtsanimation
    public float detectionRange = 3.0f; // Ab wie viel Metern er redet
    public Transform player;       // Dein XR Origin / Kamera

    private bool hasPlayed = false;

    void Update()
    {
        if (player == null || hasPlayed) return;

        // Distanz messen
        float distance = Vector3.Distance(transform.position, player.position);

        if (distance <= detectionRange)
        {
            PlayDialogue();
        }
    }

    void PlayDialogue()
    {
        hasPlayed = true;
        
        // Sound abspielen
        if (audioSource != null) audioSource.Play();

        // Gesichtsanimation starten
        // Du musst im Animator eine Transition zu einem "Talk"-State haben
        if (animator != null) animator.SetBool("isTalking", true);
        
        // Nach der Länge des Sounds wieder aufhören zu animieren
        Invoke("StopTalking", audioSource.clip.length);
    }

    void StopTalking()
    {
        if (animator != null) animator.SetBool("isTalking", false);
    }
}