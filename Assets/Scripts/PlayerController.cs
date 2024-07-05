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
    //  bool isWalking = false;
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

        // Rigidbody bileþeninin var olup olmadýðýný kontrol edin
        if (rb == null)
        {
            Debug.LogError("No Rigidbody component found on " + gameObject.name);
        }
        else
        {
            rb.useGravity = true; // Ensure gravity is enabled
        }
    }


    void Update()
    {
        MovePlayer();
        HandleJump();
    }

    void MovePlayer()
    {
        Vector2 direction = moveAction.ReadValue<Vector2>();
        float currentSpeed;

        if (isSneaking)
        {
            currentSpeed = sneakSpeed;
        }
        else if (isRunning)
        {
            currentSpeed = runSpeed;
        }
        else
        {
            currentSpeed = walkSpeed;
        }

        Vector3 movement = new Vector3(direction.x, 0, direction.y) * (currentSpeed * Time.deltaTime);
        transform.position += movement;

        if (movement != Vector3.zero)
        {
            boy.SetBool("isWalking", true);
            boy.SetBool("isRunning", isRunning);
            boy.SetBool("isSneaking", isSneaking);
        }
        else
        {
            boy.SetBool("isWalking", false);
            boy.SetBool("isRunning", false);
            boy.SetBool("isSneaking", false);
        }

        if (runAction.ReadValue<float>() > 0)
        {
            isRunning = true;
        }
        else
        {
            isRunning = false;
        }

        if (sneakAction.ReadValue<float>() > 0)
        {
            isSneaking = true;
        }
        else
        {
            isSneaking = false;
        }
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
        // Assuming the ground is tagged as "Ground"
        if (collision.gameObject.CompareTag("Ground"))
        {
            isJumping = false;
            boy.SetBool("isJumping", false);
        }
    }
}
