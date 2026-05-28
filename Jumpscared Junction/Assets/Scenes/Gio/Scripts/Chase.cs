// using UnityEngine;

// public class EnemyFollow : MonoBehaviour
// {
//     public Transform player;
//     public float speed = 3f;
//     private Rigidbody rb;
//     public float jumpForce = 5f;
//     public float jumpCooldown = 2f;
//     private float jumpTimer;
//     private bool isGrounded;
//     public LayerMask groundLayer;

//     void Start()
//     {
//         rb = GetComponent<Rigidbody>();
//     }

//     // Needed for Physics animatiosn apparently 
//     void FixedUpdate()
//     {
//         isGrounded = Physics.Raycast(transform.position, Vector3.down, 2f, groundLayer);

//         Vector3 direction = (player.position - transform.position).normalized;
//         direction.y = 0;

//         Vector3 velocity = direction * speed;
//         velocity.y = rb.linearVelocity.y;
//         rb.linearVelocity = velocity;

//         jumpTimer -= Time.fixedDeltaTime;
//         if (isGrounded && jumpTimer <= 0f) // When the enemy hasnt jumped
//         {
//             Vector3 rayOrigin = transform.position + Vector3.down * 0.8f;
//             Debug.DrawRay(rayOrigin, direction * 1.5f, Color.red);

//             // When raycast touches
//             if (Physics.Raycast(rayOrigin, direction, 1.5f, groundLayer))
//             {
//                 rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
//                 jumpTimer = jumpCooldown;
//             }
//         }
//     }
// }