using UnityEngine;

public class BodyFollow : MonoBehaviour
{
    public Transform targetCamera;
    public float yOffset = -0.5f;
    public LayerMask wallLayer; // Hier im Inspector "Everything" oder "Default" wählen

    void Update()
    {
        if (targetCamera != null)
        {
            Vector3 targetPos = targetCamera.position;
            targetPos.y += yOffset;

            // Wir prüfen mit einem unsichtbaren Strahl, ob eine Wand im Weg ist
            Vector3 direction = targetPos - transform.position;
            float distance = direction.magnitude;

            if (distance > 0.01f)
            {
                // Wenn nichts im Weg ist, folge der Kamera
                if (!Physics.Raycast(transform.position, direction.normalized, distance, wallLayer))
                {
                    transform.position = targetPos;
                }
            }

            // Rotation
            Vector3 forward = targetCamera.forward;
            forward.y = 0;
            if (forward != Vector3.zero)
            {
                transform.rotation = Quaternion.LookRotation(forward);
            }
        }
    }
}