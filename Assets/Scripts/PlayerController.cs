using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    #region Serialize
    [SerializeField] float walkSpeed = 5;
    [SerializeField] float runSpeed = 10;
    [SerializeField] float jumpForce = 5;
    [SerializeField] float sneakSpeed = 2;

    [SerializeField] private Transform groundCheck;
    [SerializeField] private GameObject newspaperPrefab;
    [SerializeField] private Transform throwPoint;
    [SerializeField] private float throwForce = 10f;
    [SerializeField] private float throwHeightOffset = 1.5f;
    #endregion

    #region Variables
    PlayerInput playerInput;
    InputAction moveAction, runAction, jumpAction, sneakAction, throwAction, toggleLampAction;
    Animator anim_;
    Rigidbody rb;
    private Vector2 move_Direction;
    bool isRunning = false;
    bool isJumping = false;
    bool isSneaking = false;
    StreetLampController currentStreetLamp;
    #endregion

    #region Main
    private void Awake()
    {
        playerInput = GetComponent<PlayerInput>();
        anim_ = GetComponent<Animator>();
        rb = GetComponent<Rigidbody>();
    }

    void Start()
    {
        moveAction = playerInput.actions.FindAction("Move");
        runAction = playerInput.actions.FindAction("Run");
        jumpAction = playerInput.actions.FindAction("Jump");
        sneakAction = playerInput.actions.FindAction("Sneak");
        throwAction = playerInput.actions.FindAction("Throw");
        toggleLampAction = playerInput.actions.FindAction("ToggleLamp");

        if (rb == null) { Debug.LogError("No Rigidbody component found on " + gameObject.name); }
        toggleLampAction.performed += ctx => ToggleNearestLamp();
    }

    void Update()
    {
        HandleInput();
        MovePlayer();
        HandleJump();
        Look();
        UpdateThrowPoint();
        HandleThrow();
    }
    #endregion

    #region Functions
    void HandleInput()
    {
        isRunning = runAction.ReadValue<float>() > 0;
        isSneaking = sneakAction.ReadValue<float>() > 0;

        if (isRunning)
        {
            isSneaking = false;
        }
        else if (isSneaking)
        {
            isRunning = false;
        }
    }

    void MovePlayer()
    {
        Vector2 direction = moveAction.ReadValue<Vector2>();
        float currentSpeed = walkSpeed;

        if (isSneaking)
        {
            currentSpeed = sneakSpeed;
        }
        else if (isRunning)
        {
            currentSpeed = runSpeed;
        }

        Vector3 movement = new Vector3(-direction.x, 0, -direction.y) * currentSpeed * Time.deltaTime;
        rb.MovePosition(transform.position + movement);

        bool isMoving = movement != Vector3.zero;

        anim_.SetBool("isWalking", isMoving && !isRunning && !isSneaking);
        anim_.SetBool("isRunning", isMoving && isRunning);
        anim_.SetBool("isSneaking", isMoving && isSneaking);
    }

    void HandleJump()
    {
        if (jumpAction.triggered && !isJumping && rb != null)
        {
            rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
            isJumping = true;
            anim_.SetBool("isJumping", true);
        }
    }

    private void Look()
    {
        Vector2 moveInput = moveAction.ReadValue<Vector2>();

        if (moveInput.sqrMagnitude > 0.1f)
        {
            float targetAngle = Mathf.Atan2(-moveInput.x, -moveInput.y) * Mathf.Rad2Deg;

            Quaternion currentRotation = transform.rotation;
            Quaternion targetRotation = Quaternion.Euler(0f, targetAngle, 0f);
            transform.rotation = Quaternion.Slerp(currentRotation, targetRotation, 20f * Time.deltaTime);
        }
        else
            rb.angularVelocity = Vector3.zero;
    }

    void UpdateThrowPoint()
    {
        if (throwPoint != null)
        {
            throwPoint.position = transform.position + transform.forward * 1f + Vector3.up * throwHeightOffset; // 1 birim önde ve yukarıda
            throwPoint.rotation = transform.rotation;
        }
    }

    void HandleThrow()
    {
        if (throwAction.triggered && newspaperPrefab != null && throwPoint != null)
        {
            GameObject newspaper = Instantiate(newspaperPrefab, throwPoint.position, throwPoint.rotation);
            Rigidbody newspaperRb = newspaper.GetComponent<Rigidbody>();
            if (newspaperRb != null)
            {
                newspaperRb.AddForce(throwPoint.forward * throwForce, ForceMode.Impulse);
            }
        }
    }

    void ToggleNearestLamp()
    {
        StreetLampController[] lamps = FindObjectsOfType<StreetLampController>();
        float closestDistance = Mathf.Infinity;
        StreetLampController closestLamp = null;

        foreach (StreetLampController lamp in lamps)
        {
            float distanceToLamp = Vector3.Distance(transform.position, lamp.transform.position);
            if (distanceToLamp < closestDistance && distanceToLamp <= lamp.activationDistance)
            {
                closestDistance = distanceToLamp;
                closestLamp = lamp;
            }
        }

        if (closestLamp != null)
        {
            closestLamp.ToggleLamp();
        }
    }
    #endregion

    #region Misc
    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            isJumping = false;
            anim_.SetBool("isJumping", false);
        }
    }

    void OnTriggerEnter(Collider other)
    {
        StreetLampController lamp = other.GetComponent<StreetLampController>();
        if (lamp != null)
        {
            currentStreetLamp = lamp;
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (currentStreetLamp != null && other.GetComponent<StreetLampController>() == currentStreetLamp)
        {
            currentStreetLamp = null;
        }
    }
    #endregion
}
