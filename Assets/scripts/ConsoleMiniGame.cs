using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ConsoleMiniGame : MonoBehaviour
{
    [SerializeField] private List<cControlObject.cControlObjectsState> codeToCheck = new List<cControlObject.cControlObjectsState>();
    [SerializeField] private cControlObject[] cControlObjects = new cControlObject[6];
    [SerializeField] private GameObject consoleBackground;
    [SerializeField] private Code[] Codes;
    [SerializeField] private MotorCode[] engineCodes;

    private bool canOpenConsole = true;
    private bool consoleIsVisible;
    private bool correctCode = false;
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
            if (Input.GetButtonDown("C Down")) UpdateVirtualCursor(cControlObject.cControlObjectsState.Down);
            else if (Input.GetButtonDown("C Left")) UpdateVirtualCursor(cControlObject.cControlObjectsState.Left);
            else if (Input.GetButtonDown("C Right")) UpdateVirtualCursor(cControlObject.cControlObjectsState.Right);
            else if (Input.GetButtonDown("C Up")) UpdateVirtualCursor(cControlObject.cControlObjectsState.Up);
        }
    }

    private void UpdateVirtualCursor(cControlObject.cControlObjectsState newState)
    {
        cControlObjects[currentcControlObjectIndex].SetState(newState);
        codeToCheck.Add(newState);
        currentcControlObjectIndex++;
    }

    private void ToggleConsoleVisibility()
    {
        if (Input.GetButtonDown("Start Button") && canOpenConsole) 
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
        Code foundCode = null;
        foreach (var code in Codes)
        {
            if (code.correctCode.Length != codeToCheck.Count) continue;

            int correctKeys = 0;
            for (int i = 0; i < code.correctCode.Length; i++)
            {
                if (code.correctCode[i] != codeToCheck[i])
                {
                    continue;
                }
                else
                {
                    correctKeys++;
                }
            }

            if (code.correctCode.Length == correctKeys)
            {
                correctCode = true;
                foundCode = code;
                continue;
            }
        }
        if (!correctCode)
        {
            canOpenConsole = false;
            consoleBackground.GetComponent<Image>().color = new Color(1f,0.2f, 0.2f);
        }
        else
        {
            canOpenConsole = false;
            consoleBackground.GetComponent<Image>().color = new Color(0.3f,1f, 0.3f);
            ApplyCode(foundCode);
        }
        // event FMOD
        Invoke("ResetCode", 0.4f);
    }

    private void ApplyCode(Code foundCode)
    {
        if (foundCode.GetType() == typeof(AmmoCode))
        {
            AmmoCode ammoCode = (AmmoCode) foundCode;

            MainManager.Manager.TankTurret.CurrentAmmoType = ammoCode.CurrentAmmoType;
        }
        else if (foundCode.GetType() == typeof(MotorCode))
        {
            MotorCode motorCode = (MotorCode) foundCode;

            // TODO: fix hier nog iets van een motorcode ofso, is wel fijn I guess
        }
    }

    private void ResetCode()
    {
        foreach (var cControlObject in cControlObjects)
        {
            consoleIsVisible = false;
            consoleBackground.SetActive(false);
            currentcControlObjectIndex = 0;
            cControlObject.SetState(cControlObject.cControlObjectsState.Empty);
            codeToCheck.Clear();
            correctCode = false;
            consoleBackground.GetComponent<Image>().color = Color.white;
            canOpenConsole  = true;
        }
    }
}
