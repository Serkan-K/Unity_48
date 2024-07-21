using UnityEngine;
public class SneakState : IState
{
    private PlayerController player;
    private Vector3 originalColliderCenter;
    private float originalColliderHeight;
    public SneakState(PlayerController player)
    {
        this.player = player;
    }
    public void Enter()
    {
        player.GetAnimator().SetBool("isSneaking", true);
        CapsuleCollider collider = player.GetCollider();
        originalColliderCenter = collider.center;
        originalColliderHeight = collider.height;
        collider.center = new Vector3(originalColliderCenter.x, 1f, originalColliderCenter.z);
        collider.height = 2f;
    }
    public void Execute()
    {
        player.MovePlayer(player.GetSneakSpeed());
        player.HandleInput();
        player.CheckMap();
        if (!player.isSneaking)
        {
            if (player.isRunning)
            {
                player.stateMachine.ChangeState(player.runState);
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
        player.GetAnimator().SetBool("isSneaking", false);
        CapsuleCollider collider = player.GetCollider();
        collider.center = originalColliderCenter;
        collider.height = originalColliderHeight;
    }
}
