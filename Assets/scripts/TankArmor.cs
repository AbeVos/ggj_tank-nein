using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TankArmor : MonoBehaviour, ITank
{
	public Ammo CurrentAmmo
	{
		get { return Ammo.Laser; }
	}

	public Ammo Weakness
	{
		get { return Ammo.Rocket; }
	}

	public bool Destroyed
	{
		get { return false; }
	}

	protected void Awake()
	{
		MainManager.Manager.TankHull = this;
	}

	public bool Hit(Ammo ammoType)
	{
		if (ammoType == Weakness)
		{
			return true;
		}

		return false;
	}
}
