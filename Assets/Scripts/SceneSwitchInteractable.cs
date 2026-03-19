using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.XR.Interaction.Toolkit.Interactables;

[RequireComponent(typeof(XRBaseInteractable))]
public class SceneSwitchInteractable : MonoBehaviour
{
    public enum InteractionType { Grab, RemoteSelect }

    [Header("Einstellungen")]
    public string targetSceneName;
    
    [Tooltip("Grab = In die Hand nehmen / RemoteSelect = Mit dem Strahl aus der Ferne klicken")]
    public InteractionType interactionMode = InteractionType.RemoteSelect;

    private XRBaseInteractable interactable;

    void Awake()
    {
        interactable = GetComponent<XRBaseInteractable>();
        
        // Wir hören auf das 'selectEntered' Event (das ist bei XR sowohl Greifen als auch Strahl-Klick)
        interactable.selectEntered.AddListener(OnActivated);
    }

    private void OnActivated(SelectEnterEventArgs args)
    {
        Debug.Log("Objekt wurde aktiviert von: " + args.interactorObject.ToString());
        SwitchScene();
    }

    private void SwitchScene()
    {
        if (!string.IsNullOrEmpty(targetSceneName))
        {
            Debug.Log("Lade Szene: " + targetSceneName);
            SceneManager.LoadScene(targetSceneName);
        }
        else
        {
            Debug.LogError("Szenenname fehlt auf " + gameObject.name);
        }
    }

    void OnDestroy()
    {
        if (interactable != null)
            interactable.selectEntered.RemoveListener(OnActivated);
    }
}