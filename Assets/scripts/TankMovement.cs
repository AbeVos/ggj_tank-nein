using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using UnityEngine;

public class TankMovement : MonoBehaviour
{
    CharacterController controller;

    private enum Gear
    {
        Reverse,
        Free,
        GearOne,
        GearTwo,
        GearThree,
        GearFour,
        GearFive
    }

    private Gear currentGear;

    private bool isCoupled;

    private int requiredGear;
    private int previousForwardInput;
    private int currentForwardInput;
    private int currentSidewaysInput;

    [SerializeField] private float[] maxAcceleration;
    [SerializeField] private float[] accelerationPeriod;

    [SerializeField] private float angularVelocity;
    [Range(0, 1)] [SerializeField] private float friction;


    private float buttonPressMomentum; // liniar momentum, we're just bullshiting this.... don't ask :D


    private void Awake()
    {
        controller = GetComponent<CharacterController>();
        currentGear = Gear.Free;
        currentForwardInput = 0;
        isCoupled = false;
    }

    void Update()
    {
        ForwardMovement();
        Rotation();
        HandleCoupling();
    }

    private void ForwardMovement()
    {
        currentForwardInput = (int)Input.GetAxis("Controlpad Vertical");

        if (currentGear == Gear.Reverse) HandleForwardInput(1);
        else if ((int)currentGear > 1) HandleForwardInput(-1);

        buttonPressMomentum = Mathf.Clamp(buttonPressMomentum, 0, accelerationPeriod[(int)currentGear]);
        float acceleration = RPM(buttonPressMomentum);
        Vector3 velocity = friction * controller.velocity + acceleration * transform.forward;

        controller.SimpleMove(velocity);
    }

    private void HandleForwardInput(int input)
    {
        if (currentForwardInput == input) buttonPressMomentum += Time.deltaTime;
        else buttonPressMomentum -= Time.deltaTime;
    }

    private float RPM(float buttonPressDuration)
    {
        return 0.5f * maxAcceleration[(int)currentGear] * (Mathf.Cos(Mathf.PI * buttonPressDuration / accelerationPeriod[(int)currentGear]) - 1);
    }

    private void Rotation()
    {
        currentSidewaysInput = (int)Input.GetAxis("Controlpad Horizontal");
        float rotation = -1f / (controller.velocity.magnitude + 1) * angularVelocity * currentSidewaysInput;
        controller.transform.eulerAngles += rotation * transform.up;
    }

    private void HandleCoupling()
    {
        if (Input.GetButtonDown("R Button")) isCoupled = true;
        else if (Input.GetButtonUp("R Button")) isCoupled = false;

        if (isCoupled)
        {
            if (Input.GetButtonDown("A Button") && (int)currentGear < maxAcceleration.Length - 1)
            {
                currentGear++;
                Debug.Log("Changed gear to " + currentGear);
            }
            else if (Input.GetButtonDown("B Button") && (int)currentGear > 0)
            {
                currentGear--;
                Debug.Log("Changed gear to " + currentGear);
            }
        }
    }
}
