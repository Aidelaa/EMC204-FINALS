using UnityEngine;
using System.Collections;

[RequireComponent(typeof(Collider))]
public class HoleSwallow : MonoBehaviour
{
    [Tooltip("How much the hole grows each time it swallows something")]
    public float growAmount = 0.2f;
    [Tooltip("Units per second at which objects fall into the hole")]
    public float fallSpeed = 2f;

    private Transform holeParent;
    private ScoreManager scoreManager;

    void Start()
    {
        holeParent = transform.parent;
        scoreManager = Object.FindAnyObjectByType<ScoreManager>();  // <- updated here
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
        Destroy(target);
        GrowHole();
    }

    void GrowHole()
    {
        Vector3 s = holeParent.localScale;
        s.x += growAmount;
        s.z += growAmount;
        holeParent.localScale = new Vector3(s.x, 1f, s.z);

        if (scoreManager != null)
            scoreManager.AddScore(1);
    }
}
