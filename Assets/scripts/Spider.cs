using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spider : Enemy
{
	[SerializeField] private float wanderRange = 5f;
	[SerializeField] private float minimumRange = 5f;

	private Transform player;
	private UnityEngine.AI.NavMeshAgent agent;
	private Vector3 startPosition;

	public override Ammo Weakness
	{
		get { return Ammo.Laser; }
	}

	protected override void Awake()
	{
		base.Awake();

		agent = GetComponent<UnityEngine.AI.NavMeshAgent>();
		startPosition = transform.position;
	}

	protected override void Start()
	{
		base.Start();

		if (MainManager.Manager.TankHull != null)
			player = MainManager.Manager.TankHull.transform;
	}

	protected void Update()
	{
		if (player == null)
			player = MainManager.Manager.TankHull.transform;

		if (currentState == State.Idle)
		{
			if (time >= 2f)
			{
				SelectDestination();
				SetState(State.Patrol);
			}
		}
		else if (currentState == State.Patrol)
		{
			if (agent.remainingDistance < 1f)
			{
				SetState(State.Idle);
			}
		}
		else if (currentState == State.Pursuit)
		{
			agent.SetDestination(player.position);

			if (agent.remainingDistance <= minimumRange)
			{
				agent.SetDestination(transform.position);
				SetState(State.Attack);
			}
		}
		else if (currentState == State.Attack)
		{
			Quaternion rotation = Quaternion.LookRotation(player.position - turret.position, Vector3.up);
			turret.rotation = Quaternion.Slerp(turret.rotation, rotation, 0.5f * Time.deltaTime);

			RaycastHit hit;

			if (Physics.Raycast(turret.position, turret.forward, out hit))
			{
				TankArmor hull = hit.transform.GetComponent<TankArmor>();

				if (hull != null)
				{
					Debug.Log("Knal");
					hull.Hit(CurrentAmmo);

					SelectDestination();
					SetState(State.Patrol);
				}
			}
		}
		else if (currentState == State.Destroyed)
		{
			if (time >= 2f)
			{
				Destroy(gameObject);
			}
		}

		if (currentState == State.Idle || currentState == State.Patrol)
		{
			if (Vector3.Distance(player.position, transform.position) < detectionDistance)
				SetState(State.Pursuit);
		}

		time += Time.deltaTime;
	}

	private void SelectDestination()
	{
		agent.SetDestination(startPosition + wanderRange * Random.insideUnitSphere);
	}
}
