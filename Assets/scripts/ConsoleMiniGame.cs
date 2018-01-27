using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ConsoleMiniGame : MonoBehaviour
{
    [SerializeField] private cControlObject[] cControlObjects = new cControlObject[6];
    [SerializeField] private GameObject consoleBackground;
    [SerializeField] private Code[] ammoCodes;
    [SerializeField] private Code[] engineCodes;

    private bool consoleIsVisible;
    private int currentcControlObjectIndex = 0;

    void Awake()
    {
        consoleIsVisible = false;
        consoleBackground.SetActive(false);
    }

    void Update()
    {
        ToggleConsoleVisibility();

        if (consoleIsVisible && currentcControlObjectIndex < 6)
        {
            if (Input.GetButtonDown("C Down"))
            {
                cControlObjects[currentcControlObjectIndex].SetState(cControlObject.cControlObjectsState.Down);
                currentcControlObjectIndex++;
            }
            else if (Input.GetButtonDown("C Left"))
            {
                cControlObjects[currentcControlObjectIndex].SetState(cControlObject.cControlObjectsState.Left);
                currentcControlObjectIndex++;
            }
            else if (Input.GetButtonDown("C Right"))
            {
                cControlObjects[currentcControlObjectIndex].SetState(cControlObject.cControlObjectsState.Right);
                currentcControlObjectIndex++;
            }
            else if (Input.GetButtonDown("C Up"))
            {
                cControlObjects[currentcControlObjectIndex].SetState(cControlObject.cControlObjectsState.Up);
                currentcControlObjectIndex++;
            }
        }
        else
        {
            CheckCode();
        }
    }

    private void ToggleConsoleVisibility()
    {
        if (Input.GetButtonDown("Start Button"))
        {
            consoleIsVisible = true;
            consoleBackground.SetActive(true);
        }
        else if (Input.GetButtonUp("Start Button"))
        {
            CheckCode();
        }
    }

    private void CheckCode()
    {

        ResetCode();
    }

    private void ResetCode()
    {
        foreach (var cControlObject in cControlObjects)
        {
            consoleIsVisible = false;
            consoleBackground.SetActive(false);
            currentcControlObjectIndex = 0;
            cControlObject.SetState(cControlObject.cControlObjectsState.Empty);
        }
    }
}
