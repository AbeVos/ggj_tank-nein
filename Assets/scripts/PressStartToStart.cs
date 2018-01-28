using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PressStartToStart : MonoBehaviour
{
    void Update()
    {
        if (Input.anyKey)
        {
            LevelManager.Manager.LoadLevel("main_scene");
        }
    }
}
