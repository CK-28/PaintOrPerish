using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LaunchProjectile : MonoBehaviour
{
    public GameObject projectile;
    public float launchVelocity = 10000f;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        // fire projectile
        if (Input.GetButtonDown("Fire1"))
        {
            GameObject ball = Instantiate(projectile, transform.position, transform.rotation); // create projectile
            ball.GetComponent<Rigidbody>().AddRelativeForce(new Vector3(launchVelocity, 0, 0)); // apply force
        }

    }
}
