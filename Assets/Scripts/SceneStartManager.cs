using UnityEngine;

public class SceneStartManager : MonoBehaviour
{
    void Awake() // Awake ist noch früher als Start!
    {
        GameObject player = GameObject.FindWithTag("Player");
        if (player != null)
        {
            // Falls du einen CharacterController hast, MUSS der kurz aus
            CharacterController cc = player.GetComponent<CharacterController>();
            if (cc != null) cc.enabled = false;

            // Teleport auf 0,0,0
            player.transform.position = Vector3.zero;
            player.transform.rotation = Quaternion.identity;

            // Kurz warten und wieder an
            if (cc != null) cc.enabled = true;
            
            Debug.Log("Player hart auf 0 gesetzt.");
        }
    }
}