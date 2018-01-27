using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
	private static UIManager _self;

	private Canvas canvas;

	public static UIManager Manager
	{
		get { return _self; }
	}

	public Canvas UI
	{
		get { return canvas; }
		set { canvas = value; }
	}

	protected void Awake()
	{
		_self = this;
	}
}
