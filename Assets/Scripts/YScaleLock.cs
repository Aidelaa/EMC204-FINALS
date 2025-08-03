using UnityEngine;

public class LockYScale : MonoBehaviour
{
    public float lockedYScale = -0.1f;

    void Update()
    {
        Vector3 currentScale = transform.localScale;
        transform.localScale = new Vector3(currentScale.x, lockedYScale, currentScale.z);
    }
}
