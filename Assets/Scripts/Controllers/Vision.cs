using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Handles AI raycasting vision
public class Vision : MonoBehaviour
{
    public float sightDistance = 20.0f;
    public float visionAngle = 160.0f;
    public int rayArrHor = 5;
    public int rayArrVert = 5;
    public float rayCastAngle = 60.0f;
    public string enemyTag;

    // Get angle to target
    float getAngle(Vector3 targetPos)
    {
        Vector3 inLocal = transform.InverseTransformPoint(targetPos);
        float angle = Mathf.Atan2(inLocal.x, inLocal.z) * Mathf.Rad2Deg;

        return angle;
    }

    // Detect if enemy seen; Returns vector to target
    public Vector3 EnemySeen(Transform target)
    {
        Vector3 toReturn = new Vector3(0.0f, 0.0f, 0.0f);
        if (getAngle(target.position) < visionAngle/2.0f && getAngle(target.position) > visionAngle/-2.0f)
        {
            Vector3 temp = RayArrayHit(rayArrHor, rayArrVert, rayCastAngle, target);
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

    // Casts array of rays based on given arguements; returns vector of ray that hits
    public Vector3 RayArrayHit(int arrHorCount, int arrVertCount, float angle, Transform target)
    {
        Vector3 toReturn = new Vector3(0.0f,0.0f,0.0f);
        Vector3 start = target.position - (gameObject.transform.position + new Vector3(0, 2.2f, 0));

        for(int i = arrHorCount / -2; i < (arrHorCount / 2)+1; i++)
        {
            
            float horAnglePart = angle / arrHorCount;
            for (int j = arrVertCount / -2; j < (arrVertCount / 2)+1; j++)
            {
                float verAnglePart = angle / arrVertCount;
                RaycastHit hit;
                Vector3 castAttempt = Quaternion.AngleAxis(horAnglePart*i,Vector3.up)*start;
                castAttempt = Quaternion.AngleAxis(horAnglePart*j, Vector3.right) * castAttempt;
                
                bool hitsTarget = Physics.Raycast(transform.position + new Vector3(0, 2.2f, 0), castAttempt, out hit, sightDistance);
                if (hitsTarget)
                {
                    if (hit.transform.gameObject.tag == enemyTag) // enemyTag is public. check the editor
                    {
                        return castAttempt;
                    }
                }
            }
        }
        return toReturn;
    }

}
