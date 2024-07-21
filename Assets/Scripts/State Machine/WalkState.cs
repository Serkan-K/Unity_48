using UnityEngine;

public class WalkState : IState
{
    private PlayerController player;

    public WalkState(PlayerController player)
    {
        this.player = player;
    }

    public void Enter()
    {
        player.GetAnimator().SetBool("isWalking", true);
    }

    public void Execute()
    {
        player.MovePlayer(player.GetWalkSpeed());
        player.HandleInput();

        if (player.isRunning)
        {
            player.stateMachine.ChangeState(player.runState);
        }
        else if (player.isSneaking)
        {
            player.stateMachine.ChangeState(player.sneakState);
        }
        else if (player.isSwimming)
        {
            player.stateMachine.ChangeState(player.swimState);
        }
        else if (player.GetMoveAction().ReadValue<Vector2>() == Vector2.zero)
        {
            player.stateMachine.ChangeState(player.idleState);
        }
    }

    public void Exit()
    {
        player.GetAnimator().SetBool("isWalking", false);
    }
}
