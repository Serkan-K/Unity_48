using UnityEngine;

public class RunState : IState
{
    private PlayerController player;

    public RunState(PlayerController player)
    {
        this.player = player;
    }

    public void Enter()
    {
        
        player.GetAnimator().SetBool("isRunning", true);
    }

    public void Execute()
    {
        player.MovePlayer(player.GetRunSpeed());
        player.HandleInput();
        player.CheckMap();


        if (player.isRunJump)
        {
            player.stateMachine.ChangeState(player.runJumpState);
        }

        if (!player.isRunning)
        {
            if (player.isSneaking)
            {
                player.stateMachine.ChangeState(player.sneakState);
            }
            else if (player.isRunJump)
            {
                player.stateMachine.ChangeState(player.runJumpState);
            }
            else if (player.isGrounded && player.GetMoveAction().ReadValue<Vector2>() == Vector2.zero )
            {
                player.stateMachine.ChangeState(player.idleState);
            }
            else if (player.isSwimming)
            {
                player.stateMachine.ChangeState(player.swimState);
            }
            else if (player.isGrounded && player.GetMoveAction().ReadValue<Vector2>() != Vector2.zero)
            {
                player.stateMachine.ChangeState(player.walkState);
            }
        }

    }

    public void Exit()
    {
        player.GetAnimator().SetBool("isRunning", false);
    }
}
