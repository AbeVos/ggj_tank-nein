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

	[SerializeField] protected float detectionDistance = 10f;
	[SerializeField] protected float reloadTime = 10f;

	protected State currentState = State.Inactive;
	protected Transform turret;
	protected float time = 0f;

	public Ammo CurrentAmmo
	{
		get { return Ammo.Laser; }
	}

	public virtual Ammo Weakness
	{
		get { return Ammo.Laser; }
	}

	public bool Destroyed
	{
		get { return currentState == State.Destroyed; }
	}

	protected virtual void Awake()
	{
		turret = transform.GetChild(0);
	}

	protected virtual void Start()
	{
		Debug.Log("Subscribe");
		MainManager.Manager.State.OnStateSwitched += OnLevelStateChanged;
	}

	protected void OnDisable()
	{
		MainManager.Manager.State.OnStateSwitched -= OnLevelStateChanged;
	}

	protected void OnLevelStateChanged(StateManager.gameState newState, StateManager.gameState oldState)
	{
		Debug.Log(newState);
		if (newState == StateManager.gameState.Playing)
		{
			SetState(State.Idle);
		}
	}

	public bool Hit(Ammo ammoType)
	{
		if (ammoType == Weakness)
		{
			SetState(State.Destroyed);
			return true;
		}

		return false;
	}

	protected void SetState(State newState)
	{
		State oldState = currentState;
		currentState = newState;

		time = 0f;
	}
}
