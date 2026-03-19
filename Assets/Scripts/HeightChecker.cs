using UnityEngine;
using UnityEngine.XR;
using UnityEngine.SceneManagement; // WICHTIG für Szenen-Events
using System.Collections.Generic;

public sealed class HeightChecker : MonoBehaviour
{
    public Transform cameraTransform;
    public GameObject warningText;
    public float threshold = 0.15f;

    private float initialLocalHeight;

    void Awake()
    {
        // Wir registrieren den Wecker: Wenn eine Szene geladen wird, führe OnSceneLoaded aus
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    void OnDestroy()
    {
        // Wichtig: Den Wecker wieder abbestellen, wenn das Objekt gelöscht wird
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    // Diese Funktion wird AUTOMATISCH aufgerufen, wenn du die Szene wechselst
    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        Debug.Log("Neue Szene geladen: " + scene.name + ". Kalibrierung wird gestartet...");
        Invoke("CalibrateHeight", 0.5f);
    }

    void Start()
    {
        CalibrateHeight();
    }

    public void CalibrateHeight()
    {
        if (cameraTransform != null)
        {
            initialLocalHeight = cameraTransform.localPosition.y;
            if (warningText != null) warningText.SetActive(false);
            Debug.Log("Größe kalibriert auf: " + initialLocalHeight);
        }
    }

    void Update()
    {
        if (cameraTransform != null)
        {
            float currentLocalHeight = cameraTransform.localPosition.y;
            
            // Warnung nur anzeigen, wenn wir über dem Limit sind
            if (currentLocalHeight > initialLocalHeight + threshold)
            {
                if (warningText != null) warningText.SetActive(true);
            }
            else
            {
                if (warningText != null) warningText.SetActive(false);
            }
        }

        // Manueller Reset über X-Button (Linker Controller)
        var leftHandDevices = new List<InputDevice>();
        InputDevices.GetDevicesAtXRNode(XRNode.LeftHand, leftHandDevices);

        if (leftHandDevices.Count > 0)
        {
            bool primaryButtonPressed = false;
            if (leftHandDevices[0].TryGetFeatureValue(CommonUsages.primaryButton, out primaryButtonPressed) && primaryButtonPressed)
            {
                CalibrateHeight();
            }
        }
    }
}