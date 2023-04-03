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
    public TDMDefend defendObjective;
    public TDMRoam roamObjective;
    public float roamCooldown = 5.0f;

    public CharacterController controller;
    public CapsuleCollider collider;
    public Animator mAnimator;

    public GameObject gun;
    public Transform shootAt;

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

            if (dist <= sightDistance)
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

    // Function to get vector magnitude to target
    public float diffInPosition(Transform target)
    {
        float diff = 0.0f;

        // Grab enemy position
        Vector3 pos = new Vector3(0, 0, 0);

        if (target)
        {
            // Grab Player position
            pos = target.transform.position;

            // Grab NPC position
            Vector3 AIPos = transform.position;

            // Find the difference between positions
            Vector3 u = AIPos - pos;
            diff = u.magnitude;
        }

        return diff;
    }

    // Checking if enemy is within range of attack
    public bool EnemyInRange()
    {
        int targetIndex = FindNearestEnemy();
        Transform target;
        float diff;

        if (targetIndex >= 0)
        {
            target = enemies[targetIndex].transform;
            diff = diffInPosition(target);
            Debug.Log("Enemy found");

            if (diff <= attackDistance)
            {
                Debug.Log("Enemy in range!");
                shootAt = target;
                return true;
            }
        }

        return false;
    }

    // Checking if enemy is seen
    public bool EnemySeen()
    {
        Vision vis = GetComponent<Vision>();
        Transform target;
        int targetIndex = FindNearestEnemy();

        if (targetIndex >= 0)
        {
            target = enemies[targetIndex].transform;
            if (vis.EnemySeen(target) != new Vector3(0, 0, 0))
            {
                Debug.Log("Enemy Seen!");
                return true;
            }
        }

        return false;
    }

    public void BeIdle()
    {
        mAnimator.SetTrigger("TriIdle");

        navMeshAgent.speed = 0;
    }

    public void BeObjective()
    {
        /*mAnimator.ResetTrigger("TriIdle");

        mAnimator.ResetTrigger("TriDead");
        mAnimator.ResetTrigger("TriArmRaise");
        mAnimator.ResetTrigger("TriWalkArmRaise");*/

        if (myRole == Role.Defend)
        {
            navMeshAgent.speed = 8;
            mAnimator.SetTrigger("TriWalk");

            // get point
            Transform location = defendObjective.getLocation();
            Vector3 position = defendObjective.getPosition();
            // go there
            navMeshAgent.destination = position;
            // hang out
            if (Vector3.Distance(position, controller.transform.position) < 1)
            {
                Quaternion rotation = Quaternion.LookRotation(location.forward, Vector3.up);
                controller.transform.rotation = rotation;
                mAnimator.SetTrigger("TriCrouch");

                // CharacterController and CapsuleCollider change size when crouching to account for smaller hitbox
                controller.height = 1.6f;
                collider.height = 1.6f;

                // CharacterController and CapsuleCollider move when crouching to account for shorter character height
                controller.center = new Vector3(0, 1, 0);
                collider.center = new Vector3(0, 1, 0);
            }
        }
        else if (myRole == Role.Roam)
        {
            navMeshAgent.speed = 12;

            Vector3 position = roamObjective.getPosition();
            navMeshAgent.destination = position;
            mAnimator.SetTrigger("TriRun");

            if (Vector3.Distance(position, controller.transform.position) < 1)
            {
                mAnimator.ResetTrigger("TriRun");
                mAnimator.SetTrigger("TriIdle");
                if (roamCooldown <= 0)
                {
                    roamObjective.getNextLocation();
                    roamCooldown = 1.0f;
                }
                else
                {
                    roamCooldown = roamCooldown - Time.deltaTime;
                }
            }
        }
    }

    public void BeApproaching()
    {
        mAnimator = GetComponent<Animator>();

        int targetIndex = FindNearestEnemy();
        GameObject target;
        if (targetIndex >= 0)
        {
            navMeshAgent.speed = 5;
            mAnimator.SetTrigger("TriWalk");

            target = enemies[targetIndex];
            navMeshAgent.destination = target.transform.position;
        }
    }

    public void BeShooting()
    {
        // stop moving
        navMeshAgent.speed = 0;

        // look at target
        transform.LookAt(shootAt);

        gun = controller.transform.GetChild(2).gameObject;

        if (attackCooldown <= 0)
        {
            gun.GetComponent<AILaunchProjectile>().LaunchProjectile();
            attackCooldown = 1.0f;
        }
        else
        {
            attackCooldown = attackCooldown - Time.deltaTime;
        }

        return;
    }
}
