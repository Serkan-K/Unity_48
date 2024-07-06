using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    PlayerInput playerInput;
    InputAction moveAction, runAction, jumpAction, sneakAction;
    Animator boy;
    Rigidbody rb;

    [SerializeField] float walkSpeed = 5;
    [SerializeField] float runSpeed = 10;
    [SerializeField] float jumpForce = 5;
    [SerializeField] float sneakSpeed = 2;
    bool isRunning = false;
    bool isJumping = false;
    bool isSneaking = false;

    void Start()
    {
        playerInput = GetComponent<PlayerInput>();
        moveAction = playerInput.actions.FindAction("Move");
        runAction = playerInput.actions.FindAction("Run");
        jumpAction = playerInput.actions.FindAction("Jump");
        sneakAction = playerInput.actions.FindAction("Sneak");
        boy = GetComponent<Animator>();
        rb = GetComponent<Rigidbody>();

        if (rb == null)
        {
            Debug.LogError("No Rigidbody component found on " + gameObject.name);
        }
        else
        {
            rb.useGravity = true; // Ensure gravity is enabled
            rb.isKinematic = false; // Ensure isKinematic is disabled
        }
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
            isSneaking = false; // Sneak ve koşma aynı anda olmamalı
        }
        else
        {
            isRunning = false;
        }

        if (sneakAction.ReadValue<float>() > 0)
        {
            isSneaking = true;
            isRunning = false; // Sneak ve koşma aynı anda olmamalı
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

        boy.SetBool("isWalking", isMoving && !isRunning && !isSneaking);
        boy.SetBool("isRunning", isMoving && isRunning);
        boy.SetBool("isSneaking", isMoving && isSneaking);
    }

    void HandleJump()
    {
        if (jumpAction.triggered && !isJumping && rb != null)
        {
            rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
            isJumping = true;
            boy.SetBool("isJumping", true);
        }
    }

    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            isJumping = false;
            boy.SetBool("isJumping", false);
        }
    }
}