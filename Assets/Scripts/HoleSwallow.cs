using UnityEngine;
using System.Collections;

[RequireComponent(typeof(Collider))]
public class HoleSwallow : MonoBehaviour
{
    [Tooltip("Default growth if no ObjectGrowth is found")]
    public float defaultGrowAmount = 0.2f;

    [Tooltip("Units per second at which objects fall into the hole")]
    public float fallSpeed = 2f;

    private Transform holeParent;
    private ScoreManager scoreManager;

    void Start()
    {
        holeParent = transform.parent;
        scoreManager = Object.FindAnyObjectByType<ScoreManager>();
        GetComponent<Collider>().isTrigger = true;
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.transform == holeParent) return;

        float holeSize = holeParent.localScale.x;

        if (other.CompareTag("Pickup") || other.CompareTag("EnemyHole"))
        {
            float targetSize = other.transform.localScale.x;
            if (targetSize < holeSize)
                StartCoroutine(SmoothFallAndConsume(other.gameObject));
        }
    }

    IEnumerator SmoothFallAndConsume(GameObject target)
    {
        Collider col = target.GetComponent<Collider>();
        if (col != null) col.enabled = false;
        Rigidbody rb = target.GetComponent<Rigidbody>();
        if (rb != null) rb.isKinematic = true;

        Vector3 startPos = target.transform.position;
        Vector3 targetPos = new Vector3(
            holeParent.position.x,
            holeParent.position.y - 1f,
            holeParent.position.z
        );

        float distance = Vector3.Distance(startPos, targetPos);
        float duration = distance / fallSpeed;
        float elapsed = 0f;

        while (elapsed < duration)
        {
            target.transform.position = Vector3.Lerp(startPos, targetPos, elapsed / duration);
            elapsed += Time.deltaTime;
            yield return null;
        }

        target.transform.position = targetPos;

        float growAmount = GetGrowthFromObject(target);
        GrowHoleBasedOnObject(target); // ✅ Move this BEFORE destroy
        Destroy(target);
    }


    float GetGrowthFromObject(GameObject obj)
    {
        ObjectGrowth growth = obj.GetComponent<ObjectGrowth>();
        if (growth != null)
        {
            return growth.growthAmount;
        }
        return defaultGrowAmount;
    }

    void GrowHoleBasedOnObject(GameObject swallowedObject)
    {
        float objectSize = swallowedObject.transform.lossyScale.x;

        float growthAmount = 0f;
        if (objectSize <= 0.2f)
            growthAmount = 0.05f;
        else if (objectSize <= 0.3f)
            growthAmount = 0.07f;
        else if (objectSize <= 0.5f)
            growthAmount = 0.1f;
        else
            growthAmount = 0.15f;

        Vector3 s = holeParent.localScale;
        s.x += growthAmount;
        s.z += growthAmount;
        holeParent.localScale = new Vector3(s.x, s.y, s.z);

        if (scoreManager != null)
            scoreManager.AddScore(1);

        Debug.Log("Swallowed object size: " + objectSize);
        Debug.Log("Hole scale after growth: " + holeParent.localScale);
    }




    void Awake()
    {
        DontDestroyOnLoad(transform.root.gameObject);
    }

}
