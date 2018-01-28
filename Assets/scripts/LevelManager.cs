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

	protected void Awake()
	{
		_self = this;
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

		if (SceneManager.sceneCount > 1)
		{
			Debug.Log("A level has already been loaded.");
			return;
		}

		currentScene = name;
		SceneManager.LoadScene(name, LoadSceneMode.Additive);
		MainManager.Manager.music.StartMainMusic = 0.1f;
	}
}
