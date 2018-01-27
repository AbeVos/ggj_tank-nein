using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Target : MonoBehaviour, ITank
{
	[SerializeField] private Ammo ammoType = Ammo.Laser;
	[SerializeField] private Ammo weakness = Ammo.Laser;

	public Ammo Weakness
	{
		get { return weakness; }
	}

	public bool Hit(Ammo ammoType)
	{
		if (ammoType == weakness)
		{
			Debug.Log("Fuckkut dat deed pijn, eikel");
			Destroy(gameObject);
			return true;
		}
		else
		{
			Debug.Log("Niets kan mij bezeren!");
			return false;
		}
	}
}
