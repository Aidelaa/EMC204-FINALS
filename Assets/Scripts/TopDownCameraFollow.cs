using UnityEngine;

public class TopDownCameraFollow : MonoBehaviour
{
    public Transform target;     // The hole to follow
    public Vector3 offset = new Vector3(0f, 20f, 0f);
    public float smoothSpeed = 5f;

    void LateUpdate()
    {
        if (target == null) return;

        Vector3 desiredPosition = target.position + offset;
        Vector3 smoothedPosition = Vector3.Lerp(transform.position, desiredPosition, smoothSpeed * Time.deltaTime);
        transform.position = smoothedPosition;
    }
}