using UnityEngine;
using UnityEngine.AI;

public class NPCTalkController : MonoBehaviour
{
    [Header("Komponenten")]
    public AudioSource audioSource;
    public Animator animator;
    public SkinnedMeshRenderer faceMesh;
    public Transform jawRoot;

    [Header("Radien (Das Radar)")]
    public float followRange = 15f;    // Ab hier rennt er los
    public float detectionRange = 3f;  // Ab hier redet er

    [Header("Tuning")]
    public int npcIndex = 0;
    public Vector3 jawRotationAxis = new Vector3(1, 0, 0); 
    public float jawOpenAmount = 30f; 
    public string mouthShapeName = "11_Angry_";

    private Transform playerCamera;
    private bool isFollowingActive = false;
    private bool isTalkingNow = false;
    private bool hasFinishedTalking = false; 
    private int shapeIndex = -1;
    private Quaternion jawStartRot;

    void Start()
    {
        if (jawRoot != null) jawStartRot = jawRoot.localRotation;
        if (faceMesh != null) shapeIndex = faceMesh.sharedMesh.GetBlendShapeIndex(mouthShapeName);
        
        playerCamera = Camera.main.transform;
        if (audioSource != null) audioSource.playOnAwake = false;
    }

    void Update()
    {
        if (playerCamera == null || hasFinishedTalking) return;

        float dist = Vector3.Distance(transform.position, playerCamera.position);

        // SCHRITT 1: LOSLAUFEN (Wenn in followRange)
        if (!isFollowingActive && dist <= followRange)
        {
            StartFollowing();
        }

        // SCHRITT 2: REDEN (Wenn in detectionRange)
        if (isFollowingActive && !isTalkingNow && dist <= detectionRange)
        {
            StartTalking();
        }
    }

    void StartFollowing()
    {
        isFollowingActive = true;
        NPCFollow followScript = GetComponent<NPCFollow>();
        if (followScript != null)
        {
            followScript.isFollowing = true;
            Debug.Log(">>> " + gameObject.name + ": Player entdeckt! Ich laufe jetzt los.");
        }

        // Hier den Sequencer benachrichtigen, um den vorigen NPC zu stoppen
        CharacterSequencer sequencer = Object.FindFirstObjectByType<CharacterSequencer>();
        if (sequencer != null) sequencer.TriggerNextCharacter(npcIndex);
    }

    void StartTalking()
    {
        isTalkingNow = true;
        if (audioSource != null && audioSource.clip != null)
        {
            // Sound-Logik (Räumlich)
            GameObject audioObj = new GameObject("TempAudio_" + gameObject.name);
            audioObj.transform.position = transform.position;
            AudioSource tempSource = audioObj.AddComponent<AudioSource>();
            tempSource.clip = audioSource.clip;
            tempSource.spatialBlend = 1.0f; 
            tempSource.Play();
            FollowTargetAudio follower = audioObj.AddComponent<FollowTargetAudio>();
            follower.target = this.transform;
            Destroy(audioObj, audioSource.clip.length);
            
            Invoke("StopTalking", audioSource.clip.length);
        }

        if (animator != null) animator.SetBool("isTalking", true);
        if (GameFlowManager.instance != null) GameFlowManager.instance.TriggerNPCError(npcIndex);
    }

    void LateUpdate()
    {
        if (isTalkingNow) ApplyFakeMouthMovement();
    }

    void ApplyFakeMouthMovement()
    {
        float noise = Mathf.PerlinNoise(Time.time * 5f, 0f); 
        float smoothedNoise = Mathf.Clamp01((noise - 0.3f) * 1.5f);
        if (jawRoot != null) jawRoot.localRotation = jawStartRot * Quaternion.Euler(jawRotationAxis * (smoothedNoise * jawOpenAmount));
        if (faceMesh != null && shapeIndex != -1) faceMesh.SetBlendShapeWeight(shapeIndex, smoothedNoise * 100f);
    }

    void StopTalking()
    {
        isTalkingNow = false;
        hasFinishedTalking = true; 
        if (animator != null) animator.SetBool("isTalking", false);
        if (jawRoot != null) jawRoot.localRotation = jawStartRot;
        if (faceMesh != null && shapeIndex != -1) faceMesh.SetBlendShapeWeight(shapeIndex, 0);

        NPCFollow followScript = GetComponent<NPCFollow>();
        if (followScript != null) followScript.StopFollowing();

        GetComponent<NPCFollow>().StopFollowing();

        // RUF DEN NÄCHSTEN NPC!
        CharacterSequencer seq = Object.FindFirstObjectByType<CharacterSequencer>();
        if (seq != null)
        {
            seq.OnNPCFinishedTalking();
        }
    }
}