using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StateManagerTester : MonoBehaviour
{
    private int stateSwitchInt = 0;

    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            stateSwitchInt++;
            StateManager.instance.SwitchState((StateManager.gameState)stateSwitchInt);
        }   
    }
}
