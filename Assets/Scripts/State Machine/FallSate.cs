using UnityEngine;

public class FallState : IState
{
    private PlayerController player;
    //private float y_vel;

    public FallState(PlayerController player)
    {
        this.player = player;
    }

    public void Enter()
    {
        //y_vel = player.GetRigidbody().velocity.y;
        //y_vel = player.GetAnimator().GetFloat("Y_velocity");
        player.GetAnimator().SetBool("isFalling", true);
    }

    public void Execute()
    {
        player.isFalling = true;
        player.isGrounded = player.isSwimming = false;
        player.HandleInput();
        player.MovePlayer(player.GetWalkSpeed());

    }

    public void Exit()
    {
        player.GetAnimator().SetBool("isJumping", false);
        if (player.isGrounded)
        {
            player.isFalling = false;
            player.GetAnimator().SetBool("isFalling", false);
            player.isGrounded = true;
        }
        else if (player.isSwimming)
        {
            player.isSwimming = true;
            player.isFalling = false;
            player.isGrounded = false;
        }
    }
}
