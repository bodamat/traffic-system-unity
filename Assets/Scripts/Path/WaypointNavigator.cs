using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaypointNavigator : MonoBehaviour
{
    CharacterNavigationController _controller;
    public Waypoint currentWaypoint;

    int _direction;

	private void Awake()
	{
        _direction = Mathf.RoundToInt(Random.Range(0f, 1f));
        _controller = GetComponent<CharacterNavigationController>();
	}

	// Start is called before the first frame update
	void Start()
    {
        _controller.SetDestination(currentWaypoint.GetPosition());
    }

    // Update is called once per frame
    void Update()
    {
        if (_controller.reachedDestination)
		{
            bool _shouldBranch = false;

            if(currentWaypoint.branches != null && currentWaypoint.branches.Count > 0)
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
                    else
					{
                        currentWaypoint = currentWaypoint.previousWaypoint;
                        _direction = 1;
					}
			    }
                else if (_direction == 1)
			    {
                    if (currentWaypoint.previousWaypoint != null)
					{
                        currentWaypoint = currentWaypoint.previousWaypoint;
					}
                    else
					{
                        currentWaypoint = currentWaypoint.nextWaypoint;
                        _direction = 0;
					}
			    }
			}

            _controller.SetDestination(currentWaypoint.GetPosition());
		}
    }
}
