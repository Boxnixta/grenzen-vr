using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public class ErrorStackManager : MonoBehaviour
{
    public List<GameObject> errorPages; // Hier ziehst du alle Bilder rein
    private int currentIndex;

    void Start()
    {
        // Sicherstellen, dass alle Bilder an sind
        foreach (var page in errorPages) page.SetActive(true);
        
        // Wir fangen beim vordersten an (das letzte in der Hierarchy Liste)
        currentIndex = errorPages.Count - 1;
    }

    public void OnStackClicked()
    {
        if (currentIndex >= 0)
        {
            // Das oberste Bild ausschalten
            errorPages[currentIndex].SetActive(false);
            currentIndex--;

            // Wenn kein Bild mehr da ist -> Spiel beenden
            if (currentIndex < 0)
            {
                FinishGame();
            }
        }
    }

    void FinishGame()
    {
        Debug.Log("Alle Errors weg - Spiel vorbei!");
        // Hier kannst du z.B. zurück zum Hauptmenü oder Application.Quit()
        // Da du am Mac/PC testest:
        #if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
        #else
            Application.Quit();
        #endif
    }
}