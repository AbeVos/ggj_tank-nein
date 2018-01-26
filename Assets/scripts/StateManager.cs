using System.Collections;
using System.Collections.Generic;
using UnityEngine;

class StateManager : MonoBehaviour
{
    public static StateManager instance = null;
    public enum gameState
    {
        Intro,
        Playing,
        Pauze,
        Outro
    };

    public gameState currentState { get; private set; }
    public gameState previousState { get; private set; }

    public void Awake()
    {
        if (instance == null) instance = this;
        else if (instance != this) Destroy(gameObject);
    }

    public void SwitchState(gameState targetState)
    {
        // Debug.Log(string.Format("Changing from {0} to {1}", currentState, targetState));
        previousState = currentState;
        currentState = targetState;
        ActionOnSwitch(currentState);
    }

    private void ActionOnSwitch(gameState state)
    {
        switch (state)
        {
            case gameState.Intro:
                break;
            case gameState.Playing:
                break;
            case gameState.Pauze:
                break;
            case gameState.Outro:
                break;
            default:
                Debug.Log("StateManager asks: dafuq u doin, fool?");
                break;
        }
    }
}
