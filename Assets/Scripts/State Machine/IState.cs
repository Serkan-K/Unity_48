using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IState
{
    void EnterState(PlayerController player);
    void UpdateState(PlayerController player);
    void ExitState(PlayerController player);
}
