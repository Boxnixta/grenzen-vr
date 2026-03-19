using UnityEngine;

public class PointToTarget : MonoBehaviour
{
    [Header("Einstellungen")]
    public Transform target;       // Die rote Scheibe (echtes Ziel am Kotti)
    public Transform pfeilModell; // Dein Sketchfab-Pfeil (Child)
    public float radius = 0.05f;   

    void Update()
    {
        if (target == null || pfeilModell == null) return;

        // 1. Die Position des Ziels im "Handy-Raum" berechnen
        // Das Handy ist hier der Nullpunkt. Wenn das Ziel 100m vor dir ist,
        // liefert das einen Vektor, der 100m in Blickrichtung zeigt.
        Vector3 localTargetPos = transform.InverseTransformPoint(target.position);

        // 2. Wir schauen nur auf X und Y (dein Display)
        // WICHTIG: Da Blau (Z) bei dir hinten rauszeigt, ist Z die Tiefe.
        // Wenn das Ziel hinter dir ist, wird localTargetPos.z negativ.
        // Wir nehmen aber X und Y für die Position auf dem Display.
        Vector2 direction = new Vector2(localTargetPos.x, localTargetPos.y);

        // 3. Falls das Ziel genau hinter dir ist, würde die Richtung auf dem Display (X/Y)
        // fast Null sein. Wir sorgen dafür, dass er trotzdem am Rand bleibt.
        if (direction.magnitude < 0.001f) 
        {
            // Wenn das Ziel exakt vor/hinter der Z-Achse ist, erzwingen wir eine Richtung
            direction = Vector2.up; 
        }
        direction.Normalize();

        // 4. Position am Rand des Displays
        pfeilModell.localPosition = new Vector3(direction.x * radius, direction.y * radius, 0);

        // 5. Winkel berechnen
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;

        // 6. Rotation anwenden
        // Wenn die Spitze falsch zeigt, probiere: angle, angle - 90, angle + 90 oder angle + 180
        pfeilModell.localRotation = Quaternion.Euler(0, 0, angle + 180f);
    }
}