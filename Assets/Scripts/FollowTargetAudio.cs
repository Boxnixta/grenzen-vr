using UnityEngine;

public class FollowTargetAudio : MonoBehaviour
{
    public Transform target;

    void Update()
    {
        if (target != null)
        {
            // Der Sound folgt dem NPC exakt
            transform.position = target.position;
        }
        else
        {
            // Wenn der NPC weg ist, lösch dich selbst
            Destroy(gameObject);
        }
    }
}