using UnityEngine;

public class KottiTrigger : MonoBehaviour
{
    public int meinIndex; 
    public NPCFollow npcZuDiesemTrigger; // Hier den NPC im Inspector zuweisen!

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") || other.CompareTag("MainCamera"))
        {
            // 1. Dem NPC SOFORT sagen: Lauf los!
            if (npcZuDiesemTrigger != null)
            {
                npcZuDiesemTrigger.isFollowing = true;
                Debug.Log(npcZuDiesemTrigger.name + " hat den Marschbefehl durch Trigger erhalten!");
            }

            // 2. Den Sequencer informieren für die Stafette
            CharacterSequencer sequencer = Object.FindFirstObjectByType<CharacterSequencer>();
            if (sequencer != null)
            {
                sequencer.TriggerNextCharacter(meinIndex);
            }

            // Trigger ausmachen
            GetComponent<Collider>().enabled = false;
        }
    }
}