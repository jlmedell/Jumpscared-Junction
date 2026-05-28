using UnityEngine;
using UnityEngine.InputSystem;

public class tempPlayerMovement : MonoBehaviour
{
	//Movement settings
	public float walkSpeed = 5.0f;
	public float sprintMultiplier = 1.6f;
	public float gravity = -20.0f;

	//Overcorrect stamina impact
	public float exhaustedSpeedMultiplier = 0.45f;
	public float exhaustedLookMultiplier = 0.55f;

	//Mouse look settings
	public float mouseSensitivity = 0.03f;
	public float maxLookAngle = 90.0f;

	CharacterController characterController;
	Transform playerCameraTransform;

	staminaSystem stamina;

	float cameraPitch = 0f;
	Vector3 verticalVelocity;

	void Start()
	{
		characterController = GetComponent<CharacterController>();
		playerCameraTransform = GetComponentInChildren<Camera>().transform;

		stamina = GetComponent<staminaSystem>();

		Cursor.lockState = CursorLockMode.Locked;
		Cursor.visible = false;
	}

	void Update()
	{
		handleMouseLook();
		handleMovement();
	}

	/*
	Handles mouse movement for camera and player rotation
	*/
	void handleMouseLook()
	{
		if(Mouse.current == null)
		{
			return;
		}

		Vector2 mouseDelta = Mouse.current.delta.ReadValue();

		float lookMultiplier = 1.0f;

		if(stamina != null && stamina.isExhausted)
		{
			lookMultiplier = exhaustedLookMultiplier;
		}

		float mouseX = (mouseDelta.x * mouseSensitivity * lookMultiplier * Time.deltaTime * 100f);
		float mouseY = (mouseDelta.y * mouseSensitivity * lookMultiplier * Time.deltaTime * 100f);

		transform.Rotate(0f, mouseX, 0f);

		cameraPitch -= mouseY;
		cameraPitch = Mathf.Clamp(cameraPitch, -maxLookAngle, maxLookAngle);

		playerCameraTransform.localRotation = Quaternion.Euler(cameraPitch, 0f, 0f);
	}

	/*
	Handles WASD movement, sprint, and gravity
	*/
	void handleMovement()
	{
		if(Keyboard.current == null)
		{
			return;
		}

		float inputX = 0f;
		float inputZ = 0f;

		if(Keyboard.current.aKey.isPressed || Keyboard.current.leftArrowKey.isPressed)
		{
			inputX = -1f;
		}
		else if(Keyboard.current.dKey.isPressed || Keyboard.current.rightArrowKey.isPressed)
		{
			inputX = 1f;
		}

		if(Keyboard.current.sKey.isPressed || Keyboard.current.downArrowKey.isPressed)
		{
			inputZ = -1f;
		}
		else if(Keyboard.current.wKey.isPressed || Keyboard.current.upArrowKey.isPressed)
		{
			inputZ = 1f;
		}

		Vector3 moveDirection = (transform.right * inputX) + (transform.forward * inputZ);
		moveDirection = moveDirection.normalized;

		float currentSpeed = walkSpeed;

		if(stamina != null)
		{
			bool isSprinting = stamina.isSprintingRequested();
			stamina.updateStamina(isSprinting);

			if(isSprinting)
			{
				currentSpeed = (walkSpeed * sprintMultiplier);
			}

			if(stamina.isExhausted)
			{
				currentSpeed = (walkSpeed * exhaustedSpeedMultiplier);
			}
		}
		else
		{
			if(Keyboard.current.leftShiftKey.isPressed)
			{
				currentSpeed = (walkSpeed * sprintMultiplier);
			}
		}

		Vector3 horizontalVelocity = (moveDirection * currentSpeed);

		if(characterController.isGrounded && (verticalVelocity.y < 0f))
		{
			verticalVelocity.y = -2f;
		}

		verticalVelocity.y += (gravity * Time.deltaTime);

		Vector3 finalVelocity = (horizontalVelocity + verticalVelocity);
		characterController.Move(finalVelocity * Time.deltaTime);
	}
}
