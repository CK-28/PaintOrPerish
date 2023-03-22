using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TheGame : MonoBehaviour
{
    //bool gameStarted = true;
    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public bool isGameStarted ()
    {
        var gameObject = GameObject.Find("Timer");
        Timer timeScript = gameObject.GetComponent<Timer>();

        if (timeScript.returnTimeLeft() < 80)
        {
            return true;
        }
        else
        {
            return false;
        }

        //Debug.Log("Grabbing time in TheGame has failed");
        //return false;
    }
}
