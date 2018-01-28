
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FMOD.Studio;

public class TankAudioController : MonoBehaviour
{
    private float turretSpeedvalue;

    public float TurretValue
    {
        set { turretSpeedvalue = value; }
    }

    public void PlayLockon()
    {
        FMODUnity.RuntimeManager.PlayOneShot(lockonOneShotEvent, transform.position);
    }
    public void PlayFire(Ammo ammoType)
    {
        if (ammoType == Ammo.Laser)
        {
            FMODUnity.RuntimeManager.PlayOneShot(laserFireOneShotEvent, transform.position);
        }
        else
        {
            FMODUnity.RuntimeManager.PlayOneShot(rocketFireOneShotEvent, transform.position);
        }
    }
    public void PlayHit(bool byEnemy)
    {
        if (byEnemy)
        {
            FMODUnity.RuntimeManager.PlayOneShot(enemyHitOneShotEvent, transform.position);
        }
        else
        {
            FMODUnity.RuntimeManager.PlayOneShot(obstacleHitOneShotEvent, transform.position);
        }
    }
#if true
    [Header("Persitent events")]

    [SerializeField, FMODUnity.EventRef]
    private string brakeInstanceEvent;
    [SerializeField, FMODUnity.EventRef] private string engineEvent;
    [SerializeField, FMODUnity.EventRef] private string ambienceEvent;
    [SerializeField, FMODUnity.EventRef] private string turretMoveEvent;
    [SerializeField, FMODUnity.EventRef] private string spinMoveEvent;

    [Header("Oneshots")]

    [SerializeField, FMODUnity.EventRef]
    private string gearshiftOneShotEvent;
    [SerializeField, FMODUnity.EventRef] private string clutchOneShotEvent;
    [SerializeField, FMODUnity.EventRef] private string brakeOneShotEvent;
    [SerializeField, FMODUnity.EventRef] private string lockonOneShotEvent;
    [SerializeField, FMODUnity.EventRef] private string obstacleHitOneShotEvent;
    [SerializeField, FMODUnity.EventRef] private string enemyHitOneShotEvent;
    [SerializeField, FMODUnity.EventRef] private string laserFireOneShotEvent;
    [SerializeField, FMODUnity.EventRef] private string rocketFireOneShotEvent;

    [Header("Parameter names")]

    [SerializeField]
    private string brakeParam;
    [SerializeField] private string engineSpeedParam;
    [SerializeField] private string engineClutchParam;
    [SerializeField] private string ambienceSpeedParam;
    [SerializeField] private string turretSpeedParam;
    [SerializeField] private string spinMoveParam;

    private EventInstance brakeInstance;
    private EventInstance ambienceInstance;
    private EventInstance engineInstance;
    private EventInstance turretInstance;
    private EventInstance spinMoveInstance;

    private ParameterInstance paramTankEngineSpeed;
    private ParameterInstance paramTankEngineClutch;
    private ParameterInstance paramAmbinceSpeed;
    private ParameterInstance paramBrake;
    private ParameterInstance paramTurretSpeed;
    private ParameterInstance paramSpinMove;

    private CharacterController controller;
    private bool isBraking = false;

    private void OnEnable()
    {
        brakeInstance = FMODUnity.RuntimeManager.CreateInstance(brakeInstanceEvent);
        ambienceInstance = FMODUnity.RuntimeManager.CreateInstance(ambienceEvent);
        engineInstance = FMODUnity.RuntimeManager.CreateInstance(engineEvent);
        turretInstance = FMODUnity.RuntimeManager.CreateInstance(turretMoveEvent);
        spinMoveInstance = FMODUnity.RuntimeManager.CreateInstance(spinMoveEvent);

        brakeInstance.getParameter(brakeParam, out paramBrake);
        engineInstance.getParameter(engineSpeedParam, out paramTankEngineSpeed);
        engineInstance.getParameter(engineClutchParam, out paramTankEngineClutch);
        ambienceInstance.getParameter(ambienceSpeedParam, out paramAmbinceSpeed);
        turretInstance.getParameter(turretSpeedParam, out paramTurretSpeed);
        spinMoveInstance.getParameter(spinMoveParam, out paramSpinMove);
        
        brakeInstance.start();
        ambienceInstance.start();
        engineInstance.start();
        turretInstance.start();
        spinMoveInstance.start();


        controller = GetComponent<CharacterController>();
    }

    void Update()
    {
        paramTankEngineSpeed.setValue(controller.velocity.magnitude);
        paramAmbinceSpeed.setValue(Mathf.Clamp(controller.velocity.magnitude, 0.25f, 10f));
        paramTurretSpeed.setValue(turretSpeedvalue / 30f);
        paramSpinMove.setValue(Mathf.Abs(Input.GetAxis("Controlpad Horizontal")));

        // Brakes
        if (Input.GetButtonDown("L Button"))
        {
            Debug.Log(controller.velocity.magnitude);
            if (!isBraking && controller.velocity.magnitude > 0.05f)
            {
                isBraking = true;
                paramBrake.setValue(1f);
            }
            FMODUnity.RuntimeManager.PlayOneShot(brakeOneShotEvent, transform.position);
        }
        if (isBraking && controller.velocity.magnitude < 0.05f)
        {
            isBraking = false;
        }

        // Clutch
        if (Input.GetButtonUp("L Button") || !isBraking)
        {
            paramBrake.setValue(0);
        }
        if (Input.GetButtonDown("R Button"))
        {
            paramTankEngineClutch.setValue(1f);
            FMODUnity.RuntimeManager.PlayOneShot(gearshiftOneShotEvent, transform.position);
        }
        if (Input.GetButtonUp("R Button"))
        {
            paramTankEngineClutch.setValue(0);
            FMODUnity.RuntimeManager.PlayOneShot(gearshiftOneShotEvent, transform.position);
        }
        if (Input.GetButton("R Button") && (Input.GetButtonDown("A Button") || Input.GetButtonDown("B Button")))
        {
            FMODUnity.RuntimeManager.PlayOneShot(clutchOneShotEvent, transform.position);
        }

    }
#endif
}