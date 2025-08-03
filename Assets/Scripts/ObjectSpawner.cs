using UnityEngine;
using System.Collections;

public class ObjectSpawner : MonoBehaviour
{
    public GameObject objectPrefab;
    public GameObject groundPlane; // assign your plane here in the Inspector

    public float spawnInterval = 2f; // time in seconds between spawns

    // Array of possible scales: 0.5, 1, 1.5, 2, 2.5, etc.
    private float[] scaleValues = { 0.5f, 1f, 1.5f, 2f, 2.5f, 3f, 3.5f, 4f };

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
        if (groundPlane == null) return;

        Renderer groundRenderer = groundPlane.GetComponent<Renderer>();
        if (groundRenderer == null) return;

        Vector3 planeSize = groundRenderer.bounds.size;
        Vector3 planeCenter = groundRenderer.bounds.center;

        float x = Random.Range(planeCenter.x - planeSize.x / 2, planeCenter.x + planeSize.x / 2);
        float z = Random.Range(planeCenter.z - planeSize.z / 2, planeCenter.z + planeSize.z / 2);

        Vector3 spawnPosition = new Vector3(x, 0.5f, z);
        GameObject obj = Instantiate(objectPrefab, spawnPosition, Quaternion.identity);

        // --- Randomly select a scale from the predefined array ---
        float randomScale = scaleValues[Random.Range(0, scaleValues.Length)];
        obj.transform.localScale = new Vector3(randomScale, randomScale, randomScale);
    }
}