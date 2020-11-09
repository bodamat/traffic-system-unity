using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Waypoint : MonoBehaviour
{
    public Waypoint previousWaypoint;
    public Waypoint nextWaypoint;
    public TrafficLight trafficLight;

    [Range(0f, 5f)]
    public float width = 1f;

    public List<Waypoint> branches = new List<Waypoint>();

    [Range(0f, 1f)]
    public float branchRatio = 0.5f;

    public Vector3 GetPosition(float ownWidth = 1f)
	{
        Vector3 _minBound = transform.position + transform.right * (width / 2f) * ownWidth;
        Vector3 _maxBound = transform.position - transform.right * (width / 2f) * ownWidth;

        return Vector3.Lerp(_minBound, _maxBound, Random.Range(0f, 1f));
	}
}
