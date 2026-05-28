using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerControler : MonoBehaviour
{

    public Rigidbody rb;
    public void Move(InputAction.CallbackContext context)
    {
        Vector2 movementValue = context.ReadValue<Vector2>();
        Debug.Log(movementValue);
        rb.linearVelocity = new Vector3 (movementValue.x, movementValue.y, 0);
    }

    public void Start()
    {
        rb = this.GetComponent<Rigidbody>();
    }
}
