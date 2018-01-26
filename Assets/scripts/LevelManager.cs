using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelManager : MonoBehaviour
{
	private static LevelManager _self;

	private string currentScene = null;

	public static LevelManager Manager
	{
		get { return LevelManager._self; }
	}

	public void LoadInitialLevel()
	{
		LoadLevel("splash_screen");
	}

	public void LoadLevel(string name)
	{
		if (currentScene != null)
		{
			SceneManager.UnloadSceneAsync(currentScene);
		}

		currentScene = name;
		SceneManager.LoadScene(name);
	}
}
