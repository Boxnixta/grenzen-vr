using UnityEngine;

public class CameraHeightSmoother : MonoBehaviour
{
    [Header("Einstellungen")]
    public float smoothSpeed = 5.0f; // Höher = direkter, Niedriger = weicher/gleitender
    
    private float targetY;
    private Vector3 lastParentPos;

    void Start()
    {
        targetY = transform.localPosition.y;
        if (transform.parent != null) lastParentPos = transform.parent.position;
    }

    void LateUpdate()
    {
        if (transform.parent == null) return;

        // Wir berechnen, wie stark sich das XR Origin (Parent) bewegt hat
        Vector3 currentParentPos = transform.parent.position;
        float yDiff = currentParentPos.y - lastParentPos.y;

        // Wir bewegen die Kamera lokal in die entgegengesetzte Richtung des "Hopplers"
        // und lassen sie dann langsam wieder auf ihre Ziel-Position gleiten
        Vector3 localPos = transform.localPosition;
        localPos.y -= yDiff; 
        localPos.y = Mathf.Lerp(localPos.y, targetY, Time.deltaTime * smoothSpeed);
        
        transform.localPosition = localPos;
        lastParentPos = currentParentPos;
    }
}