using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainManager : MonoBehaviour
{
	private static MainManager _self;
	private static StateManager _stateManager;

	public static MainManager Manager
	{
		get { return MainManager._self; }
	}

	public static StateManager State
	{
		get { return MainManager._stateManager; }
		set { MainManager.Manager._stateManager = value; }
	}

	protected void Awake()
	{
		MainManager._self = this;

		LevelManager.Manager.LoadInitialLevel();
	}
}
