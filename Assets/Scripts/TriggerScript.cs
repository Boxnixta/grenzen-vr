using UnityEngine;

public class AreaTrigger : MonoBehaviour
{
    public CharacterSequencer sequencer;
    public int myNumber; // 0 für die große, 1 für die mittlere, 2 für die kleine

    void OnTriggerEnter(Collider other)
    {
        // Prüfen, ob der Spieler (Main Camera oder XR Origin) reingelaufen ist
        if (other.CompareTag("MainCamera") || other.GetComponent<Camera>() != null)
        {
            sequencer.TriggerNextCharacter(myNumber);
        }
    }
}