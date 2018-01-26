using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TankMovement : MonoBehaviour
{
    public enum verticalMovementState
    {
        FourthReverse,
        ThirdReverse,
        SecondReverse,
        FirstReverse,
        Idle,
        FirstGear,
        SecondGear,
        ThirdGear,
        FourthGear,
        FifthGear
    };

    private verticalMovementState currentIdleMovementState;

    CharacterController controller;

    [SerializeField] private int rPM;
    [SerializeField] private int maxrPM;
    [SerializeField] private float speed;

    private void Awake()
    {
        controller = GetComponent<CharacterController>();
        currentIdleMovementState = verticalMovementState.Idle;
    }

    void Update()
    {
        switch (currentIdleMovementState)
        {
            case verticalMovementState.FourthReverse:
                break;
            case verticalMovementState.ThirdReverse:
                break;
            case verticalMovementState.SecondReverse:
                break;
            case verticalMovementState.FirstReverse:
                break;
            case verticalMovementState.Idle:
                break;
            case verticalMovementState.FirstGear:
                break;
            case verticalMovementState.SecondGear:
                break;
            case verticalMovementState.ThirdGear:
                break;
            case verticalMovementState.FourthGear:
                break;
            case verticalMovementState.FifthGear:
                break;
            default:
                throw new ArgumentOutOfRangeException();
        }
        Vector3 forward = transform.TransformDirection(Vector3.forward);
        float curSpeed = speed * Input.GetAxis("Controlpad Vertical");
        controller.SimpleMove(forward * curSpeed);
    }
}
