using UnityEngine;

public class ParentToBone : MonoBehaviour
{
    public Animator npcAnimator; 
    public Vector3 positionsOffset; 
    public Vector3 rotationsOffset; 

    private Transform headBone;

    void Start()
    {
        if (npcAnimator != null)
        {
            // Wir holen uns den Head-Bone. 
            // Wichtig: Der NPC muss im Import auf "Humanoid" stehen!
            headBone = npcAnimator.GetBoneTransform(HumanBodyBones.Head);

            if (headBone == null)
            {
                Debug.LogError("Kopf-Knochen nicht gefunden! Ist der NPC wirklich auf Humanoid gestellt?");
            }
        }
    }

    // Wir nutzen LateUpdate, damit der Hut sich erst bewegt, 
    // NACHDEM die Animation den Kopf an die neue Position gebracht hat.
    void LateUpdate()
    {
        if (headBone != null)
        {
            // Position setzen
            transform.position = headBone.position + headBone.TransformDirection(positionsOffset);
            
            // Rotation setzen
            transform.rotation = headBone.rotation * Quaternion.Euler(rotationsOffset);
        }
    }
}