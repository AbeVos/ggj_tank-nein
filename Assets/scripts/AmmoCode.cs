using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "AmmoCode", menuName = "Codes/AmmoCodes", order = 3)]
public class AmmoCode : Code
{
    [SerializeField] private Ammo currentAmmoType;
    public Ammo CurrentAmmoType
    {
        get { return currentAmmoType; }
    }
}
