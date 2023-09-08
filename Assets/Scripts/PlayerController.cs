using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(CharacterController))]
public class PlayerController : MonoBehaviour
{
    private readonly float rotationThreshold = 0.01f;

    [SerializeField]
    private Camera playerCamera;

    [SerializeField]
    private WeaponManager weaponManager;

    [SerializeField]
    [Tooltip("Character movement speed")]
    private float movementSpeed = 4.6f;

    [SerializeField]
    private float gravity = -12f;

    [SerializeField]
    [Tooltip("Acceleration and deceleration")]
    private float speedChangeRate = 2.0f;

    [SerializeField]
    [Tooltip("Horizontal and vertical mouse sensitivity multiplier")]
    private float mouseSensitivityMultiplier = 1f;

    [SerializeField]
    [Tooltip("How far in degrees can you move the camera up")]
    private float topClamp = 90.0f;

    [SerializeField]
    [Tooltip("How far in degrees can you move the camera down")]
    private float bottomClamp = -90.0f;

    private PlayerActions playerActions;

    private CharacterController characterController;

    private float pitch = 0f;

    private float yaw = 0f;

    private Vector2 look;

    private Vector2 move;

    private float speed;

    private float velocityY;

    private RaycastHit shootRaycast;

    private DestructibleObstacle destructibleObstacle;

    public event EventHandler<DestructibleObstacle> OnObstacleChanged;

    public DestructibleObstacle DestructibleObstacle
    {
        get => destructibleObstacle;
        private set
        {
            if (destructibleObstacle != value)
            {
                destructibleObstacle = value;
                OnObstacleChanged?.Invoke(this, destructibleObstacle);
            }
        }
    }

    private void Awake()
    {
        playerActions = new PlayerActions();
        characterController = GetComponent<CharacterController>();
    }

    private void Start()
    {
        playerActions.Main.SlotOne.performed += (o) => weaponManager.SetWeapon(0);
        playerActions.Main.SlotTwo.performed += (o) => weaponManager.SetWeapon(1);
        playerActions.Main.SlotThree.performed += (o) => weaponManager.SetWeapon(2);
    }

    private void OnEnable()
    {
        playerActions.Enable();
        LockCursor();
    }

    private void OnDisable()
    {
        playerActions.Disable();
        UnlockCursor();
    }

    private void Update()
    {
        UpdateMovement();
        UpdateShooting();

        if (playerActions.Main.Shoot.IsPressed())
        {
            weaponManager.TryShoot(DestructibleObstacle);
        }
    }

    private void UpdateShooting()
    {
        if (!Physics.Raycast(playerCamera.transform.position, playerCamera.transform.forward, out shootRaycast, 200f))
        {
            DestructibleObstacle = null;
            return;
        }

        DestructibleObstacle = shootRaycast.collider.gameObject.GetComponent<DestructibleObstacle>();

        if (DestructibleObstacle == null)
        {
            return;
        }
    }

    private void LateUpdate()
    {
        UpdateLook();
    }

    private void UpdateMovement()
    {
        float currentVelocity = new Vector3(characterController.velocity.x, 0.0f, characterController.velocity.z).magnitude;
        float targetVelocity = (move == Vector2.zero) ? 0f : movementSpeed;
        float velocityDifferenceThreshold = 0.05f;

        if (Approximately(currentVelocity, targetVelocity, velocityDifferenceThreshold))
        {
            speed = targetVelocity;
        }
        else
        {
            speed = Mathf.Lerp(currentVelocity, targetVelocity, Time.deltaTime * speedChangeRate);
        }

        move = playerActions.Main.Move.ReadValue<Vector2>();
        Vector3 inputDirection = new Vector3(move.x, 0.0f, move.y).normalized;

        if (move != Vector2.zero)
        {
            inputDirection = (transform.right * move.x) + (transform.forward * move.y);
        }

        if (characterController.isGrounded)
        {
            velocityY = 0f;
        }

        velocityY += gravity * Time.deltaTime;

        characterController.Move((inputDirection.normalized * (speed * Time.deltaTime)) + (new Vector3(0.0f, velocityY, 0.0f) * Time.deltaTime));
    }

    private void UpdateLook()
    {
        look = playerActions.Main.Look.ReadValue<Vector2>();

        if (look.sqrMagnitude < rotationThreshold)
        {
            return;
        }

        pitch += look.y * mouseSensitivityMultiplier;
        pitch = ClampAngle(pitch, bottomClamp, topClamp);

        yaw = look.x * mouseSensitivityMultiplier;

        playerCamera.transform.localRotation = Quaternion.Euler(pitch, 0.0f, 0.0f);
        transform.Rotate(Vector3.up * yaw);
    }

    private float ClampAngle(float angle, float min, float max)
    {
        if (angle < -360f)
        {
            angle += 360f;
        }
        else if (angle > 360f)
        {
            angle -= 360f;
        }

        return Mathf.Clamp(angle, min, max);
    }

    private bool Approximately(float value1, float value2, float epsilon)
    {
        return Math.Abs(value1 - value2) < epsilon;
    }

    private void LockCursor()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    private void UnlockCursor()
    {
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }
}
