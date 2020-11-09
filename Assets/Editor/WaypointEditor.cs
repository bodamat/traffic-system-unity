using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[InitializeOnLoad()]
public class WaypointEditor : MonoBehaviour
{
	private static Vector3 WaypointTransformRight(Waypoint waypoint)
	{
		return waypoint.transform.right * waypoint.width / 2f;
	}

    [DrawGizmo(GizmoType.NonSelected | GizmoType.Selected | GizmoType.Pickable)]
    public static void OnDrawSceneGizmo(Waypoint waypoint, GizmoType gizmoType)
	{
		if ((gizmoType & GizmoType.Selected) != 0)
		{
			Gizmos.color = Color.green;
		}
		else
		{
			Gizmos.color = Color.yellow;
		}

		Gizmos.DrawSphere(waypoint.transform.position, 0.3f);

		Gizmos.color = Color.white;
		Gizmos.DrawLine(waypoint.transform.position + WaypointTransformRight(waypoint), waypoint.transform.position - WaypointTransformRight(waypoint));

		if (waypoint.previousWaypoint != null)
		{
			Gizmos.color = Color.red;
			Vector3 offset = WaypointTransformRight(waypoint);
			Vector3 offsetTo = WaypointTransformRight(waypoint.previousWaypoint);

			DrawArrow.ForGizmo(waypoint.previousWaypoint.transform.position + offsetTo, waypoint.transform.position + offset);
		}
		if (waypoint.nextWaypoint != null)
		{
			Gizmos.color = Color.green;
			Vector3 offset = -WaypointTransformRight(waypoint);
			Vector3 offsetTo = -WaypointTransformRight(waypoint.nextWaypoint);
			DrawArrow.ForGizmo(waypoint.transform.position + offset, waypoint.nextWaypoint.transform.position + offsetTo);
		}

		if (waypoint.branches != null)
		{
			foreach (Waypoint branch in waypoint.branches)
			{
				Gizmos.color = Color.blue;
				//Gizmos.DrawLine(waypoint.transform.position, branch.transform.position);
				DrawArrow.ForGizmo(waypoint.transform.position, branch.transform.position);
			}
		}

		if (waypoint.trafficLight)
		{
			switch (waypoint.trafficLight.lightColor)
			{
				case TrafficLight.LightColor.Red: Gizmos.color = Color.red;
					break;
				case TrafficLight.LightColor.Yellow: Gizmos.color = Color.yellow;
					break;
				case TrafficLight.LightColor.Green: Gizmos.color = Color.green;
					break;
				default:
					Gizmos.color = Color.red;
					break;
			}

			Gizmos.DrawLine(waypoint.transform.position, waypoint.trafficLight.transform.position + waypoint.trafficLight.transform.up * 5f);
		}
	}
    
}
