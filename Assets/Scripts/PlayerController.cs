using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UIElements;

public class PlayerController : MonoBehaviour
{

    #region Serialize
    [Header("Values")]
    [SerializeField] float walkSpeed = 5;
    [SerializeField] float runSpeed = 10;
    [SerializeField] float sneakSpeed = 2;
    [SerializeField] float jumpHeight;
    [Space(20)]
    [SerializeField] private GameObject newspaperPrefab;
    [SerializeField] private float throwForce = 10f;
    [SerializeField] private float throwHeightOffset = 1.5f;
    [SerializeField] private LayerMask interactableLayer;
    [SerializeField] private float interactionRange = 2f;
    [SerializeField] private Transform interactionPoint;
    [SerializeField] private float pushForce = 10f;
    [SerializeField] private float pullForce = 10f;
    [Space(3)]
    [Header("Swim")]
    [SerializeField] float swimSpeed = 3;

    public float GetWalkSpeed() => walkSpeed;

    public float GetRunSpeed() => runSpeed;

    public float GetSneakSpeed() => sneakSpeed;

    public float GetSwimSpeed() => swimSpeed;


    #endregion







    #region Variables
    private PlayerInput playerInput;
    private InputActionAsset inputActions;
    [HideInInspector]
    public InputAction moveAction, runAction, jumpAction, sneakAction, throwAction, toggleLampAction, pushAction, pullAction, swimAction;
    private Animator anim_;
    private Rigidbody rb;
    private StreetLampController currentStreetLamp;
    private Vector2 move_Direction;
    private CapsuleCollider sneakCollider;
    private Transform throwPoint;
    private GroundCheck ground_control_;
    private Rigidbody boxes;

    private float mapCode = 1;


    public StateMachine stateMachine { get; private set; }
    public IState idleState, walkState, runState, sneakState, jumpState, fallState, swimState;

    public bool isRunning { get; set; }
    public bool isSneaking { get; set; } = false;
    public bool isGrounded { get; set; }
    public bool isSwimming { get; set; } = false;
    public bool isFalling { get; set; }
    public bool isPushing { get; set; }
    public bool isPulling { get; set; }
    

    #endregion

    #region Main


    private void Awake()
    {
        playerInput = GetComponent<PlayerInput>();
        anim_ = GetComponent<Animator>();
        rb = GetComponent<Rigidbody>();
        sneakCollider = GetComponent<CapsuleCollider>();
        ground_control_ = GetComponent<GroundCheck>();

        idleState = new IdleState(this);
        walkState = new WalkState(this);
        runState = new RunState(this);
        sneakState = new SneakState(this);
        jumpState = new JumpState(this);
        fallState = new FallState(this);
        swimState = new SwimState(this);
        stateMachine = new StateMachine();
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
        pushAction = playerInput.actions.FindAction("Push");
        pullAction = playerInput.actions.FindAction("Pull");


        playerInput.actions.FindActionMap("Water");
        swimAction = playerInput.actions.FindAction("Swim");

        SetInputActionMap("Movement");

        if (rb == null) { Debug.LogError("No Rigidbody component found on " + gameObject.name); }
        toggleLampAction.performed += ctx => ToggleNearestLamp();

        interactionPoint = new GameObject("InteractionPoint").transform;
        interactionPoint.SetParent(transform);
        interactionPoint.localPosition = new Vector3(0, 0, 1.5f);

        stateMachine.ChangeState(idleState);
    }
    void Update()
    {
        stateMachine.Update();
        PushPullObject();
        UpdateThrowPoint();
        HandleThrow();
    }





    private void FixedUpdate()
    {
        HandleInput();
        Ground_Control();
    }
    #endregion


    #region Functions

    public void HandleInput()
    {
        isRunning = runAction.ReadValue<float>() > 0;
        isSneaking = sneakAction.ReadValue<float>() > 0;


        if (isGrounded)
        {
            if (isRunning)
            {
                isSneaking = false;
            }
            else if (isSneaking)
            {
                isRunning = false;
            }
            else if (jumpAction.triggered && isGrounded)
            {
                stateMachine.ChangeState(jumpState);
            }
            
        }

        else if (isSwimming)
        {
            mapCode = 2;
            SetInputActionMap("Water");
            stateMachine.ChangeState(swimState);
        }

        else if (isFalling && rb.velocity.y < 0)
        {
            stateMachine.ChangeState(fallState);
        }

        //bu kod blogu da sıkıntılı gıbı
        
    }
    

