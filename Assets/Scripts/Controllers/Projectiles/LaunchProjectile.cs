using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LaunchProjectile : MonoBehaviour
{
    public GameObject projectile;
    public float launchVelocity = 5000f;

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
