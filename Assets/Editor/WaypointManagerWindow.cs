using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System;

public class WaypointManagerWindow : EditorWindow
{
	public Transform waypointRoot;

	float createWaypointOffset = 3f;

	[MenuItem("Tools/Waypoint Editor")]
    public static void Open()
	{
		GetWindow<WaypointManagerWindow>();
	}

	private void OnGUI()
	{
		SerializedObject obj = new SerializedObject(this);


		EditorGUILayout.PropertyField(obj.FindProperty("waypointRoot"));
		createWaypointOffset = EditorGUILayout.DelayedFloatField("createWaypointOffset", createWaypointOffset);
		if (GUILayout.Button("ResetToDefault"))
		{
			Default();
		}
		EditorGUILayout.Space();

		if (waypointRoot == null)
		{
			EditorGUILayout.HelpBox("Root transform must be selected. Please assign a root transform!", MessageType.Warning);
		}
		else
		{
			EditorGUILayout.BeginVertical("box");
			DrawButtons();
			EditorGUILayout.EndVertical();
		}

		obj.ApplyModifiedProperties();

		  
		void DrawButtons()
		{
			if (GUILayout.Button("Create Waypoint"))
			{
				CreateWaypoint();
			}
			if(Selection.activeGameObject)
			{
				if (Selection.activeGameObject.GetComponent<Waypoint>())
				{
					if (GUILayout.Button("Add Brach Waypoint"))
					{
						CreateBranch();
					}
					if (GUILayout.Button("Create Waypoint Before"))
					{
						CreateWaypointBefore();
					}
					if (GUILayout.Button("Create Waypoint After"))
					{
						CreateWaypointAfter();
					}
					if (GUILayout.Button("Remove Waypoint"))
					{
						RemoveWaypoint();
					}
				}
			}
		}
	}

	void CreateWaypoint()
	{
		GameObject _waypointObject;
		Waypoint _waypoint = null;

		if (Selection.activeGameObject)
		{
			Waypoint _selectedWaypoint = Selection.activeGameObject.GetComponent<Waypoint>();
			if (_selectedWaypoint)
			{
				if (_selectedWaypoint.nextWaypoint)
					return;

				_waypointObject = new GameObject("Waypoint " + waypointRoot.childCount, typeof(Waypoint));
				_waypointObject.transform.SetParent(waypointRoot, false);
				_waypoint = _waypointObject.GetComponent<Waypoint>();

				_waypoint.previousWaypoint = _selectedWaypoint;
				_selectedWaypoint.nextWaypoint = _waypoint;

				_waypoint.transform.forward = _selectedWaypoint.transform.forward;
				_waypoint.transform.position = _selectedWaypoint.transform.position + _waypoint.transform.forward * createWaypointOffset;
				_waypoint.width = _selectedWaypoint.width;
			}
		}
		else
		{
			_waypointObject = new GameObject("Waypoint " + waypointRoot.childCount, typeof(Waypoint));
			_waypointObject.transform.SetParent(waypointRoot, false);
			_waypoint = _waypointObject.GetComponent<Waypoint>();
		}

		if (_waypoint)
		{
			Selection.activeGameObject = _waypoint.gameObject;
		}

	}

	void CreateWaypointBefore()
	{
		GameObject _waypointObject = new GameObject("Waypoint " + waypointRoot.childCount, typeof(Waypoint));
		_waypointObject.transform.SetParent(waypointRoot, false);
		Waypoint _newWaypoint = _waypointObject.GetComponent<Waypoint>();

		Waypoint _selectedWaypoint = Selection.activeGameObject.GetComponent<Waypoint>();

		_waypointObject.transform.forward = _selectedWaypoint.transform.forward;
		_waypointObject.transform.position = _selectedWaypoint.transform.position - _waypointObject.transform.forward * createWaypointOffset;

		if (_selectedWaypoint.previousWaypoint != null)
		{
			_newWaypoint.previousWaypoint = _selectedWaypoint.previousWaypoint;
			_selectedWaypoint.previousWaypoint.nextWaypoint = _newWaypoint;
		}

		_newWaypoint.nextWaypoint = _selectedWaypoint;
		_selectedWaypoint.previousWaypoint = _newWaypoint;
		_newWaypoint.transform.SetSiblingIndex(_selectedWaypoint.transform.GetSiblingIndex());

		if (_newWaypoint.nextWaypoint != null)
		{
			_newWaypoint.width = _newWaypoint.nextWaypoint.width;
		}
		else
		{
			_newWaypoint.width = _newWaypoint.previousWaypoint.width;
		}

		Selection.activeGameObject = _newWaypoint.gameObject;
	}

	void CreateWaypointAfter()
	{
		GameObject _waypointObject = new GameObject("Waypoint " + waypointRoot.childCount, typeof(Waypoint));
		_waypointObject.transform.SetParent(waypointRoot, false);

		Waypoint _newWaypoint = _waypointObject.GetComponent<Waypoint>();

		Waypoint _selectedWaypoint = Selection.activeGameObject.GetComponent<Waypoint>();

		_waypointObject.transform.forward = _selectedWaypoint.transform.forward;
		_waypointObject.transform.position = _selectedWaypoint.transform.position + _waypointObject.transform.forward * createWaypointOffset;

		_newWaypoint.previousWaypoint = _selectedWaypoint;

		if (_selectedWaypoint.nextWaypoint != null)
		{
			_selectedWaypoint.nextWaypoint.previousWaypoint = _newWaypoint;
			_newWaypoint.nextWaypoint = _selectedWaypoint.nextWaypoint;
		}

		_selectedWaypoint.nextWaypoint = _newWaypoint;
		_newWaypoint.transform.SetSiblingIndex(_selectedWaypoint.transform.GetSiblingIndex());

		if (_newWaypoint.nextWaypoint != null)
		{
			_newWaypoint.width = _newWaypoint.nextWaypoint.width;
		}
		else
		{
			_newWaypoint.width = _newWaypoint.previousWaypoint.width;
		}

		Selection.activeGameObject = _newWaypoint.gameObject;
	}

	void CreateBranch()
	{
		GameObject _waypointObject = new GameObject("Waypoint " + waypointRoot.childCount, typeof(Waypoint));
		_waypointObject.transform.SetParent(waypointRoot, false);

		Waypoint _waypoint = _waypointObject.GetComponent<Waypoint>();

		Waypoint _branchesFrom = Selection.activeGameObject.GetComponent<Waypoint>();
		_branchesFrom.branches.Add(_waypoint);

		_waypoint.transform.position = _branchesFrom.transform.position + _branchesFrom.transform.forward * createWaypointOffset;
		_waypoint.transform.forward = _branchesFrom.transform.forward;
		_waypoint.width = _branchesFrom.width;

		Selection.activeGameObject = _waypoint.gameObject;
	}

	void RemoveWaypoint()
	{
		Waypoint _selectedWaypoint = Selection.activeGameObject.GetComponent<Waypoint>();

		if (_selectedWaypoint.nextWaypoint != null)
		{
			_selectedWaypoint.nextWaypoint.previousWaypoint = _selectedWaypoint.previousWaypoint;
		}
		if (_selectedWaypoint.previousWaypoint != null)
		{
			_selectedWaypoint.previousWaypoint.nextWaypoint = _selectedWaypoint.nextWaypoint;
			Selection.activeGameObject = _selectedWaypoint.previousWaypoint.gameObject;
		}

		//Transform _root = waypointRoot;
		DestroyImmediate(_selectedWaypoint.gameObject);
		//waypointRoot = _root;
	}

	void Default()
	{
		createWaypointOffset = 3f;
	}
}
