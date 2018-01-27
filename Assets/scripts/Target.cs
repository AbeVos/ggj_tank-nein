using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Target : MonoBehaviour, ITank
{
	[SerializeField] private Ammo ammoType = Ammo.Laser;
	[SerializeField] private Ammo weakness = Ammo.Laser;

	private bool destroyed;

	public Ammo CurrentAmmo
	{
		get { return Ammo.Laser; }
	}

	public Ammo Weakness
	{
		get { return weakness; }
	}

	public bool Destroyed
	{
		get { return destroyed; }
	}

	public bool Hit(Ammo ammoType)
	{
		if (ammoType == weakness)
		{
			Debug.Log("Fuckkut dat deed pijn, eikel");
			Destroy(gameObject);
			destroyed = true;
			return true;
		}
		else
		{
			Debug.Log("Niets kan mij bezeren!");
			destroyed = false;
			return false;
		}
	}
}
