using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.XR.Interaction.Toolkit.Samples.StarterAssets;

public class PlayerController : MonoBehaviour
{
    [Header("References")]
    public DynamicMoveProvider moveProvider; 
    public Animator animator; 
    public GameObject movementTarget; 

    [Header("Movement Settings")]
    public InputActionReference runAction;   
    public float walkSpeed = 2.5f;
    public float runSpeed = 6.0f;
    public float stopDelay = 0.2f; // Zeit in Sekunden, die er "nachlaufen" darf

    private Vector3 lastPosition;
    private float stopTimer;

    void Start()
    {
        if (movementTarget != null)
            lastPosition = movementTarget.transform.position;
    }

    void Update()
    {
        if (moveProvider == null || animator == null || movementTarget == null) return;

        // 1. Bewegung messen
        Vector3 currentPos = movementTarget.transform.position;
        float distanceMoved = Vector2.Distance(new Vector2(currentPos.x, currentPos.z), new Vector2(lastPosition.x, lastPosition.z));
        lastPosition = currentPos;

        // 2. Renn-Logik
        float runInput = runAction != null ? runAction.action.ReadValue<float>() : 0f;
        bool isRunning = runInput > 0.1f;
        moveProvider.moveSpeed = isRunning ? runSpeed : walkSpeed;

        // 3. Animation mit Puffer steuern
        // Wenn die Bewegung über einem winzigen Schwellenwert liegt:
        if (distanceMoved > 0.001f) 
        {
            stopTimer = stopDelay; // Timer immer wieder auf max setzen
            animator.SetBool("isWalking", true);
        }
        else
        {
            // Wenn keine Bewegung da ist, den Timer runterzählen
            stopTimer -= Time.deltaTime;
            if (stopTimer <= 0)
            {
                animator.SetBool("isWalking", false);
            }
        }

        // WalkSpeed setzen (nur wenn isWalking noch true ist)
        if (animator.GetBool("isWalking"))
        {
            animator.SetFloat("WalkSpeed", isRunning ? 1.4f : 1.0f);
        }
        else
        {
            animator.SetFloat("WalkSpeed", 0f);
        }
    }
}