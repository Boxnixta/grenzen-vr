using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneChanger : MonoBehaviour
{
    public void StartKottiSpiel()
    {
        // Lädt die Szene mit Index 1 (deine KottiSzene laut Build Profile)
        SceneManager.LoadScene(1);
    }
}