    public void PushPullObject()
    {
        isPushing = pushAction.ReadValue<float>() >0;
        isPulling = pullAction.ReadValue<float>() >0 ;
        if (!isPushing && !isPulling)
        {
            return;
        }

        Collider[] hitColliders = Physics.OverlapSphere(interactionPoint.position, interactionRange, interactableLayer);
        if (hitColliders.Length > 0)
        {
           
            if (boxes == null && isPushing == true)
            {
                boxes = hitColliders[0].GetComponent<Rigidbody>();
                anim_.SetBool("isPushing", true);
                MoveObject();
            }
            else if (boxes == null && isPulling == true)
            {
                boxes = hitColliders[0].GetComponent<Rigidbody>();
                anim_.SetBool("isPulling", true);
                MoveObject();
            }
            else if (boxes != null && isPushing == true)
            {
                anim_.SetBool("isPushing", true);
                MoveObject();


            }
            else if (boxes != null && isPulling == true)
            {

                anim_.SetBool("isPulling", true);
                MoveObject();

            }
            else if (boxes != null && isPulling == false && isPushing == false)
            {
                anim_.SetBool("isPushing", false);
                anim_.SetBool("isPulling", false);
                boxes = null;
            }
        }
        boxes = null;
       
    }
    private void MoveObject()
    {

        Vector3 direction = (interactionPoint.position - boxes.position).normalized;
        float distance = Vector3.Distance(interactionPoint.position, boxes.position);
        direction.y = 0;

        if (distance > 0.5f)
        {
           boxes.MovePosition(boxes.position + direction * walkSpeed * Time.deltaTime);
        }
        else
        {
            boxes.velocity = Vector3.zero;
        }
        float maxSpeed = 2f;
        if (boxes.velocity.magnitude > maxSpeed)
        {
            boxes.velocity = boxes.velocity.normalized * maxSpeed;
        }

    }










    public void MovePlayer(float speed)
    {
        if (isSwimming)
        {
            Vector2 directionSwim = swimAction.ReadValue<Vector2>();
            Vector3 movementSwim = speed * Time.deltaTime * new Vector3(-directionSwim.x, 0, -directionSwim.y);
            rb.MovePosition(transform.position + movementSwim);


            if (directionSwim.sqrMagnitude > 0.1f)
            {
                float targetAngle = Mathf.Atan2(-directionSwim.x, -directionSwim.y) * Mathf.Rad2Deg;

                Quaternion currentRotation = transform.rotation;
                Quaternion targetRotation = Quaternion.Euler(0f, targetAngle, 0f);
                transform.rotation = Quaternion.Slerp(currentRotation, targetRotation, 20f * Time.deltaTime);
            }
            else
                rb.angularVelocity = Vector3.zero;

        }
        else
        {
            Vector2 direction = moveAction.ReadValue<Vector2>();
            Vector3 movement = speed * Time.deltaTime * new Vector3(-direction.x, 0, -direction.y);
            rb.MovePosition(transform.position + movement);

            if (direction.sqrMagnitude > 0.1f)
            {
                float targetAngle = Mathf.Atan2(-direction.x, -direction.y) * Mathf.Rad2Deg;

                Quaternion currentRotation = transform.rotation;
                Quaternion targetRotation = Quaternion.Euler(0f, targetAngle, 0f);
                transform.rotation = Quaternion.Slerp(currentRotation, targetRotation, 20f * Time.deltaTime);
            }
            else
                rb.angularVelocity = Vector3.zero;
        }

    }





    public Animator GetAnimator() { return anim_; }

    public InputAction GetMoveAction() { return moveAction; }


    public void SetInputActionMap(string actionMapName)
    {
        playerInput.SwitchCurrentActionMap(actionMapName);
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








    public void Jump()
    {
        rb.velocity = new Vector3(rb.velocity.x, jumpHeight, rb.velocity.z);
    }

    #endregion



    #region Misc


    private void Ground_Control()
    {
        if (ground_control_ == null)
        {
            Debug.LogError("GroundCheck component not found!");
            return;
        }

        if (ground_control_._walk)
        {
            rb.drag = 0;
            isGrounded = true;
            isSwimming = false;
            isFalling = false;
            stateMachine.ChangeState(idleState);
            SetInputActionMap("Movement");
            mapCode = 1;


        }
        else if (ground_control_._swim)
        {
            isSwimming = true;
            isGrounded = false;
            isFalling = false;
            stateMachine.ChangeState(swimState);
            SetInputActionMap("Water");
            mapCode = 2;


        }
        else
        {
            rb.drag = 1;
            isFalling = true;
            isGrounded = false;
            isSwimming = false;
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



