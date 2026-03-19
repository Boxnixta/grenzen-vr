using UnityEngine;
using Unity.XR.CoreUtils;

public class PlayerSceneStartReset : MonoBehaviour
{
    void Start()
    {
        // Wir suchen den XR Origin in der neuen Szene
        XROrigin xrOrigin = FindFirstObjectByType<XROrigin>();

        if (xrOrigin != null)
        {
            // 1. Wir setzen die Kamera-Verschiebung innerhalb des Rigs zurück
            xrOrigin.MoveCameraToWorldLocation(xrOrigin.transform.position);
            
            // 2. Wir stellen sicher, dass die Rotation auch stimmt
            xrOrigin.MatchOriginUpCameraForward(xrOrigin.transform.up, xrOrigin.transform.forward);
            
            Debug.Log("Player-Position für diese Szene erfolgreich auf den XR Origin Startpunkt resetet.");
        }
    }
}