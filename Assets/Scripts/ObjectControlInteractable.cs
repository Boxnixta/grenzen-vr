using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.XR.Interaction.Toolkit.Interactables;

public class ObjectControlInteractable : MonoBehaviour
{
    public enum InteractionType { Grab, RemoteSelect }
    public enum ActionType { Deactivate, Activate, Toggle }

    [Header("Ziel-Einstellungen")]
    public GameObject targetObject;
    
    [Header("Interaktions-Einstellungen")]
    public InteractionType interactionMode = InteractionType.RemoteSelect;
    public ActionType action = ActionType.Deactivate;

    [Header("Sichtbarkeit nach Klick")]
    [Tooltip("Wenn AN, wird der Button im Inspector deaktiviert (grau), aber NICHT gelöscht.")]
    public bool hideSelfOnClick = true;

    private XRBaseInteractable interactable;

    void Awake()
    {
        interactable = GetComponent<XRBaseInteractable>();
        if (interactable != null)
            interactable.selectEntered.AddListener(OnActivated);
    }

    private void OnActivated(SelectEnterEventArgs args)
    {
        // Wir prüfen, ob der Interactor zum gewählten Modus passt
        bool isDirect = args.interactorObject is UnityEngine.XR.Interaction.Toolkit.Interactors.XRDirectInteractor;
        bool isRay = args.interactorObject is UnityEngine.XR.Interaction.Toolkit.Interactors.XRRayInteractor;

        // Falls Unity 6 NearFarInteractor nutzt, erlauben wir beides zur Sicherheit
        ExecuteAction();
    }

    private void ExecuteAction()
    {
        if (targetObject == null) return;

        switch (action)
        {
            case ActionType.Deactivate:
                // Schaltet das Raw Image aus
                targetObject.SetActive(false);
                break;

            case ActionType.Activate:
                // Schaltet das Raw Image AN und weckt alle grauen Kinder auf
                // Wir nutzen GetComponentsInChildren(true), um Inaktive zu finden!
                ActivateObjectAndAllChildren(targetObject);
                break;

            case ActionType.Toggle:
                if (targetObject.activeSelf) targetObject.SetActive(false);
                else ActivateObjectAndAllChildren(targetObject);
                break;
        }

        // HIER IST DIE KRITISCHE STELLE:
        if (hideSelfOnClick)
        {
            // SetActive(false) macht das Objekt GRAU in der Hierarchy.
            // Es darf hier NIEMALS 'Destroy(gameObject)' stehen!
            this.gameObject.SetActive(false); 
            Debug.Log($"[ObjectControl] {gameObject.name} wurde deaktiviert.");
        }
    }

    private void ActivateObjectAndAllChildren(GameObject parent)
    {
        parent.SetActive(true);
        
        // Holt absolut alle Kinder, auch die, die im Inspector auf 'Aus' stehen
        Transform[] allChildren = parent.GetComponentsInChildren<Transform>(true);
        foreach (Transform t in allChildren)
        {
            t.gameObject.SetActive(true);
        }
    }

    void OnDestroy()
    {
        if (interactable != null)
            interactable.selectEntered.RemoveListener(OnActivated);
    }
}