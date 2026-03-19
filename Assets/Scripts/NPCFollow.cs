using UnityEngine;
using UnityEngine.AI;

public class NPCFollow : MonoBehaviour
{
    private static NPCFollow aktuellerBegleiter;

    private NavMeshAgent agent;
    private Animator animator;
    private AudioSource audioSource;

    [Header("VR Setup")]
    public Transform playerCamera; 

    [Header("Blickkontakt Settings")]
    [Range(0, 1)] public float headLookWeight = 0.8f;
    [Range(0, 1)] public float bodyLookWeight = 0.2f;

    [Header("Status")]
    public bool isFollowing = false;
    private bool hatGesprochen = false;
    private bool isPausedByError = false;
    
    [Header("Abstände & Speed")]
    public float stoppAbstand = 1.8f;      // Er hält 1.8m vor dir an
    public float abstandVorKamera = 2.0f;  // Er läuft auf diesen Punkt zu
    public float laufSpeed = 1.2f;         // Gemütliches Gehtempo
    public float rückzugDistanz = 1.2f;    // Wenn du näher als das kommst, weicht er aus

    private float updateIntervall = 0.1f; 
    private float nächsterUpdateTermin;

    void Awake()
    {
        animator = GetComponent<Animator>();
        audioSource = GetComponent<AudioSource>();
        if (playerCamera == null) playerCamera = Camera.main.transform;
    }

    void Update()
    {
        // 1. Error Check (Pausiert Audio bei UI)
        bool errorAktiv = false;
        if (GameFlowManager.instance != null && GameFlowManager.instance.errorCanvas != null)
        {
            errorAktiv = GameFlowManager.instance.errorCanvas.activeInHierarchy;
        }

        HandleAudioPause(errorAktiv);

        if (isFollowing)
        {
            // Stafetten-Logik: Nur einer darf folgen
            if (aktuellerBegleiter != null && aktuellerBegleiter != this)
            {
                aktuellerBegleiter.StopFollowing();
            }
            aktuellerBegleiter = this;

            if (agent == null) SetupAgentAtRuntime();
            if (agent != null && agent.isStopped) agent.isStopped = false;

            // 2. BEWEGUNG MIT ABSTANDS-LOGIK
            if (agent != null && agent.isOnNavMesh && playerCamera != null)
            {
                float aktuelleDistanz = Vector3.Distance(transform.position, playerCamera.position);

                // LOGIK A: Du bist ZU NAH (Social Distancing)
                if (aktuelleDistanz < rückzugDistanz)
                {
                    // Berechne einen Punkt weg vom Player
                    Vector3 richtungWeg = (transform.position - playerCamera.position).normalized;
                    Vector3 fluchtZiel = transform.position + richtungWeg * 1.5f;
                    
                    agent.SetDestination(fluchtZiel);
                    agent.speed = 0.8f; // Er weicht vorsichtig zurück
                }
                // LOGIK B: Normales Folgen
                else if (Time.time >= nächsterUpdateTermin)
                {
                    Vector3 zielPunkt = playerCamera.position + (playerCamera.forward * abstandVorKamera);
                    zielPunkt.y = transform.position.y; // Auf dem Boden bleiben
                    
                    agent.SetDestination(zielPunkt);
                    agent.speed = laufSpeed;
                    nächsterUpdateTermin = Time.time + updateIntervall;
                }

                // Animationen steuern
                if (animator != null)
                {
                    float speed = agent.velocity.magnitude;
                    animator.SetBool("isWalking", speed > 0.1f);
                    animator.SetFloat("WalkSpeed", speed);
                }
                LookAtPlayer();
            }
        }

        // 3. Audio & Talk Animation
        if (isFollowing && audioSource != null && audioSource.clip != null)
        {
            if (audioSource.isPlaying) 
            {
                hatGesprochen = true;
                if (animator != null) animator.SetBool("isTalking", true);
            }
            else if (!isPausedByError && hatGesprochen) 
            {
                if (animator != null) animator.SetBool("isTalking", false);
                // Hier stoppen wir nicht mehr sofort, damit er nach dem Reden 
                // nicht einfach "einfriert", sondern der Sequencer ihn ablöst.
            }
        }
    }

    private void HandleAudioPause(bool errorAktiv)
    {
        if (audioSource == null) return;
        if (errorAktiv && audioSource.isPlaying)
        {
            audioSource.Pause();
            isPausedByError = true;
            if (animator != null) animator.SetBool("isTalking", false);
        }
        else if (!errorAktiv && isPausedByError)
        {
            audioSource.UnPause();
            isPausedByError = false;
            if (animator != null) animator.SetBool("isTalking", true);
        }
    }

    void OnAnimatorIK(int layerIndex)
    {
        if (isFollowing && animator != null && playerCamera != null)
        {
            animator.SetLookAtPosition(playerCamera.position);
            animator.SetLookAtWeight(headLookWeight, bodyLookWeight, 0.5f); 
        }
    }

    public void StopFollowing()
    {
        isFollowing = false;
        hatGesprochen = false;
        if (agent != null && agent.isActiveAndEnabled)
        {
            agent.isStopped = true;
            agent.velocity = Vector3.zero;
        }
        if (animator != null)
        {
            animator.SetBool("isWalking", false);
            animator.SetBool("isTalking", false);
        }
        if (aktuellerBegleiter == this) aktuellerBegleiter = null;
    }

    void SetupAgentAtRuntime()
    {
        agent = GetComponent<NavMeshAgent>();
        if (agent == null) agent = gameObject.AddComponent<NavMeshAgent>();

        agent.speed = laufSpeed;
        agent.acceleration = 3f; // Sanftes Anlaufen
        agent.stoppingDistance = stoppAbstand;
        agent.radius = 0.4f; // NPC hat einen "Körper"
        
        if (NavMesh.SamplePosition(transform.position, out NavMeshHit hit, 1.0f, NavMesh.AllAreas))
            agent.Warp(hit.position);
    }

    void LookAtPlayer()
    {
        Vector3 richtung = (playerCamera.position - transform.position).normalized;
        richtung.y = 0; 
        if (richtung != Vector3.zero)
            transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(richtung), Time.deltaTime * 5f);
    }
}