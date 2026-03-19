using UnityEngine;

public class MapFollow : MonoBehaviour
{
    public Transform player; // Hier dein XR Origin reinziehen
    public float height = 500f; // Die Höhe der Kamera

    void LateUpdate()
    {
        if (player != null)
        {
            // Die Kamera folgt der X- und Z-Position des Spielers, bleibt aber auf ihrer Höhe
            transform.position = new Vector3(player.position.x, height, player.position.z);
            
            // Die Kamera schaut immer flach nach unten (keine Rotation vom Spieler übernehmen)
            transform.rotation = Quaternion.Euler(90f, 0f, 0f);
        }
    }
}