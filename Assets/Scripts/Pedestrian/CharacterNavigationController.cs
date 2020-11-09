using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterNavigationController : MonoBehaviour
{
	public float movementSpeed = 4;
	public float rotationSpeed = 120;
	public float stopDistance = 2.5f;
	public Vector3 destination = new Vector3(544, 0f, 351);
	public bool reachedDestination = false;

	private void Start()
	{
		movementSpeed = Random.Range(1, 5);
	}

	void Update()
	{
		if (transform.position != destination)
		{
			Vector3 _destinationDirection = destination - transform.position;
			_destinationDirection.y = 0;

			float _destinationDistance = _destinationDirection.magnitude;

			if (_destinationDistance >= stopDistance)
			{
				reachedDestination = false;
				Quaternion _targetRotation = Quaternion.LookRotation(_destinationDirection);
				transform.rotation = Quaternion.RotateTowards(transform.rotation, _targetRotation, rotationSpeed * Time.deltaTime);
				transform.Translate(Vector3.forward * movementSpeed * Time.deltaTime);
			}
			else
			{
				reachedDestination = true;
			}
		}
	}

	public void SetDestination(Vector3 destination)
	{
		this.destination = destination;
		reachedDestination = false;
	}
}
