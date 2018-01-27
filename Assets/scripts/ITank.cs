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
	Ammo Weakness { get; }

	bool Hit(Ammo ammoType);
}
