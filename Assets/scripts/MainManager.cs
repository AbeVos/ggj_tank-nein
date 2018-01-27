using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainManager : MonoBehaviour
{
	private static MainManager _self;
	private StateManager _stateManager;

    public TankAiming TankTurret { get; set; }
	public TankArmor TankHull { get; set; }

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
