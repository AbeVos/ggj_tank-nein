using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ConsoleMiniGame : MonoBehaviour
{
    #if true
    [SerializeField] private List<cControlObject.cControlObjectsState> codeToCheck = new List<cControlObject.cControlObjectsState>();
    [SerializeField] private cControlObject[] cControlObjects = new cControlObject[6];
    [SerializeField] private GameObject consoleBackground;
    [SerializeField] private Code[] Codes;
    [SerializeField] private MotorCode[] engineCodes;

    [SerializeField, FMODUnity.EventRef] private string buttonPressEvent;
	[SerializeField, FMODUnity.EventRef] private string codeTypeEvent;
	[SerializeField, FMODUnity.EventRef] private string codeRightEvent;
	[SerializeField, FMODUnity.EventRef] private string CodeWrongEvent;

    private bool canOpenConsole = true;
    private bool consoleIsVisible;
    private bool correctCode = false;
    private int currentcControlObjectIndex = 0;
    private Image backgroundImage;

    void Awake()
    {
        consoleIsVisible = false;
        backgroundImage = consoleBackground.GetComponent<Image>();
        backgroundImage.color = new Color(1,1,1,0);
    }

    void Update()
    {
        if (!canOpenConsole) {return;}
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
        FMODUnity.RuntimeManager.PlayOneShot(codeTypeEvent, transform.position);
    }

    private void ToggleConsoleVisibility()
    {
        if (Input.GetButtonDown("Start Button")) 
        {
            consoleIsVisible = true;
            backgroundImage.CrossFadeAlpha(1.0f,0.2f,false);
            FMODUnity.RuntimeManager.PlayOneShot(buttonPressEvent, transform.position);
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
            FMODUnity.RuntimeManager.PlayOneShot(CodeWrongEvent, transform.position);
        }
        else
        {
            canOpenConsole = false;
            backgroundImage.CrossFadeColor(new Color(0.3f,1f, 0.3f), 1f, false);
            FMODUnity.RuntimeManager.PlayOneShot(codeRightEvent, transform.position);
            ApplyCode(foundCode);
        }
        Invoke("ResetCode", 1f);
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
            consoleBackground.GetComponent<Image>().CrossFadeAlpha(0.0f,0.4f,false);
            currentcControlObjectIndex = 0;
            cControlObject.SetState(cControlObject.cControlObjectsState.Empty);
            codeToCheck.Clear();
            correctCode = false;
            consoleBackground.GetComponent<Image>().color = Color.white;
            canOpenConsole  = true;
        }
    }
    #endif
}
