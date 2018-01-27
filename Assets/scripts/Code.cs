using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Code", menuName = "Codes", order = 1)]
public class Code : ScriptableObject
{
    [SerializeField] public cControlObject.cControlObjectsState[] correctCode;
}
