using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Splat : MonoBehaviour
{
    public GameObject splatterMark;
    private bool painted = false;
    public void Update()
    {
        RaycastHit smt;
        Vector3 forward = gameObject.transform.TransformDirection(Vector3.forward).normalized;
        Debug.DrawRay(gameObject.transform.position, forward, Color.green);
        if (Physics.Raycast(gameObject.transform.position, forward, out smt, 2f))
        {
            if (painted != true)
            {
                painted = true;
                Instantiate(splatterMark, smt.point + (smt.normal * 0.1f), Quaternion.FromToRotation(Vector3.forward, smt.normal));
            }
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (!painted)
        {
            Instantiate(splatterMark, gameObject.transform.position, gameObject.transform.rotation);
        }
        //print("Splash!");

        Destroy(this.gameObject);
    }
}
