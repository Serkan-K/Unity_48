using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JumpState : IState
{
    public void EnterState(PlayerController player)
    {
        Debug.Log("Enter Jump state");
    }
    public void ExitState(PlayerController player)
    {
        Debug.Log("Exit Jump state");
    }

    public void UpdateState(PlayerController player)
    {
        Debug.Log("Update Jump state");
    }
}
