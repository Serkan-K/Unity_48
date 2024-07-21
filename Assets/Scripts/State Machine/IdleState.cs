using System.Net;
using UnityEngine;

public class IdleState : IState
{
    private PlayerController player;

    public IdleState(PlayerController player)
    {
        this.player = player;
    }

    public void Enter()
    {
        player.GetAnimator().SetBool("isWalking", false);
        player.GetAnimator().SetBool("isRunning", false);
        player.GetAnimator().SetBool("isSneaking", false);
        player.GetAnimator().SetBool("isJumping", false);
        player.GetAnimator().SetBool("isSwimming", false);

    }
    public void CheckMap()
    {
        if (player.GetMap() == 2)
        {
            player.stateMachine.ChangeState(player.swimState);
        }
    }

    public void Execute()
    {
        player.HandleInput();

        Vector2 moveInput = player.GetMoveAction().ReadValue<Vector2>();
        if (moveInput != Vector2.zero)
        {

            if (player.isRunning)
            {
                player.stateMachine.ChangeState(player.runState);
            }
            else if (player.isSneaking)
            {
                player.stateMachine.ChangeState(player.sneakState);
            }
            else
            {
                //CheckMap();
                player.stateMachine.ChangeState(player.walkState);
            }
            if (player.isSwimming)
            {
                player.stateMachine.ChangeState(player.swimState);
            }
        }
    }

    public void Exit()
    {

    }
}
