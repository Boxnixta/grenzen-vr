using UnityEngine;

public class PortalCamera : MonoBehaviour
{
    public Transform playerCamera;      
    public Transform[] portalEntrances; 
    public Transform portalExit;        
    public float activationDistance = 50f; 

    private Camera portalCam;
    private Transform activeEntrance;

    void Start()
    {
        portalCam = GetComponent<Camera>();
        // Wir entfernen portalCam.aspect hier komplett, 
        // damit deine Einstellungen im Inspektor erhalten bleiben.
    }

    void Update()
    {
        activeEntrance = GetClosestEntrance();
        if (activeEntrance == null) return;

        float dist = Vector3.Distance(playerCamera.position, activeEntrance.position);

        if (dist < activationDistance)
        {
            portalCam.enabled = true;

            // NUR DIE POSITION WIRD AKTUALISIERT
            // Die Kamera bleibt genau am portalExit. 
            // Die Rotation, die du im Editor eingestellt hast, wird NICHT mehr überschrieben.
            transform.position = portalExit.position;
        }
        else
        {
            portalCam.enabled = false; 
        }
    }

    Transform GetClosestEntrance()
    {
        Transform closest = null;
        float minDistance = Mathf.Infinity;
        foreach (Transform entrance in portalEntrances)
        {
            if (entrance == null) continue;
            float dist = Vector3.Distance(playerCamera.position, entrance.position);
            if (dist < minDistance)
            {
                minDistance = dist;
                closest = entrance;
            }
        }
        return closest;
    }
}