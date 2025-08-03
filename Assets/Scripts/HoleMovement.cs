using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class HoleMovement : MonoBehaviour
{
    public float moveSpeed = 5f;  // Speed of movement
    private Rigidbody rb;
    private Vector3 inputDirection;

    // References to the plane (set this in the Inspector)
    public GameObject plane;  // The plane GameObject
    private Vector3 minBounds;
    private Vector3 maxBounds;

    void Awake()
    {
        rb = GetComponent<Rigidbody>();
        rb.interpolation = RigidbodyInterpolation.Interpolate; // For smoother physics updates
        rb.isKinematic = true;  // Ensure the Rigidbody is kinematic for controlled movement
    }

    void Start()
    {
        // Set the initial bounds based on the plane's size
        UpdateBounds();
    }

    void Update()
    {
        // Get input every frame for responsiveness
        float h = Input.GetAxisRaw("Horizontal");
        float v = Input.GetAxisRaw("Vertical");

        // Normalize input direction to ensure consistent movement speed in all directions
        inputDirection = new Vector3(h, 0f, v).normalized;
    }

    void FixedUpdate()
    {
        // Calculate the target position based on input direction
        Vector3 targetPos = rb.position + inputDirection * moveSpeed * Time.fixedDeltaTime;

        // Update the bounds in case the plane changes size
        UpdateBounds();

        // Clamp the position to stay within bounds (X, Z only)
        targetPos.x = Mathf.Clamp(targetPos.x, minBounds.x, maxBounds.x);
        targetPos.z = Mathf.Clamp(targetPos.z, minBounds.z, maxBounds.z);

        // Keep the Y position fixed (no vertical movement)
        targetPos.y = rb.position.y;

        // Move the Rigidbody to the target position using MovePosition (no velocity manipulation)
        rb.MovePosition(targetPos);
    }

    // Method to update the dynamic bounds based on the plane's size
    void UpdateBounds()
    {
        if (plane != null)
        {
            // Get the plane's size (assumes it's a square or rectangular plane)
            Renderer planeRenderer = plane.GetComponent<Renderer>();
            if (planeRenderer != null)
            {
                // Get the size of the plane based on the renderer bounds
                Vector3 planeSize = planeRenderer.bounds.size;

                // Update min and max bounds
                minBounds = new Vector3(plane.transform.position.x - planeSize.x / 2, 0f, plane.transform.position.z - planeSize.z / 2);
                maxBounds = new Vector3(plane.transform.position.x + planeSize.x / 2, 0f, plane.transform.position.z + planeSize.z / 2);
            }
        }
    }

    // Optional: Detect collisions with walls if needed for additional custom logic
    void OnCollisionStay(Collision collision)
    {
        if (collision.gameObject.CompareTag("Wall"))
        {
            // Handle collision with the wall, like stopping or adjusting movement if necessary
            Debug.Log("Hole colliding with a wall.");
        }
    }
}
