using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour, ITank
{
	protected enum State
	{
		Inactive,
		Idle,
		Patrol,
		Pursuit,
		Attack,
		Destroyed
	}

	protected State currentState = State.Inactive;
	protected Transform turret;

	public Ammo Weakness
	{
		get { return Ammo.Rocket; }
	}

	protected void Awake()
	{
		turret = transform.GetChild(0);
	}

	protected virtual void Start()
	{
		MainManager.Manager.State.OnStateSwitched += OnLevelStateChanged;
	}

	protected void OnDisable()
	{
		MainManager.Manager.State.OnStateSwitched -= OnLevelStateChanged;
	}

	protected void OnLevelStateChanged(StateManager.gameState newState, StateManager.gameState oldState)
	{
		if (newState == StateManager.gameState.Playing)
		{
			SetState(State.Idle);
		}
	}

	public bool Hit(Ammo ammoType)
	{
		if (ammoType == Weakness)
		{
			return true;
		}

		return false;
	}

	protected void SetState(State newState)
	{
		State oldState = currentState;
		currentState = newState;
	}
}
