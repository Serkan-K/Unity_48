using UnityEngine;

public class JumpState : IState
{
    private PlayerController player;

    public JumpState(PlayerController player)
    {
        this.player = player;
    }

    public void Enter()
    {
        player.GetAnimator().SetBool("isJumping", true);
        player.Jump();
    }

    public void Execute()
    {
        player.CheckMap();
        player.HandleInput();

        if (player.isGrounded)
        {
            player.stateMachine.ChangeState(player.idleState);
        }
        else if (!player.isGrounded || player.isFalling)
        {
            player.stateMachine.ChangeState(player.fallState);
        }

    }

    public void Exit()
    {
        player.GetAnimator().SetBool("isJumping", false);
    }
}
