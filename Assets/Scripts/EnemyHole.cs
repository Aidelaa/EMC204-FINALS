using UnityEngine;

[RequireComponent(typeof(Collider))]
public class EnemyHole : MonoBehaviour
{
    [HideInInspector] public EnemySpawner spawner;  // set by spawner
    [HideInInspector] public Transform plane;    // set by spawner

    [Header("Movement Settings")]
    public float moveSpeed = 2f;
    public float changeDirInterval = 2f;

    [Header("Swallow Settings")]
    public float growAmount = 0.2f;  // how much it grows when it eats

    Vector3 moveDirection;
    float nextDirChangeTime;

    void Start()
    {
        ChooseNewDirection();
        nextDirChangeTime = Time.time + changeDirInterval;
        GetComponent<Collider>().isTrigger = true;

        // Make sure the Y scale is set to -0.1 to keep it flat
        if (transform.localScale.y != -0.1f)
        {
            transform.localScale = new Vector3(transform.localScale.x, -0.1f, transform.localScale.z);
        }
    }

    void Update()
    {
        // 1) Chase edible food if any
        Transform target = GetClosestSwallowableFood();
        if (target != null)
        {
            MoveTowards(target.position);
            if (Vector3.Distance(transform.position, target.position) < 0.5f)
            {
                Destroy(target.gameObject);
                Grow();
            }
        }
        else
        {
            // 2) Wander if no target
            transform.position += moveDirection * moveSpeed * Time.deltaTime;
        }

        // 3) Clamp inside plane each frame
        ClampToPlane();

        // 4) Occasionally pick a new random direction
        if (Time.time >= nextDirChangeTime)
        {
            ChooseNewDirection();
            nextDirChangeTime = Time.time + changeDirInterval + Random.Range(-1f, 1f);
        }
    }

    void ChooseNewDirection()
    {
        float ang = Random.Range(0f, 360f) * Mathf.Deg2Rad;
        moveDirection = new Vector3(Mathf.Cos(ang), 0f, Mathf.Sin(ang)).normalized;
    }

    Transform GetClosestSwallowableFood()
    {
        // Now the range scales directly with the enemy size
        float range = transform.localScale.x;  // swallowing range is the same as the hole's current size
        Collider[] hits = Physics.OverlapSphere(transform.position, range);

        Transform best = null;
        float bestD = Mathf.Infinity;
        float mySize = transform.localScale.x;

        foreach (var col in hits)
        {
            if (!col.CompareTag("Pickup")) continue;
            float d = Vector3.Distance(transform.position, col.transform.position);
            if (d < bestD && col.transform.localScale.x < mySize)
            {
                bestD = d;
                best = col.transform;
            }
        }
        return best;
    }

    void MoveTowards(Vector3 pos)
    {
        Vector3 dir = (pos - transform.position).normalized;
        transform.position += dir * moveSpeed * Time.deltaTime;
    }

    void ClampToPlane()
    {
        if (plane == null) return;
        var rend = plane.GetComponent<Renderer>();
        if (rend == null) return;

        Vector3 c = rend.bounds.center;
        Vector3 sz = rend.bounds.size;
        float halfX = sz.x * 0.5f;
        float halfZ = sz.z * 0.5f;
        float yTop = rend.bounds.max.y;

        Vector3 p = transform.position;
        p.x = Mathf.Clamp(p.x, c.x - halfX, c.x + halfX);
        p.z = Mathf.Clamp(p.z, c.z - halfZ, c.z + halfZ);
        p.y = yTop;  // keep the enemy at the top of the plane
        transform.position = p;
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("PlayerHole") &&
            other.transform.localScale.x < transform.localScale.x)
        {
            Destroy(other.gameObject);
            Grow();
        }
    }

    void Grow()
    {
        // Get current scale, ensuring we only increase X and Z and keep Y fixed
        Vector3 s = transform.localScale;

        // Only increase X and Z, keeping Y locked at -0.1
        s.x += growAmount;
        s.z += growAmount;
        s.y = -0.1f;  // Lock Y scale to -0.1 to keep it flat

        // Apply the scale back to the object
        transform.localScale = s;
    }

    void OnDestroy()
    {
        if (spawner != null)
            spawner.OnEnemyDeath();
    }
}
