using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StateManager : MonoBehaviour
{
    public static StateManager instance = null;
    public enum gameState
    {
        Intro,
        Playing,
        Pauze,
        Outro
    };

    public delegate void OnStateSwitchEvent(gameState newState, gameState oldState);
    /// <summary>Occurs on state switch, do not use SetState in this.</summary>
    public event OnStateSwitchEvent OnStateSwitched;

    public gameState currentState { get; private set; }
    public gameState previousState { get; private set; }

    protected void Awake()
    {
        MainManager.Manager.State = this;
        UIManager.Manager.UI = GetComponentInChildren<Canvas>();
    }

    protected void Start()
    {
        MainManager.Manager.State.SwitchState(gameState.Playing);
    }

    public void SwitchState(gameState targetState)
    {
        // Debug.Log(string.Format("Changing from {0} to {1}", currentState, targetState));
        previousState = currentState;
        currentState = targetState;
        ActionOnSwitch(currentState);

        if (OnStateSwitched != null)
        {
            OnStateSwitched(currentState, previousState);
        }
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
