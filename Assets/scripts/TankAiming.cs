using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TankAiming : MonoBehaviour
{
    [SerializeField] private float turretPanSpeed = 1f;
    [SerializeField] private float turretTiltSpeed = 1f;

    [SerializeField] private Ammo currentAmmoType = Ammo.Laser;
    [SerializeField] private TankAudioController audioController;

    private Transform parent;
    private Transform cannon;

    [SerializeField] private Sprite laserSprite;
    [SerializeField] private Sprite rocketSprite;

    public Ammo CurrentAmmoType
    {
        set
        {
            currentAmmoType = value;
            if (currentAmmoType == Ammo.Laser)
            {
                UIManager.Manager.UI.transform.Find("Crosshair").GetComponent<Image>().sprite = laserSprite;
            }
            else if (currentAmmoType == Ammo.Rocket)
            {
                UIManager.Manager.UI.transform.Find("Crosshair").GetComponent<Image>().sprite = rocketSprite;
            }
        }
    }

    private bool lockedOn, playedLockon;
    private Transform lockTarget;

    protected void Awake()
    {
        parent = transform.parent;
        cannon = transform.GetChild(0);

        lockedOn = false;
        lockTarget = null;
    }

    protected void Start()
    {
        MainManager.Manager.TankTurret = this;
    }

    protected void Update()
    {
        if (!lockedOn)
        {
            MoveTurret();
            lockedOn = CheckLockOn();
        }
        else
        {
            AutoMoveTurret();
        }

        if (Input.GetButtonDown("Z Button"))
        {
            int mask = LayerMask.GetMask("Tank");
            RaycastHit hit;

            if (Physics.Raycast(cannon.position, cannon.forward, out hit, Mathf.Infinity, mask))
            {
                // Check if target is a Tank
                ITank target = hit.collider.GetComponent<ITank>();

                if (target != null)
                {
                    // Fire at target and check if it is destroyed
                    audioController.PlayFire(currentAmmoType);
                    bool hasDied = target.Hit(currentAmmoType);
                    if (hasDied) lockedOn = false;
                }
            }
        }
    }

    private void MoveTurret()
    {
        float xAxis = Input.GetAxis("Analog Stick X");
        float yAxis = Input.GetAxis("Analog Stick Y");

        // cancel out movement rotation
        TankMovement parentMovement = parent.GetComponent<TankMovement>();
        transform.localEulerAngles -= parentMovement.rotation * parent.up;
        transform.eulerAngles += xAxis * (Time.deltaTime * turretPanSpeed) * parent.up;
        UIManager.Manager.UI.transform.Find("TurretDirection").Find("HullDirection").GetComponent<Image>().transform.eulerAngles = transform.localEulerAngles.y * Vector3.forward;

        cannon.localEulerAngles += Time.deltaTime * turretTiltSpeed * yAxis * Vector3.right;

        audioController.TurretValue = new Vector2(turretPanSpeed * xAxis, turretTiltSpeed * yAxis).magnitude;
    }

    private void AutoMoveTurret()
    {
        // Turret rotation
        Vector3 target = lockTarget.position;
        target.y = transform.position.y;

        Quaternion direction = Quaternion.LookRotation(target - transform.position, parent.up);
        transform.rotation = Quaternion.Lerp(transform.rotation, direction, Time.deltaTime);

        // Cannon rotation
        float distance = Vector3.Distance(transform.position, lockTarget.position);
        target = transform.position + distance * transform.forward;
        target.y = lockTarget.position.y;

        direction = Quaternion.LookRotation(target - transform.position, parent.up);
        cannon.rotation = Quaternion.Slerp(cannon.rotation, direction, Time.deltaTime);
    }

    private bool CheckLockOn()
    {
        RaycastHit hit;
        int mask = LayerMask.GetMask("LockOn");

        if (Physics.Raycast(cannon.position, cannon.forward, out hit, Mathf.Infinity, mask))
        {
            lockTarget = hit.transform.parent;

            ITank tank = lockTarget.GetComponent<ITank>();

            if (tank != null)
            {
                Debug.Log(currentAmmoType + "," + tank.Weakness + ", " + tank.GetType());
                if (!playedLockon)
                {
                    audioController.PlayLockon();
                    playedLockon = true;
                }
                return true && !tank.Destroyed && currentAmmoType == tank.Weakness;
            }
        }
        playedLockon = false;
        return false;
    }
}
