using UnityEngine;

public class CharacterSequencer : MonoBehaviour
{
    public GameObject[] characters; // Deine 3 NPCs in der richtigen Reihenfolge
    private int currentStep = 0;
    private bool initialized = false;

    void Start()
    {
        // Wir starten NPC 0 nach einer kurzen Verzögerung, damit die Szene laden kann
        Invoke("StartFirstNPC", 0.5f);
    }

    void StartFirstNPC()
    {
        if (!initialized) ActivateNPC(0);
    }

    // Diese Funktion repariert deine Fehlermeldungen (KottiTrigger & Co)
    public void TriggerNextCharacter(int index)
    {
        ActivateNPC(index);
    }

    public void ActivateNPC(int index)
    {
        if (index < 0 || index >= characters.Length) return;

        initialized = true;
        currentStep = index;

        // Den Ziel-NPC aktivieren und ihm den Marschbefehl geben
        GameObject npc = characters[index];
        if (npc != null)
        {
            npc.SetActive(true); 
            NPCFollow follow = npc.GetComponent<NPCFollow>();
            if (follow != null)
            {
                follow.isFollowing = true;
                Debug.Log("SEQUENCER: " + npc.name + " (Index " + index + ") läuft jetzt los!");
            }
        }
    }

    // Wird vom NPCTalkController aufgerufen, wenn die Audio zu Ende ist
    public void OnNPCFinishedTalking()
    {
        int next = currentStep + 1;
        if (next < characters.Length)
        {
            Debug.Log("SEQUENCER: NPC " + currentStep + " ist fertig. Rufe NPC " + next);
            ActivateNPC(next);
        }
        else
        {
            Debug.Log("SEQUENCER: Alle Charaktere haben gesprochen. Ende der Kette.");
        }
    }
}