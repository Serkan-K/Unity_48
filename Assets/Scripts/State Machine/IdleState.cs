using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IdleState : IState
{
    public void EnterState(PlayerController player)
    {
        Debug.Log("Enter Idle state");
    }
    public void ExitState(PlayerController player)
    {
        Debug.Log("Exit Idle state");
    }

    public void UpdateState(PlayerController player)
    {
        Debug.Log("Update Idle state");
    }



}
