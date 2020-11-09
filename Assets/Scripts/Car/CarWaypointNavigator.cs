using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarWaypointNavigator : MonoBehaviour
{
	public Waypoint currentWaypoint;
	public float waypointReachedRange = 1f;

	private CarEngine _carController;
	private int _direction;

	private void Awake()
	{
		//_direction = Mathf.RoundToInt(Random.Range(0f, 1f));
		_direction = 0;
		_carController = GetComponent<CarEngine>();
	}

	// Start is called before the first frame update
	void Start()
	{
		_carController.SetTarget(currentWaypoint.GetPosition(0f));
	}

	// Update is called once per frame
	void Update()
	{
		if (currentWaypoint.trafficLight)
		{
			if (currentWaypoint.trafficLight.lightColor == TrafficLight.LightColor.Red)
			{
				_carController.SetBrake(true);
				return;
			}
			else if (currentWaypoint.trafficLight.lightColor == TrafficLight.LightColor.Green)
			{
				_carController.SetBrake(false);
			}
		}

		if (isTargetReached())
		{
			bool _shouldBranch = false;

			if (currentWaypoint.branches != null && currentWaypoint.branches.Count > 0)
			{
				_shouldBranch = Random.Range(0f, 1f) <= currentWaypoint.branchRatio ? true : false;
			}

			if (_shouldBranch)
			{
				currentWaypoint = currentWaypoint.branches[Random.Range(0, currentWaypoint.branches.Count - 1)];
			}
			else
			{
				if (_direction == 0)
				{
					if (currentWaypoint.nextWaypoint != null)
					{
						currentWaypoint = currentWaypoint.nextWaypoint;
					}
				}
				else if (_direction == 1)
				{
					if (currentWaypoint.previousWaypoint != null)
					{
						currentWaypoint = currentWaypoint.previousWaypoint;
					}
				}
			}

			_carController.SetTarget(currentWaypoint.GetPosition(0f));
		}
	}

	private bool isTargetReached()
	{
		if (currentWaypoint.trafficLight)
		{
			if (currentWaypoint.trafficLight.lightColor != TrafficLight.LightColor.Red)
			{
				return Vector3.Distance(transform.position, currentWaypoint.GetPosition()) < waypointReachedRange;
			}
		}
		else
		{
			return Vector3.Distance(transform.position, currentWaypoint.GetPosition()) < waypointReachedRange;
		}

		return false;
	}
}
