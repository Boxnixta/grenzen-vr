using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.XR.Interaction.Toolkit.Interactors; // Neu für Unity 6
using UnityEngine.EventSystems; // Für die UI-Erkennung

public class RaycastDebugger : MonoBehaviour
{
    private XRRayInteractor rayInteractor;

    void Start() 
    { 
        rayInteractor = GetComponent<XRRayInteractor>(); 
    }

    void Update()
    {
        if (rayInteractor == null) return;

        // Check für 3D Objekte
        if (rayInteractor.TryGetCurrent3DRaycastHit(out RaycastHit hit))
        {
            Debug.Log("Strahl trifft OBJEKT: " + hit.collider.gameObject.name);
        }
        
        // Check für UI (Buttons)
        if (rayInteractor.TryGetCurrentUIRaycastResult(out var uiHit))
        {
            if (uiHit.gameObject != null)
            {
                Debug.Log("Strahl trifft UI: " + uiHit.gameObject.name);
            }
        }
    }
}