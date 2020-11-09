using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarSpawner : MonoBehaviour
{
    public GameObject objectPrefab;
    public int objectsToSpawn;

    List<int> _randomList = new List<int>();

    void Start()
    {
        StartCoroutine(Spawn());
    }

    IEnumerator Spawn()
    {
        objectsToSpawn = objectsToSpawn > transform.childCount ? transform.childCount : objectsToSpawn;

        _randomList.Clear();
        int count = 0;
        while (count < objectsToSpawn)
        {
            int rndInt = Random.Range(0, transform.childCount - 1);
            if (_randomList.Contains(rndInt))
                continue;
            _randomList.Add(rndInt);

            GameObject _obj = Instantiate(objectPrefab);
            Transform _child = transform.GetChild(rndInt);
            Waypoint _waypoint = _child.GetComponent<Waypoint>();
            _obj.GetComponent<CarWaypointNavigator>().currentWaypoint = _waypoint;
            _obj.transform.position = _child.transform.position;
            _obj.transform.forward = _child.transform.forward;

            yield return new WaitForEndOfFrame();

            count++;
        }
    }
}
