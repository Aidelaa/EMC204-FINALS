using System.Collections;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    [Header("Spawner Settings")]
    public GameObject enemyPrefab;
    public GameObject groundPlane;   // assign your plane in the Inspector
    public float respawnDelay = 5f;

    private GameObject currentEnemy;

    void Start()
    {
        SpawnEnemy();
    }

    void SpawnEnemy()
    {
        // pick a random point on the plane
        Vector3 spawnPos = GetRandomPositionOnPlane();
        currentEnemy = Instantiate(enemyPrefab, spawnPos, Quaternion.identity);

        // if it has our EnemyHole script, wire it up:
        var enemyScript = currentEnemy.GetComponent<EnemyHole>();
        if (enemyScript != null)
        {
            enemyScript.spawner = this;
            enemyScript.plane = groundPlane.transform;
        }
    }

    Vector3 GetRandomPositionOnPlane()
    {
        if (groundPlane == null)
        {
            Debug.LogWarning("EnemySpawner: groundPlane is not assigned!");
            return Vector3.zero;
        }

        var rend = groundPlane.GetComponent<Renderer>();
        if (rend == null)
        {
            Debug.LogWarning("EnemySpawner: groundPlane has no Renderer!");
            return Vector3.zero;
        }

        Vector3 size = rend.bounds.size;
        Vector3 center = rend.bounds.center;
        float halfW = size.x * 0.5f;
        float halfD = size.z * 0.5f;

        float x = Random.Range(center.x - halfW, center.x + halfW);
        float z = Random.Range(center.z - halfD, center.z + halfD);

        // *** Spawn flush on top of the plane ***
        float y = rend.bounds.max.y;

        return new Vector3(x, y, z);
    }

    // Called by EnemyHole when it is destroyed
    public void OnEnemyDeath()
    {
        StartCoroutine(RespawnAfterDelay());
    }

    IEnumerator RespawnAfterDelay()
    {
        yield return new WaitForSeconds(respawnDelay);
        SpawnEnemy();
    }
}
