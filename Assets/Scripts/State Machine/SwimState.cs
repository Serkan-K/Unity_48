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
        //Debug.Log("Yüzme state'e geçti");
        player.GetRigidbody().velocity = Vector3.zero;
        player.GetAnimator().SetBool("isSwimming", true);
        player.GetRigidbody().useGravity = false;
        player.GetRigidbody().drag = 1;

        player.SetInputActionMap("Water");
    }

    public void Execute()
    {

        player.MovePlayer(player.GetSwimSpeed());
        player.HandleInput();


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
        player.GetRigidbody().drag = 0;
    }

}
