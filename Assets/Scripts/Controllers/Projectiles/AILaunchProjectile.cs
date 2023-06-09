using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AILaunchProjectile : MonoBehaviour
{
    public GameObject projectile;
    public float launchVelocity = 5000f;

    public void LaunchProjectile()
    {
        GameObject ball = Instantiate(projectile, transform.position, transform.rotation); // create projectile
        ball.GetComponent<Rigidbody>().AddRelativeForce(new Vector3(launchVelocity, 0, 0)); // apply force
    }
}
