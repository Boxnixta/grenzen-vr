using UnityEngine;
using UnityEngine.SceneManagement;

public class CubeSceneChanger : MonoBehaviour
{
    private void OnTriggerStay(Collider other)
    {
        // Checken ob Left oder Right Controller im Trigger ist
        if (other.CompareTag("LeftController") || other.CompareTag("RightController"))
        {
            // Szene wechseln (Index 1 wie in deinem SceneChanger)
            SceneManager.LoadScene(1);
            Debug.Log("🚀 Scene gewechselt!");
        }
    }
}