using UnityEngine;
using System.Collections;
using UnityEngine.AI;

public abstract class AIController : MonoBehaviour
{
    public State currentState;
    public bool isDead = false;
    public GameObject[] enemies;

    // Determines objective
    public enum Role { Defend, Roam };
    public Role myRole;

    public float fieldOfView = 130.0f;
    public float sightDistance = 20.0f;
    public float attackDistance = 5.0f;
    public float attackCooldown = 1.0f;

    public NavMeshAgent navMeshAgent;


    public abstract bool EnemyInRange();
    public abstract bool EnemySeen();
    public abstract void BeIdle();
    public abstract void BeApproaching();
    public abstract void BeShooting();
    public abstract void BeObjective();
    public abstract void goToSpawn();

    // Checking if character is dead
    public bool IsDead
    {
        get { return isDead; }
        set { isDead = value; }
    }

    public void ChangeState(State newState)
    {
        currentState = newState;
    }

    public int FindNearestEnemy()
    {
        AIController enemyController;
        PlayerController playerController;

        Transform target;
        float minDistance = 20000;
        int closest = -1;

        for (int i = 0; i < enemies.Length; i++)
        {
            if(enemyController = enemies[i].GetComponent<AIController>())
            {
                if (enemyController.IsDead)
                    continue;
            }
            if (playerController = enemies[i].GetComponent<PlayerController>())
            {
                if (playerController.IsDead)
                    continue;
            }

            target = enemies[i].transform;
            Vector3 toPlayer = target.position - transform.position;

            float dist = toPlayer.magnitude;

            toPlayer.y = 0;
            toPlayer = Vector3.Normalize(toPlayer);

            //Forward in world space
            Vector3 forward = transform.TransformDirection(new Vector3(0, 0, 1));
            forward.y = 0;
            forward = Vector3.Normalize(forward);

            if (dist <= attackDistance)
            {
                if (dist < minDistance)
                {
                    minDistance = dist;
                    closest = i;
                }
            }

        }

        return closest;
    }
}
