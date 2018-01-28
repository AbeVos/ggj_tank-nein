using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum Ammo
{
	Laser,
	Rocket
}

public interface ITank
{
	Ammo CurrentAmmo { get; }
	Ammo Weakness { get; }
	bool Destroyed { get; }

	bool Hit(Ammo ammoType);
}
