using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Vision : MonoBehaviour
{
    // Start is called before the first frame update
    public Transform target;
    public float sightDistance = 20.0f;
    public float visionAngle = 180.0f;
    public int rayArrHor = 5;
    public int rayArrVert = 5;
    public float rayCastAngle = 60.0f;
    public string enemyTag = "Red";

    void Start()
    {
        //Debug.DrawRay(gameObject.transform.position, EnemySeen(), Color.red);
        //print(EnemySeen());
    }

    // Update is called once per frame
    void Update()
    {
        //Debug.DrawRay(gameObject.transform.position, EnemySeen(), Color.red);
        //RayArrayHit(5, 5, 60.0f);
    }

    float getAngle(Vector3 targetPos)
    {
        Vector3 inLocal = transform.InverseTransformPoint(targetPos);
        float angle = Mathf.Atan2(inLocal.x, inLocal.z) * Mathf.Rad2Deg;

        return angle;
    }

    public Vector3 EnemySeen()
    {
        Vector3 toReturn = new Vector3(0.0f, 0.0f, 0.0f);
        if (getAngle(target.position) < visionAngle/2.0f && getAngle(target.position) > visionAngle/-2.0f)
        {
            Vector3 temp = RayArrayHit(rayArrHor, rayArrVert, rayCastAngle);
            if(toReturn == temp)
            {
                return toReturn;
            }
            else
            {
                return temp.normalized;
            }
        }
        return toReturn;
    }

    public Vector3 RayArrayHit(int arrHorCount, int arrVertCount,float angle)
    {
        Vector3 toReturn = new Vector3(0.0f,0.0f,0.0f);
        Vector3 start = target.position - gameObject.transform.position;

        for(int i = arrHorCount / -2; i < (arrHorCount / 2)+1; i++)
        {
            float horAnglePart = angle / arrHorCount;
            for (int j = arrVertCount / -2; j < (arrVertCount / 2)+1; j++)
            {
                float verAnglePart = angle / arrVertCount;
                RaycastHit hit;
                Vector3 castAttempt = Quaternion.AngleAxis(horAnglePart*i,Vector3.up)*start;
                castAttempt = Quaternion.AngleAxis(horAnglePart*j, Vector3.right) * castAttempt;

                //Debug.DrawRay(transform.position, castAttempt, Color.green, 15, false);
                
                bool hitsTarget = Physics.Raycast(gameObject.transform.position, castAttempt, out hit, sightDistance);
                if (hitsTarget)
                {
                    if (hit.transform.gameObject.tag == enemyTag)
                    {
                        //Debug.DrawRay(transform.position, castAttempt, Color.red, 15, false);
                        return castAttempt;
                    }
                    //else{
                        //Debug.DrawRay(transform.position, castAttempt, Color.green, 15, false);
                    //}
                }
            }
        }
        return toReturn;
    }

}
