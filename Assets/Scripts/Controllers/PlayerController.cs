using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    private PlayerInputAction playerInpputAction;
    private InputAction movement;
    private Transform cameraTransform;
    private Camera mainCamera;
    private GameObject cameraMountPoint;
    // Only needed if I wanna use the ray out of the Controller
    //private BoardController boardController;

    //horizontal motion
    [SerializeField]
    private float maxSpeed = 5f;
    private float currentSpeed;
    [SerializeField]
    private float acceleration = 10f;
    [SerializeField]
    private float damping = 15f;

    //vertical motion - zooming
    [SerializeField]
    private float stepsize = 2f;
    [SerializeField]
    private float zoomDampening = 7.5f;
    [SerializeField]
    private float minHeight = 5f;
    [SerializeField]
    private float maxHeight = 50f;
    //[SerializeField]
    //private float zoomSpeed = 2f;

    //WASD movement
    [SerializeField]
    private bool useWASDMovement = true;

    //screen edge motion
    [SerializeField]
    [Range(0f, 0.1f)]
    private float edgeTolerance = 0.05f;
    [SerializeField]
    private bool useScreenEdge = true;

    //value set in various functions
    //used to update the position of the camera base object.
    private Vector3 targetPosition;

    private float zoomHeight;
    //use to track and maintain velocity w/o a rigidbody
    private Vector3 horizontalVelocity;
    private Vector3 lastPosition;

    //track where the dragging action started
    Vector3 startDrag;

    private void Awake()
    {
        mainCamera = Camera.main;
        playerInpputAction = new PlayerInputAction();
        cameraTransform = mainCamera.transform;
    }

	private void Start()
	{
        //if (IsLocalPlayer)
        //{
            mainCamera.transform.GetComponent<CameraController>().AttachCameraToCameraMountPoint(transform.GetChild(0));
        //}
    }

	private void OnEnable()
	{
        zoomHeight = cameraTransform.localPosition.y;
        cameraTransform.LookAt(this.transform);

        lastPosition = this.transform.position;
        movement = playerInpputAction.PlayerCamera.Movement;
        playerInpputAction.PlayerCamera.ZoomCamera.performed += ZoomCamera;
        playerInpputAction.PlayerCamera.Enable();
	}

	private void OnDisable()
	{
        playerInpputAction.PlayerCamera.ZoomCamera.performed -= ZoomCamera;
        playerInpputAction.Disable();
	}

	private void Update()
	{
        //if (!IsLocalPlayer) return;

		if (useWASDMovement)
		{
            GetKeyboardMovement();
		}

		if (useScreenEdge)
		{
            CheckMouseAtScreenEdge();
		}

        DragCamera();

        UpdateVelocity();
        UpdateCameraPosition();
        UpdateBasePosition();
	}

	private void UpdateVelocity()
	{
        horizontalVelocity = (this.transform.position - lastPosition) / Time.deltaTime;
        horizontalVelocity.y = 0;
        lastPosition = this.transform.position;
	}

    private void GetKeyboardMovement()
	{
        Vector3 inputValue = movement.ReadValue<Vector2>().x * GetCameraRight() + movement.ReadValue<Vector2>().y * GetCameraForward();

        inputValue = inputValue.normalized;

		if (inputValue.sqrMagnitude > 0.1f)
		{
            targetPosition += inputValue;
		}
	}

    private Vector3 GetCameraRight()
	{
        Vector3 right = cameraTransform.right;
        right.y = 0;
        return right;
	}

    private Vector3 GetCameraForward()
	{
        Vector3 forward = cameraTransform.up;
        forward.y = 0;
        return forward;
	}

    private void UpdateBasePosition()
	{
		if (targetPosition.sqrMagnitude > 0.1f)
		{
            currentSpeed = Mathf.Lerp(currentSpeed, maxSpeed, Time.deltaTime * acceleration);
            transform.position += targetPosition * currentSpeed * Time.deltaTime;
		}
        else
		{
            horizontalVelocity = Vector3.Lerp(horizontalVelocity, Vector3.zero, Time.deltaTime * damping);
            transform.position += horizontalVelocity * Time.deltaTime;
		}

        targetPosition = Vector3.zero;
	}

    private void ZoomCamera(InputAction.CallbackContext inputValue)
    {
        float value = -inputValue.ReadValue<Vector2>().y / 100f;

		if (Mathf.Abs(value) > 0.1f)
		{
            zoomHeight = cameraTransform.localPosition.y + value * stepsize;

			if (zoomHeight < minHeight)
			{
                zoomHeight = minHeight;
			}

			if (zoomHeight > maxHeight)
			{
                zoomHeight = maxHeight;
			}
		}
    }

    private void UpdateCameraPosition()
	{
        Vector3 zoomTarget = new Vector3(cameraTransform.localPosition.x, zoomHeight, cameraTransform.localPosition.z);
        //zoomTarget -= zoomSpeed * (zoomHeight - cameraTransform.localPosition.y) * Vector3.forward;

        cameraTransform.localPosition = Vector3.Lerp(cameraTransform.localPosition, zoomTarget, Time.deltaTime * zoomDampening);
        cameraTransform.LookAt(this.transform);
    }

    private void CheckMouseAtScreenEdge()
	{
        Vector2 mousePosition = Mouse.current.position.ReadValue();
        Vector3 moveDirection = Vector3.zero;

		if (mousePosition.x < edgeTolerance * Screen.width)
		{
            moveDirection -= GetCameraRight();
		}
		else if (mousePosition.x > (1f - edgeTolerance) * Screen.width)
		{
            moveDirection += GetCameraRight();
		}

		if (mousePosition.y < edgeTolerance * Screen.height)
		{
            moveDirection -= GetCameraForward();
		}
		else if (mousePosition.y > (1f - edgeTolerance) * Screen.height)
		{
            moveDirection += GetCameraForward();
		}

        targetPosition += moveDirection;
	}

    private void DragCamera()
	{
        if (!Mouse.current.rightButton.isPressed)
		{
            return;
		}

        Plane plane = new Plane(Vector3.up, Vector3.zero);

        //Would be nice to only have one ray
        //Need to check if there is a way to access it
        //Ray ray = boardController.GetRay();
        Ray ray = mainCamera.ScreenPointToRay(Input.mousePosition);

        if (plane.Raycast(ray, out float distance))
		{
			if (Mouse.current.rightButton.wasPressedThisFrame)
			{
                startDrag = ray.GetPoint(distance);
			}
			else
			{
                targetPosition += startDrag - ray.GetPoint(distance);
			}
        }
	}
}
