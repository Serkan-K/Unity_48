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
        player.GetRigidbody().AddForce(Vector3.up * player.GetJumpForce(), ForceMode.Impulse);
    }

    public void Execute()
    {
        player.CheckMap();
        if (player.isGrounded)
        {
            player.stateMachine.ChangeState(player.idleState);
        }

    }

    public void Exit()
    {
        player.GetAnimator().SetBool("isJumping", false);
    }
}
