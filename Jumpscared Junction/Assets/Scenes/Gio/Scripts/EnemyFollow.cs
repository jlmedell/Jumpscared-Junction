using UnityEngine;

public class EnemyFollow : MonoBehaviour
{
    public Transform player;
    public float speed = 3f;
    private Rigidbody rb;

    // -- Jumping Params --------------------
    public float jumpForce = 10f;
    private bool isGrounded;
    public LayerMask groundLayer;
    public float maxJumpableHeight = 2f;
    // -- Wall crawl -----------------------
    public float wallCrawlSpeed = 3f;
    public float wallCrawlDuration = 2f;
    private Vector3 lastDirection;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    void FixedUpdate()
    {
        isGrounded = Physics.Raycast(transform.position, Vector3.down, 1.1f, groundLayer);
        rb.AddForce(Vector3.down * 20f, ForceMode.Acceleration);
        //Direction between enemy and player
        Vector3 direction = (player.position - transform.position).normalized;
        direction.y = 0;
        lastDirection = direction;

        //Chasing speed ------------------------------------
        Vector3 velocity = direction * speed;
        velocity.y = rb.linearVelocity.y;
        rb.linearVelocity = velocity;

        // Wall Check --------------------------------------
        RaycastHit wallHit;

        Vector3 wallRayOrigin = transform.position + (Vector3.down * 1f);
        bool wallAhead = Physics.Raycast(wallRayOrigin, direction, out wallHit, 1.5f, groundLayer);
        bool isTallWall = wallAhead && wallHit.collider.bounds.size.y > maxJumpableHeight;

        //Wall Crawl or Jump
        if (isTallWall)
        {
            Debug.Log("Wall is Tall");
            rb.linearVelocity = Vector3.up * wallCrawlSpeed;

        } else if (wallAhead)
        {
           Debug.Log("Short as bort");
            // Checking if can jump
            if (isGrounded)
            {
                rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
            
            } else
            {
                velocity.x = 0f;
                velocity.z = 0f;
                rb.linearVelocity = velocity;
            }
        }

        // Make enemy point the player
        if (direction != Vector3.zero)
        {
            Quaternion targetRotation = Quaternion.LookRotation(direction);
            rb.MoveRotation(targetRotation);
        }
    }

    void OnDrawGizmos()
    {
        // ground check ray
        Gizmos.color = Color.green;
        Gizmos.DrawLine(transform.position, transform.position + Vector3.down * 1.1f);

        // wall check ray
        Gizmos.color = Color.blue;
        Vector3 wallRayOrigin = transform.position + Vector3.down * 0.9f;
        Gizmos.DrawLine(wallRayOrigin, wallRayOrigin + transform.forward * 1.5f);
    }
}