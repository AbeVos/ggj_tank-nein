using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class cControlObject : MonoBehaviour
{
    [SerializeField] private Sprite[] cControlSprites = new Sprite[4];
    private Image thisImage;

    [Serializable]
    public enum cControlObjectsState
    {
        Empty = -1,
        Down,
        Right,
        Left,
        Up,
    }

    private cControlObjectsState currentState;

    private void Awake()
    {
        currentState = cControlObjectsState.Empty;
        thisImage = GetComponent<Image>();
        thisImage.enabled = false;
    }

    public void SetState(cControlObjectsState targetState)
    {
        currentState = targetState;
        SetCorrectSprite(currentState);
    }

    private void SetCorrectSprite(cControlObjectsState stateToMatchSpriteTo)
    {
        if (stateToMatchSpriteTo == cControlObjectsState.Empty)
        {
            thisImage.enabled = false;
        }
        else
        {
            thisImage.enabled = true;
            thisImage.sprite = cControlSprites[(int)stateToMatchSpriteTo];
        }
    }
}
