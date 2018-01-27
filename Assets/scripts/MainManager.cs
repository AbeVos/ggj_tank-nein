﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainManager : MonoBehaviour
{
	private static MainManager _self;
	private StateManager _stateManager;

	public static MainManager Manager
	{
		get { return MainManager._self; }
	}

	public StateManager State
	{
		get { return _stateManager; }
		set { _stateManager = value; }
	}

	protected void Awake()
	{
		MainManager._self = this;
	}

	protected void Start()
	{
		LevelManager.Manager.LoadInitialLevel();
	}
}
