using UnityEngine;

public class SwimState : IState
{
    private PlayerController player;
    

    public SwimState(PlayerController player)
    {
        this.player = player;
    }

    public void Enter()
    {
        player.GetAnimator().SetBool("isSwimming", true);
        player.GetRigidbody().useGravity = false;
        player.GetRigidbody().drag = 2f;

        player.SetInputActionMap("Water");
    }

    public void Execute()
    {

        player.MovePlayer(player.GetSwimSpeed());
        player.HandleInput();
        player.ApplyBuoyancy();
        //yukar�daki methoda bak kuvvet uygulamal�. 
        // y�zme look yonunda olmal� ok

        if (!player.isSwimming)
        {
            player.GetRigidbody().useGravity = true;
            player.GetRigidbody().drag = 0f;

            player.SetInputActionMap("Movement");

            if (player.isRunning)
            {
                player.stateMachine.ChangeState(player.runState);
            }
            else if (player.isSneaking)
            {
                player.stateMachine.ChangeState(player.sneakState);
            }
            else if (player.GetMoveAction().ReadValue<Vector2>() == Vector2.zero)
            {
                player.stateMachine.ChangeState(player.idleState);
            }
            else
            {
                player.stateMachine.ChangeState(player.walkState);
            }
        }
    }

    public void Exit()
    {
        player.GetAnimator().SetBool("isSwimming", false);
        player.GetRigidbody().useGravity = true;
    }

}
