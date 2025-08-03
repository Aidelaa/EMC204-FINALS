using UnityEngine;

public class InvisibleWalls : MonoBehaviour
{
    public GameObject groundPlane;  // Assign your ground plane in the Inspector
    public GameObject wallLeft;     // Assign your manual Wall_Left here in the Inspector
    public GameObject wallRight;    // Assign your manual Wall_Right here in the Inspector
    public GameObject wallTop;      // Assign your manual Wall_Top here in the Inspector
    public GameObject wallBottom;   // Assign your manual Wall_Bottom here in the Inspector
    public float wallHeight = 10f;  // Height of the walls (fixed)

    void Start()
    {
        ScaleAndPositionWalls();
    }

    void ScaleAndPositionWalls()
    {
        if (groundPlane == null || wallLeft == null || wallRight == null || wallTop == null || wallBottom == null)
        {
            Debug.LogError("Missing references for groundPlane or walls.");
            return;
        }

        // Get the size of the ground plane
        Renderer groundRenderer = groundPlane.GetComponent<Renderer>();
        if (groundRenderer == null) return;

        Vector3 planeSize = groundRenderer.bounds.size;
        Vector3 planeCenter = groundRenderer.bounds.center;

        // Position and scale the left wall
        wallLeft.transform.localScale = new Vector3(1, wallHeight, planeSize.z); // Width = 1, Height = wallHeight, Depth = planeSize.z
        wallLeft.transform.position = new Vector3(planeCenter.x - planeSize.x / 2, wallHeight / 2, planeCenter.z);

        // Position and scale the right wall
        wallRight.transform.localScale = new Vector3(1, wallHeight, planeSize.z); // Width = 1, Height = wallHeight, Depth = planeSize.z
        wallRight.transform.position = new Vector3(planeCenter.x + planeSize.x / 2, wallHeight / 2, planeCenter.z);

        // Position and scale the top wall
        wallTop.transform.localScale = new Vector3(planeSize.x, wallHeight, 1); // Width = planeSize.x, Height = wallHeight, Depth = 1
        wallTop.transform.position = new Vector3(planeCenter.x, wallHeight / 2, planeCenter.z + planeSize.z / 2);

        // Position and scale the bottom wall
        wallBottom.transform.localScale = new Vector3(planeSize.x, wallHeight, 1); // Width = planeSize.x, Height = wallHeight, Depth = 1
        wallBottom.transform.position = new Vector3(planeCenter.x, wallHeight / 2, planeCenter.z - planeSize.z / 2);
    }
}