using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
public class PlayerMovement : MonoBehaviour
{
    private Rigidbody _rigidbody;
    private Vector3 movementInput;
    public float speed;

    private float mouseX, mouseY;
    private float xRotation = 0f;
    public float mouseSensitivity;
    public Transform cameraTransform;

    public GameObject flechaPrefab;
    public Transform puntoOrigin;
    public float forceFlecha = 10f;

    public LineRenderer trajectory;
    public int resolution = 30;
    public float gravity = -9.81f;

    private bool isAiming;
    private Vector3 launchDirection; 

    void Awake()
    {
        _rigidbody = GetComponent<Rigidbody>();
    }

    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        trajectory.startWidth = 0.05f;
        trajectory.endWidth = 0.05f;
    }

    void Update()
    {
        RotateCamera();

        if (isAiming)
        {
            CalculateDirection();  
            MostrarTrayectory();
        }
        else
        {
            trajectory.positionCount = 0;
        }
    }

    public void ReadDirection(InputAction.CallbackContext context)
    {
        movementInput = context.ReadValue<Vector2>();
    }

    private void FixedUpdate()
    {
        MovePlayer();
    }

    private void MovePlayer()
    {
        Vector3 forward = cameraTransform.forward;
        Vector3 right = cameraTransform.right;
        forward.y = 0f;
        right.y = 0f;
        forward.Normalize();
        right.Normalize();
        Vector3 movement = (forward * movementInput.y + right * movementInput.x) * speed;
        movement.y = _rigidbody.velocity.y;

        _rigidbody.velocity = movement;
    }

    public void ReadMouseInput(InputAction.CallbackContext context)
    {
        Vector2 lookInput = context.ReadValue<Vector2>();
        mouseX = lookInput.x * mouseSensitivity * Time.deltaTime;
        mouseY = lookInput.y * mouseSensitivity * Time.deltaTime;
    }

    private void RotateCamera()
    {
        xRotation = xRotation - mouseY;
        xRotation = Mathf.Clamp(xRotation, -30f, 10f);

        cameraTransform.localRotation = Quaternion.Euler(xRotation, 0f, 0f);
        transform.Rotate(Vector3.up * mouseX);
    }

    public void Aim(InputAction.CallbackContext context)
    {
        if (context.started)
        {
            isAiming = true;
        }
        else if (context.canceled)
        {
            isAiming = false;
        }
    }

    public void Shoot(InputAction.CallbackContext context)
    {
        if (context.performed && isAiming)
        {
            GameObject flecha = Instantiate(flechaPrefab, puntoOrigin.position, Quaternion.identity);
            Rigidbody rb = flecha.GetComponent<Rigidbody>();
            if (rb != null)
            {
                rb.velocity = launchDirection * forceFlecha;
                flecha.transform.rotation = Quaternion.LookRotation(launchDirection);
            }
            isAiming = false;
        }
    }

    private void CalculateDirection()
    {
        launchDirection = cameraTransform.forward;
    }

    private void MostrarTrayectory()
    {
        Vector3[] trajectoryPoints = new Vector3[resolution];

        for (int i = 0; i < resolution; i++)
        {
            float time = i * 0.1f;
            trajectoryPoints[i] = TrayectoryPoint(time);
        }

        trajectory.positionCount = resolution;
        trajectory.SetPositions(trajectoryPoints);
    }

    private Vector3 TrayectoryPoint (float time)
    {
        Vector3 startPosition = puntoOrigin.position;
        Vector3 velocity = launchDirection * forceFlecha;
        return startPosition + velocity * time + 0.5f * new Vector3(0, gravity, 0) * (time * time);
    }
}