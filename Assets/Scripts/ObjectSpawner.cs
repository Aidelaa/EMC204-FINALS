using UnityEngine;
using System.Collections;

public class ObjectSpawner : MonoBehaviour
{
    public GameObject[] objectPrefabs;
    public GameObject groundPlane;
    public float spawnInterval = 2f;

    // Scale values used for spawning
    private float[] scaleValues = { 0.2f, 0.3f, 0.4f, 0.5f, 1f, 1.5f, 2f, 2.5f, 3f, 3.5f, 4f };

    void Start()
    {
        StartCoroutine(SpawnObjectsPeriodically());
    }

    IEnumerator SpawnObjectsPeriodically()
    {
        while (true)
        {
            SpawnSingleObject();
            yield return new WaitForSeconds(spawnInterval);
        }
    }

    void SpawnSingleObject()
    {
        if (groundPlane == null || objectPrefabs.Length == 0) return;

        Renderer groundRenderer = groundPlane.GetComponent<Renderer>();
        if (groundRenderer == null) return;

        Vector3 planeSize = groundRenderer.bounds.size;
        Vector3 planeCenter = groundRenderer.bounds.center;

        float x = Random.Range(planeCenter.x - planeSize.x / 2, planeCenter.x + planeSize.x / 2);
        float z = Random.Range(planeCenter.z - planeSize.z / 2, planeCenter.z + planeSize.z / 2);

        Vector3 spawnPosition = new Vector3(x, 0.5f, z);

        // Select a random prefab and scale
        GameObject selectedPrefab = objectPrefabs[Random.Range(0, objectPrefabs.Length)];
        float randomScale = scaleValues[Random.Range(0, scaleValues.Length)];

        // Spawn and apply scale
        GameObject obj = Instantiate(selectedPrefab, spawnPosition, Quaternion.identity);
        obj.transform.localScale = Vector3.one * randomScale;

        // Assign growth value
        float growth = GetGrowthValue(randomScale);

        // Add or get ObjectGrowth script and assign value
        ObjectGrowth growthComp = obj.GetComponent<ObjectGrowth>();
        if (growthComp == null)
        {
            growthComp = obj.AddComponent<ObjectGrowth>();
        }
        growthComp.growthAmount = growth;
    }

    float GetGrowthValue(float scale)
    {
        // Define growth logic based on scale
        if (scale <= 0.2f) return 0.05f;
        else if (scale <= 0.3f) return 0.07f;
        else if (scale <= 0.5f) return -0.1f;
        else if (scale <= 1f) return -0.2f;
        else if (scale <= 2f) return -0.3f;
        else return -0.4f; // For very large objects
    }
}
