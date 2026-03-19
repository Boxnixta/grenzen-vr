using UnityEngine;

public class AvatarHutFix : MonoBehaviour
{
    public Animator npcAnimator; // Hier den Charakter reinziehen
    public Vector3 posOffset;
    public Vector3 rotOffset;

    void LateUpdate()
    {
        if (npcAnimator != null && npcAnimator.isHuman)
        {
            // Holt sich die Position des Kopfes direkt aus dem Animator-System
            Transform headTransform = npcAnimator.GetBoneTransform(HumanBodyBones.Head);

            if (headTransform != null)
            {
                transform.position = headTransform.position;
                transform.rotation = headTransform.rotation;

                // Korrektur für den verschobenen Pivot-Point
                transform.Translate(posOffset, Space.Self);
                transform.Rotate(rotOffset, Space.Self);
            }
        }
    }
}