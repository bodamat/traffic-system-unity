using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PedestrianSpawner : MonoBehaviour
{
    public GameObject pedestrianPrefab;
    public int pedestriansToSpawn;

    void Start()
    {
        StartCoroutine(Spawn());
    }

    IEnumerator Spawn()
	{
        int count = 0;
        while (count < pedestriansToSpawn)
		{
            GameObject _obj = Instantiate(pedestrianPrefab);
            Transform _child = transform.GetChild(Random.Range(0, transform.childCount - 1));
            Waypoint _waypoint = _child.GetComponent<Waypoint>();
            _obj.GetComponent<WaypointNavigator>().currentWaypoint = _waypoint;
            _obj.transform.position = _child.transform.position;
			_obj.transform.position = new Vector3(_obj.transform.position.x, _obj.GetComponent<CapsuleCollider>().height / 2, _obj.transform.position.z);

			yield return new WaitForEndOfFrame();

            count++;
		}
	}

    private Vector3 RandomVector3(Vector3 minVector, Vector3 maxVector)
	{
        return new Vector3(Random.Range(minVector.x, maxVector.x), Random.Range(minVector.y, maxVector.y), Random.Range(minVector.z, maxVector.z));
    }
}
