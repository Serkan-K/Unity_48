using System;
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
    #endregion


    #region Variables
    PlayerInput playerInput;
    InputAction moveAction, runAction, jumpAction, sneakAction;
    Animator anim_;
    Rigidbody rb;
    private Vector2 move_Direction;
    bool isRunning = false;
    bool isJumping = false;
    bool isSneaking = false;
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

        if (rb == null) { Debug.LogError("No Rigidbody component found on " + gameObject.name); }
    }

    void Update()
    {
        HandleInput();
        MovePlayer();
        HandleJump();
        Look();
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






    #endregion





    #region Misc
    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Ground"))
        {
            isJumping = false;
            anim_.SetBool("isJumping", false);
        }
    }
    #endregion

}