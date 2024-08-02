using UnityEngine;

public class RunJumpState : IState
{
    private PlayerController player;

    public RunJumpState(PlayerController player)
    {
        this.player = player;
    }

    public void Enter()
    {
        Debug.Log("Entering RunJumpState");
        player.GetAnimator().SetBool("isRunJump", true);
        player.GetAnimator().SetBool("isWalking",false);
        player.GetAnimator().SetBool("isRunning", false);

        player.Jump();
    }

    public void Execute()
    {
        player.CheckMap();
        player.HandleInput();

        if (player.isGrounded && player.GetMoveAction().ReadValue<Vector2>() == Vector2.zero)
        {
            Debug.Log("Switching to IdleState");
            player.stateMachine.ChangeState(player.idleState);
        }
        else if (!player.isGrounded || player.isFalling)
        {
            Debug.Log("Switching to FallState");
            player.stateMachine.ChangeState(player.fallState);
        }
    }

    public void Exit()
    {
        Debug.Log("Exiting RunJumpState");
        player.GetAnimator().SetBool("isRunJump", false);
    }
}

