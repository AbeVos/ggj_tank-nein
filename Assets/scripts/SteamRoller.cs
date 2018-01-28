using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;

public class SteamRoller : MonoBehaviour
{
    public WheelCollider leftWheel;
    public WheelCollider rightWheel;

    public float maxMotorTorque; // maximum torque the motor can apply to wheel
    public float maxSteeringAngle; // maximum steer angle the wheel can have

    public void Awake()
    {
        leftWheel.ConfigureVehicleSubsteps(5,12,15);
        rightWheel.ConfigureVehicleSubsteps(5, 12, 15);
    }

    public void FixedUpdate()
    {
        float motor = maxMotorTorque * Input.GetAxis("Controlpad Vertical");
        float steering = maxSteeringAngle * Input.GetAxis("Controlpad Horizontal");
        float motorLeft = 0;
        float motorRight = 0;

        int yAxis = (int)Input.GetAxis("Controlpad Vertical");
        int xAxis = (int)Input.GetAxis("Controlpad Horizontal");

        motorRight += maxMotorTorque * yAxis;
        motorLeft += maxMotorTorque * yAxis;

        motorRight += maxMotorTorque * xAxis;
        motorLeft -= maxMotorTorque * xAxis;

        leftWheel.motorTorque = motorLeft;
        rightWheel.motorTorque = motorRight;

        //foreach (AxleInfo axleInfo in axleInfos)
        //{
        //    if (axleInfo.steering)
        //    {
        //        axleInfo.leftWheel.steerAngle = steering;
        //        axleInfo.rightWheel.steerAngle = steering;
        //    }
        //    if (axleInfo.motor)
        //    {
        //        axleInfo.leftWheel.motorTorque = motor;
        //        axleInfo.rightWheel.motorTorque = motor;
        //    }
        //}
    }
}

[System.Serializable]
public class AxleInfo
{
    public WheelCollider leftWheel;
    public WheelCollider rightWheel;
    public bool motor; // is this wheel attached to motor?
    public bool steering; // does this wheel apply steer angle?
}