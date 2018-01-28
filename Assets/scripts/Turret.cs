using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Turret : Enemy
{
	private Transform player;

	protected override void Start()
	{
		base.Start();

		player = MainManager.Manager.TankHull.transform;
	}

	protected void Update()
	{

		if (currentState == State.Idle)
		{
			if (Vector3.Distance(player.position, transform.position) < detectionDistance)
			{
				SetState(State.Pursuit);
			}
		}
		else if (currentState == State.Pursuit)
		{
			Quaternion rotation = Quaternion.LookRotation(turret.position - player.position, Vector3.up);
			turret.rotation = Quaternion.Slerp(turret.rotation, rotation, 0.5f * Time.deltaTime);

			RaycastHit hit;

			if (Physics.Raycast(turret.position, -turret.forward, out hit))
			{
				TankArmor hull = hit.transform.GetComponent<TankArmor>();

				if (hull != null)
				{
					Debug.Log("Knal");
					hull.Hit(CurrentAmmo);

					SetState(State.Attack);
				}
			}
		}
		else if (currentState == State.Attack)
		{
			if (time >= reloadTime)
			{
				SetState(State.Idle);
			}
		}
		else if (currentState == State.Destroyed)
		{
			if (time >= 1f)
			{
				Destroy(gameObject);
			}
		}

		time += Time.deltaTime;
	}
}
