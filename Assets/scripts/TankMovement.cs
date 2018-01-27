using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using UnityEngine;

public class TankMovement : MonoBehaviour
{
    CharacterController controller;

    private bool isCoupled;

    private int currentGear;
    private int currentForwardDirection;
    private int currentSidewaysDirection;
    [SerializeField] private int maxrPM;

    [SerializeField] private float[] maxAcceleration;
    [SerializeField] private float[] accelerationPeriod;

    [SerializeField] private float angularVelocity;


    private float buttonPressMomentum; // liniar momentum, we're just bullshiting this.... don't ask :D


    private void Awake()
    {
        controller = GetComponent<CharacterController>();
        currentGear = (maxAcceleration.Length - 1) / 2 + 1;
        currentForwardDirection = 0;
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
        currentForwardDirection = (int)Input.GetAxis("Controlpad Vertical");

        if (currentForwardDirection != 0) buttonPressMomentum += Time.deltaTime;
        else
        {
            buttonPressMomentum -= Time.deltaTime;
            currentForwardDirection = 1;
        }

        buttonPressMomentum = Mathf.Clamp(buttonPressMomentum, 0, accelerationPeriod[currentGear]);
        Vector3 forward = currentForwardDirection * transform.forward;
        float acceleration = RPM(buttonPressMomentum);
        float magnitude = Mathf.Lerp(acceleration, controller.velocity.magnitude, Time.deltaTime);

        controller.SimpleMove(magnitude * forward);
    }

    private float RPM(float buttonPressDuration)
    {
        return 0.5f * maxAcceleration[currentGear] * (Mathf.Cos(Mathf.PI * buttonPressDuration / accelerationPeriod[currentGear]) - 1);
    }

    private void Rotation()
    {
        currentSidewaysDirection = (int)Input.GetAxis("Controlpad Horizontal");
        float rotation = 1f / (controller.velocity.magnitude + 1) * angularVelocity * currentSidewaysDirection;
        controller.transform.eulerAngles += rotation * transform.up;
    }

    private void HandleCoupling()
    {
        if (Input.GetButtonDown("R Button")) isCoupled = true;
        else if (Input.GetButtonDown("R Button")) isCoupled = false;

        if (isCoupled)
        {
            if (Input.GetButtonDown("B Button"))
            {
                currentGear++;
            }
            else if (Input.GetButtonDown("B Button"))
            {
                currentGear--;
            }
        }
    }
}
