using UnityEngine;

public class UIFollowPlayer : MonoBehaviour
{
    public Transform cameraTransform;
    public float distance = 0.6f;
    public float speed = 5.0f;

    void LateUpdate()
    {
        // Zielposition berechnen
        Vector3 targetPosition = cameraTransform.position + cameraTransform.forward * distance;
        
        // Sanft dorthin bewegen
        transform.position = Vector3.Lerp(transform.position, targetPosition, Time.deltaTime * speed);
        
        // Immer zum Spieler schauen
        transform.LookAt(transform.position + cameraTransform.rotation * Vector3.forward, cameraTransform.rotation * Vector3.up);
    }
}