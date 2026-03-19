using UnityEngine;

public class StickToChest : MonoBehaviour {
    public SkinnedMeshRenderer targetMesh;
    
    void LateUpdate() {
        if (targetMesh != null) {
            // Die Sphere nutzt die Position des Zentrums vom T-Shirt-Mesh
            transform.position = targetMesh.bounds.center + new Vector3(0, 0.12f, 0); 
        }
    }
}