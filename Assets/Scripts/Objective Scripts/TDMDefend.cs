using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TDMDefend : MonoBehaviour
{
    GameObject[] defensePoints;
    GameObject location;
    Vector3 position;
    // Start is called before he first frame update
    void Start()
    {
        defensePoints = GameObject.FindGameObjectsWithTag("TDMDefense");
        Debug.Log("There are " + defensePoints.Length + " defense points in the map");        

        generateLocation();
    }

    // Get location to go to
    public void generateLocation()
    {
        // generate random point
        int index = Random.Range(0, defensePoints.Length);
        location = defensePoints[index];
        position = location.transform.position;
    }

    // Return transform of defense point
    public Transform getLocation()
    {
        return location.transform;
    }

    // Return vector position of defense point
    public Vector3 getPosition()
    {
        return location.transform.position;
    }
}
