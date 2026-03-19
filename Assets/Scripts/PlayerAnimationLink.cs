using UnityEngine;

public class PlayerAnimationLink : MonoBehaviour
{
    private Animator animator;
    private Vector3 lastPosition;
    private float currentSpeed;

    void Start()
    {
        animator = GetComponent<Animator>();
        lastPosition = transform.position;
    }

    void Update()
    {
        // Geschwindigkeit manuell berechnen: Weg durch Zeit
        float distance = Vector3.Distance(transform.position, lastPosition);
        currentSpeed = distance / Time.deltaTime;
        lastPosition = transform.position;

        if (animator != null)
        {
            bool isMoving = currentSpeed > 0.05f; // Sehr sensibel eingestellt
            
            animator.SetBool("isWalking", isMoving);

            if (isMoving)
            {
                // Wir sorgen dafür, dass die Animation mindestens mit Speed 1 läuft,
                // auch wenn wir uns in VR nur langsam schieben.
                float animSpeed = Mathf.Max(currentSpeed * 1.5f, 0.8f); 
                animator.SetFloat("WalkSpeed", animSpeed);
            }
            else
            {
                animator.SetFloat("WalkSpeed", 0);
            }
        }
    }
}