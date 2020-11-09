using UnityEngine;
using System.Collections;

public static class DrawArrow
{
	public static void ForGizmo(Vector3 pos, Vector3 endPos, float arrowHeadLength = 1f, float arrowHeadAngle = 20.0f)
	{
		Gizmos.DrawLine(pos, endPos);
        DrawArrowEnd(true, pos, endPos, Gizmos.color, arrowHeadLength, arrowHeadAngle);
    }

    private static void DrawArrowEnd(bool gizmos, Vector3 pos, Vector3 endPos, Color color, float arrowHeadLength, float arrowHeadAngle)
    {

        Vector3 endPoint = (endPos - pos).normalized * ((pos - endPos).magnitude / 2);

        Vector3 right = Quaternion.LookRotation(endPoint) * Quaternion.Euler(arrowHeadAngle, 0, 0) * Vector3.back;
        Vector3 left = Quaternion.LookRotation(endPoint) * Quaternion.Euler(-arrowHeadAngle, 0, 0) * Vector3.back;
        Vector3 up = Quaternion.LookRotation(endPoint) * Quaternion.Euler(0, arrowHeadAngle, 0) * Vector3.back;
        Vector3 down = Quaternion.LookRotation(endPoint) * Quaternion.Euler(0, -arrowHeadAngle, 0) * Vector3.back;
        if (gizmos)
        {
            Gizmos.color = color;
            Gizmos.DrawRay(pos + endPoint, right * arrowHeadLength);
            Gizmos.DrawRay(pos + endPoint, left * arrowHeadLength);
            Gizmos.DrawRay(pos + endPoint, up * arrowHeadLength);
            Gizmos.DrawRay(pos + endPoint, down * arrowHeadLength);
        }
        else
        {
            Debug.DrawRay(pos + endPos, right * arrowHeadLength, color);
            Debug.DrawRay(pos + endPos, left * arrowHeadLength, color);
            Debug.DrawRay(pos + endPos, up * arrowHeadLength, color);
            Debug.DrawRay(pos + endPos, down * arrowHeadLength, color);
        }
    }
}