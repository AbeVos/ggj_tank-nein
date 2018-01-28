using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TankArmor : MonoBehaviour, ITank
{
    private float health = 100f;
    private float displayedHealth = 100f;
    private Image armorBar;

    [SerializeField] private float damage = 20f;
    [SerializeField] private TankAudioController audioController;

    public Ammo CurrentAmmo
    {
        get { return Ammo.Laser; }
    }

    public Ammo Weakness
    {
        get { return Ammo.Laser; }
    }

    public bool Destroyed
    {
        get { return false; }
    }

    protected void Start()
    {
        MainManager.Manager.TankHull = this;
        armorBar = UIManager.Manager.UI.transform.Find("Armor").Find("ArmorBar").GetComponent<Image>();
    }

    protected void Update()
    {
        displayedHealth = Mathf.Lerp(displayedHealth, health, 2 * Time.deltaTime);
        armorBar.fillAmount = displayedHealth / 100f;

        if (Input.GetKeyDown(KeyCode.Space)) { Hit(Ammo.Laser); }

        if (displayedHealth <= 0)
        {
            Lose();
        }
    }

    private void Lose()
    {
        LevelManager.Manager.LoadLevel("lose screen");
    }

    public bool Hit(Ammo ammoType)
    {
        if (ammoType == Weakness)
        {
            float randomValue = Vector3.Dot(Random.insideUnitSphere, Vector3.left);

            health -= damage + randomValue;
            Camera.main.GetComponent<ScreenShake>().shakeDuration = 1f;
            audioController.PlayHit(true);
            audioController.HealthValue = 1 - (health / 100f);
            return true;
        }
        return false;
    }
}
