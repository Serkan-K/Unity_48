using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    PlayerInput playerInput;
    InputAction moveAction, runAction, jumpAction, sneakAction;
    Animator anim_;
    Rigidbody rb;
    private Vector2 move_Direction;

    [SerializeField] float walkSpeed = 5;
    [SerializeField] float runSpeed = 10;
    [SerializeField] float jumpForce = 5;
    [SerializeField] float sneakSpeed = 2;

    bool isRunning = false;
    bool isJumping = false;
    bool isSneaking = false;



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
    }


    void HandleInput()
    {
        if (runAction.ReadValue<float>() > 0)
        {
            isRunning = true;
            isSneaking = false;
        }
        else
        {
            isRunning = false;
        }

        if (sneakAction.ReadValue<float>() > 0)
        {
            isSneaking = true;
            isRunning = false;
        }
        else
        {
            isSneaking = false;
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

        Vector3 movement = new Vector3(direction.x, 0, direction.y) * currentSpeed * Time.deltaTime;
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

    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            isJumping = false;
            anim_.SetBool("isJumping", false);
        }
    }
}