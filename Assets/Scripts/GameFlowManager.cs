using UnityEngine;
using UnityEngine.SceneManagement;

public class GameFlowManager : MonoBehaviour
{
    public static GameFlowManager instance;

    [Header("UI Referenzen")]
    public GameObject errorCanvas;

    [Header("UI Elemente")]
    public GameObject[] npcWarnings;    
    public GameObject[] systemErrors;   
    public GameObject finalErrorObj;    
    public GameObject aufgebenAbfrageObj; 

    private int errorsGezählt = 0;
    private int aktuellerNPCIndex = 0;
    private GameObject aktuellAktivesBild;
    
    private bool warteAufGrabWarning = false;
    private bool warteAufGrabSystem = false;

    void Awake() { instance = this; }

    void Update()
    {
        // PRÜFUNG: Wurde das Bild weggegriffen/deaktiviert?
        if (aktuellAktivesBild != null && !aktuellAktivesBild.activeInHierarchy)
        {
            OnBildDeaktiviert();
        }
    }

    public void TriggerNPCError(int index)
    {
        aktuellerNPCIndex = index;
        Invoke("ZeigeVerzoegerteWarning", 10f);
    }

    void ZeigeVerzoegerteWarning()
    {
        WechselBild(npcWarnings[aktuellerNPCIndex]);
        warteAufGrabWarning = true;
    }

    // Diese Logik ersetzt den Button-Click!
    void OnBildDeaktiviert()
    {
        aktuellAktivesBild = null; // Reset, damit wir nicht mehrfach feuern

        if (warteAufGrabWarning)
        {
            warteAufGrabWarning = false;
            errorsGezählt++;
            Debug.Log("Warning weggegriffen! Errors: " + errorsGezählt);
            Invoke("ZeigeSystemError", 10f);
        }
        else if (warteAufGrabSystem)
        {
            warteAufGrabSystem = false;
            errorsGezählt++;
            Debug.Log("System Error weggegriffen! Errors: " + errorsGezählt);
            
            if(errorsGezählt >= 6) Invoke("ZeigeFinalError", 3f);
        }
    }

    void ZeigeSystemError()
    {
        WechselBild(systemErrors[aktuellerNPCIndex]);
        warteAufGrabSystem = true;
    }

    void ZeigeFinalError()
    {
        WechselBild(finalErrorObj);
    }

    void WechselBild(GameObject neuesBild)
    {
        if (neuesBild == null) return;
        errorCanvas.SetActive(true);
        neuesBild.SetActive(true);
        aktuellAktivesBild = neuesBild;
    }

    public void ClickNachHause()
    {
        SceneManager.LoadScene("NachHauseRaum");
    }
}