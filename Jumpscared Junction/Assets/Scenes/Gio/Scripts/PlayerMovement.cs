using UnityEngine;
using UnityEngine.InputSystem;
using System.Collections;

public class PlayerMovement : MonoBehaviour
{
    //Speeds---------------------------------------
    public float moveSpeed = 5f;
    public float sprintSpeed = 10f;
    public float sprintBuildUpPerTap = 10f;
    public float sprintDecaySpeed = 0.00000000000000000000000000000000000000000000000000000000000001f;
    public float tapWindow = 15f;

    // Jump -----------------------------------------
    public float gravity = -9.81f * 2;
    public float jumpHeight = 3f;

    /// Ground Handling------------------------------
    public Transform groundCheck;
    public float groundDistance = 0.4f;
    public LayerMask groundMask;

    //Sliding ---------------------------------------
    public float slowSlide = 3f;
    public float fastSlide = 12f;
    public float slideDuration = 3f;

    // Mouse look for Y-axis only
    public float mouseSensitivity = 2f;
    float xRotation = 0f; // vertical (up/down) - for camera
    float yRotation = 0f; // horizontal (left/right) - for body

    [SerializeField] Transform cameraTransform;


    // Player Input component variables--------------
    private Vector2 moveInput;
    private Vector2 lookInput;

    // Private fields -------------------------------
    private CharacterController controller;
    private Vector3 velocity;
    private bool isGrounded;
    private float currentSpeed;
    private float sprintProgress = 0f;
    private float lastTapTime = 0f;
    private bool slideCoroutineRunning = false;
    private float slideSpeed;


    void Start()
    {
        controller = GetComponent<CharacterController>();
        currentSpeed = moveSpeed;
    }

    void Update()
    {
        HandleGroundCheck();
        HandleSprintDecay();
        HandleBodyRotation();
        HandleMovement();
        ApplyGravity();
    }

    void HandleGroundCheck()
    {
        if (groundCheck == null) return;
        
        isGrounded = Physics.CheckSphere(groundCheck.position, groundDistance, groundMask);

        if (isGrounded && velocity.y < 0)
        {
            velocity.y = -2f;
        }
    }

    public void OnMove(InputAction.CallbackContext context)
    {
        moveInput = context.ReadValue<Vector2>();
    }

    public void OnLook(InputAction.CallbackContext context)
    {
        lookInput = context.ReadValue<Vector2>();
    }

    void HandleBodyRotation()
    {
        float mouseX = lookInput.x * mouseSensitivity;
        float mouseY = lookInput.y * mouseSensitivity;

        yRotation += mouseX;
        xRotation -= mouseY; // minus so moving mouse up looks up
        xRotation = Mathf.Clamp(xRotation, -80f, 80f); // stops you flipping upside down

        // Body rotates left/right
        transform.localRotation = Quaternion.Euler(0f, yRotation, 0f);

        // Camera rotates up/down
        cameraTransform.localRotation = Quaternion.Euler(xRotation, 0f, 0f);
    }

    public void OnSprint(InputAction.CallbackContext context)
    {
            float timeSinceLastTap = Time.time - lastTapTime;
            if (timeSinceLastTap < tapWindow)
            {
                sprintProgress += sprintBuildUpPerTap;
                sprintProgress = Mathf.Clamp01(sprintProgress);
            }
            lastTapTime = Time.time;
    }
    
    void HandleSprintDecay()
    {
        sprintProgress -= sprintDecaySpeed * Time.deltaTime;
        sprintProgress = Mathf.Clamp01(sprintProgress);
        currentSpeed = Mathf.Lerp(moveSpeed, sprintSpeed, sprintProgress);
    }

    void HandleMovement()
    {
        Vector3 move = transform.right * moveInput.x + transform.forward * moveInput.y;
        controller.Move(move * currentSpeed * Time.deltaTime);
    }

    public void OnJump(InputAction.CallbackContext context)
    {
        if (isGrounded)
        {
            velocity.y = Mathf.Sqrt(jumpHeight * -2f * gravity);
        }
    }

    void startNewSlide()
    {
        Vector3 startPosition = controller.transform.position;
        Vector3 endPosition = startPosition + transform.TransformDirection(Vector3.forward * slideSpeed);
        if(slideCoroutineRunning == false)
        {
            StartCoroutine(SlideCoroutine(startPosition, endPosition, slideDuration));
        }
    }

    IEnumerator SlideCoroutine(Vector3 start, Vector3 end, float duration)
    {
        slideCoroutineRunning = true;
        PlayerInput pi = GetComponent<PlayerInput>();
        pi.actions.FindAction("Move").Disable();
        
        float timeElapsed = 0;
        while (timeElapsed < duration)
        {
            float t = timeElapsed / duration;
            controller.Move(transform.TransformDirection(Vector3.forward) * slideSpeed * Time.deltaTime);
            timeElapsed += Time.deltaTime;

            yield return null;
        }
    
        slideCoroutineRunning = false;
        pi.actions.FindAction("Move").Enable();
        StopCoroutine(SlideCoroutine(start, end, duration));

    }


    public void OnSlide(InputAction.CallbackContext context)
    {
        if(isGrounded)
        {
            if(currentSpeed <= moveSpeed)
            {
                slideSpeed = slowSlide;
            }
            else
            {
                slideSpeed = fastSlide;
            }
            startNewSlide();
        }
    }

    void ApplyGravity()
    {
        velocity.y += gravity * Time.deltaTime;
        velocity.y = Mathf.Max(velocity.y, -50f);
        controller.Move(velocity * Time.deltaTime);
    }
}
