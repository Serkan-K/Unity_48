using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WalkingState : IState
{
    public void EnterState(PlayerController player)
    {
        Debug.Log("Enter Walk state");
    }
    public void ExitState(PlayerController player)
    {
        Debug.Log("Exit Walk state");
    }

    public void UpdateState(PlayerController player)
    {
        Debug.Log("Update Walk state");
    }
}
