using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RunState : IState
{
    public void EnterState(PlayerController player)
    {
        Debug.Log("Enter Run state");
    }
    public void ExitState(PlayerController player)
    {
        Debug.Log("Exit Run state");
    }

    public void UpdateState(PlayerController player)
    {
        Debug.Log("Update Run state");
    }
}
