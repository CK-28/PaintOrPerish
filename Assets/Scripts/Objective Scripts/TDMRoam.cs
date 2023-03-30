using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TDMRoam : MonoBehaviour
{
    GameObject[] route1;
    GameObject[] route2;
    GameObject[] route3;

    // Holds all possible routes
    GameObject[][] routes;
    GameObject[] currentRoute;
    int currentTarget;

    // Start is called before the first frame update
    void Start()
    {
        route1 = GameObject.FindGameObjectsWithTag("TDMRoute1");
        route2 = GameObject.FindGameObjectsWithTag("TDMRoute2");
        route3 = GameObject.FindGameObjectsWithTag("TDMRoute3");

        routes = new GameObject[][] { route1, route2, route3};

        chooseRoute();
    }

    // Chooses a random route to follow
    public void chooseRoute()
    {
        int index = Random.Range(0, routes.Length);
        currentRoute = routes[index];
        currentTarget = 0;
    }

    // Return position of current target
    public Vector3 getPosition()
    {
        return currentRoute[currentTarget].transform.position;
    }

    // Get next target
    public void getNextLocation()
    {
        currentTarget++;
    }
}
