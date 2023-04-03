using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SelfErase : MonoBehaviour
{
    public float timeToErase;
    private float timeAlive = 0.0f;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        timeAlive += Time.deltaTime;
        if (timeAlive >= timeToErase)
        {
            Destroy(this.gameObject);
        }
    }
}
