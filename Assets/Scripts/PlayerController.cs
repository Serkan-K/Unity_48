﻿using System.Collections;
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
    [SerializeField] float mapCode = 1;

    [SerializeField] private Transform groundCheck;
    [SerializeField] private GameObject newspaperPrefab;
    [SerializeField] private Transform throwPoint;
    [SerializeField] private float throwForce = 10f;
    [SerializeField] private float throwHeightOffset = 1.5f;
    [SerializeField] private Transform interactionPoint;
    [SerializeField] private float interactionRange = 2f;
    [SerializeField] private LayerMask interactableLayer;
    [SerializeField] float swimSpeed = 3;
    [SerializeField] float buoyancyForce = 10f;
    [SerializeField] LayerMask waterLayer;
    public float GetJumpForce() => jumpForce;

    public float GetWalkSpeed() => walkSpeed;

    public float GetRunSpeed() => runSpeed;

    public float GetSneakSpeed() => sneakSpeed;

    public float GetSwimSpeed() => swimSpeed;


    #endregion

    #region Variables
    private PlayerInput playerInput;
    private InputActionAsset inputActions;
    private InputAction moveAction, runAction, jumpAction, sneakAction, throwAction, toggleLampAction, interactAction, swimAction;
    private Animator anim_;
    private Rigidbody rb;
    private StreetLampController currentStreetLamp;
    private Rigidbody objectBeingMoved;
    private Vector2 move_Direction;
    private CapsuleCollider sneakCollider;

    public StateMachine stateMachine { get; private set; }
    public IState idleState, walkState, runState, sneakState, jumpState, swimState;

    public bool isRunning { get; set; }
    public bool isSneaking { get; set; } = false;
    public bool isGrounded { get; private set; }
    private bool isPulling;
    public bool isSwimming { get; set; } = false;

    #endregion


    #region Main
    private void Awake()
    {
        playerInput = GetComponent<PlayerInput>();
        anim_ = GetComponent<Animator>();
        rb = GetComponent<Rigidbody>();
        sneakCollider = GetComponent<CapsuleCollider>();
        

        idleState = new IdleState(this);
        walkState = new WalkState(this);
        runState = new RunState(this);
        sneakState = new SneakState(this);
        jumpState = new JumpState(this);
        swimState = new SwimState(this);

        stateMachine = new StateMachine();

        interactionPoint = new GameObject("InteractionPoint").transform;
        interactionPoint.SetParent(transform);
        interactionPoint.localPosition = new Vector3(0, 0, 1.5f);
    }
    public Rigidbody GetRigidbody() => rb;
    public CapsuleCollider GetCollider() => sneakCollider;

    void Start()
    {
        inputActions = playerInput.actions;

        moveAction = playerInput.actions.FindAction("Move");
        runAction = playerInput.actions.FindAction("Run");
        jumpAction = playerInput.actions.FindAction("Jump");
        sneakAction = playerInput.actions.FindAction("Sneak");
        throwAction = playerInput.actions.FindAction("Throw");
        toggleLampAction = playerInput.actions.FindAction("ToggleLamp");
        interactAction = playerInput.actions.FindAction("Interact");
        playerInput.actions.FindActionMap("Water");
        swimAction = playerInput.actions.FindAction("Swim");
        SetInputActionMap("Movement");
        if (rb == null) { Debug.LogError("No Rigidbody component found on " + gameObject.name); }
        toggleLampAction.performed += ctx => ToggleNearestLamp();

        stateMachine.ChangeState(idleState);
    }

    void Update()
    {
        Vector2 swimInput = swimAction.ReadValue<Vector2>();
        Debug.Log(swimInput);
        stateMachine.Update();
        Look();
        UpdateThrowPoint();
        HandleThrow();
        HandleInteraction();
        HandleInput();

        if (!isRunning && !isSneaking && !isSwimming && objectBeingMoved != null)
        {
            if (moveAction.ReadValue<Vector2>().sqrMagnitude == 0)
            {
                anim_.SetBool("isPushing", false);
                anim_.SetBool("isPulling", false);
                objectBeingMoved = null;
            }
        }
    }
    #endregion

    #region Functions
    public void HandleInput()
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

        if (jumpAction.triggered && isGrounded)
        {
            stateMachine.ChangeState(jumpState);
        }

        if (isSwimming)
        {
            stateMachine.ChangeState(swimState);
        }
    }


    public void MovePlayer(float speed)
    {
        if (isSwimming)
        {
            Vector2 directionSwim = swimAction.ReadValue<Vector2>();
            Vector3 movementSwim = new Vector3(-directionSwim.x, 0, -directionSwim.y) * speed * Time.deltaTime;
            rb.MovePosition(transform.position + movementSwim);
        }
        else
        {
            Vector2 direction = moveAction.ReadValue<Vector2>();
            Vector3 movement = new Vector3(-direction.x, 0, -direction.y) * speed * Time.deltaTime;
            rb.MovePosition(transform.position + movement);
        }

    }

    public Animator GetAnimator()
    {
        return anim_;
    }

    public InputAction GetMoveAction()
    {
        return moveAction;
    }

    public void SetInputActionMap(string actionMapName)
    {
        playerInput.SwitchCurrentActionMap(actionMapName);
    }

    private void Look()
    {
        // karakter bakmıyo
        if (isSwimming)
        {
            //SetInputActionMap("Water");
            //swimAction = playerInput.actions.FindAction("Swim");
            Vector2 swimInput = swimAction.ReadValue<Vector2>();
            if (swimInput.sqrMagnitude > 0.1f)
            {
                float targetAngle = Mathf.Atan2(-swimInput.x, -swimInput.y) * Mathf.Rad2Deg;

                Quaternion currentRotation = transform.rotation;
                Quaternion targetRotation = Quaternion.Euler(0f, targetAngle, 0f);
                transform.rotation = Quaternion.Slerp(currentRotation, targetRotation, 20f * Time.deltaTime);
            }
            else
                rb.angularVelocity = Vector3.zero;
        }
        else
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

    void UpdateThrowPoint()
    {
        if (throwPoint != null)
        {
            throwPoint.position = transform.position + transform.forward * 1f + Vector3.up * throwHeightOffset;
            throwPoint.rotation = transform.rotation;
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

    void HandleInteraction()
    {
        // Karakterin önündeyse pushla arkasındaysa pull ile çek
        Collider[] hitColliders = Physics.OverlapSphere(interactionPoint.position, interactionRange, interactableLayer);
        if (hitColliders.Length > 0)
        {
            if (objectBeingMoved == null && interactAction.ReadValue<float>() > 0)
            {
                objectBeingMoved = hitColliders[0].GetComponent<Rigidbody>();
                if (objectBeingMoved != null)
                {
                    if (Vector3.Dot(transform.forward, (objectBeingMoved.position - transform.position).normalized) > 0)
                    {
                        isPulling = false;
                        anim_.SetBool("isPushing", true);
                    }
                    else
                    {
                        isPulling = true;
                        anim_.SetBool("isPulling", true);
                    }
                }
            }
        }
        else if (objectBeingMoved != null && interactAction.ReadValue<float>() == 0)
        {
            anim_.SetBool("isPushing", false);
            anim_.SetBool("isPulling", false);
            objectBeingMoved = null;
        }

        if (objectBeingMoved != null && interactAction.ReadValue<float>() > 0)
        {
            MoveObject();
        }
    }

    private void MoveObject()
    {
        if (objectBeingMoved == null) return;

        Vector3 direction = (interactionPoint.position - objectBeingMoved.position).normalized;
        float distance = Vector3.Distance(interactionPoint.position, objectBeingMoved.position);


        direction.y = 0;

        if (distance > 0.5f)
        {
            objectBeingMoved.MovePosition(objectBeingMoved.position + direction * walkSpeed * Time.deltaTime);
        }
        else
        {
            objectBeingMoved.velocity = Vector3.zero;
        }


        float maxSpeed = 5f;
        if (objectBeingMoved.velocity.magnitude > maxSpeed)
        {
            objectBeingMoved.velocity = objectBeingMoved.velocity.normalized * maxSpeed;
        }
    }
    public void ApplyBuoyancy()
    {

        Collider[] hitColliders = Physics.OverlapSphere(transform.position, .1f, waterLayer);
        if (hitColliders.Length > 0)
        {

            rb.AddForce(Vector3.up * buoyancyForce, ForceMode.Acceleration);
        }
    }
    #endregion

    #region Misc
    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            isGrounded = true;
            stateMachine.ChangeState(idleState);

        }
    }

    void OnCollisionExit(Collision collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            isGrounded = false;
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Water"))
        {
            isSwimming = true;
            stateMachine.ChangeState(swimState);
            SetInputActionMap("Water");
            mapCode = 2;

        }

        StreetLampController lamp = other.GetComponent<StreetLampController>();
        if (lamp != null)
        {
            currentStreetLamp = lamp;
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Water"))
        {
            isSwimming = false;
            stateMachine.ChangeState(idleState);
            SetInputActionMap("Movement");
            mapCode = 1;

        }

        if (currentStreetLamp != null && other.GetComponent<StreetLampController>() == currentStreetLamp)
        {
            currentStreetLamp = null;
        }
    }
    public float GetMap() => mapCode;

    public void CheckMap()
    {
        if (GetMap() == 2)
        {
            stateMachine.ChangeState(swimState);
        }
    }

    #endregion
}



