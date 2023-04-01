using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;


public class EnemyAIController : AIController
{
    private CharacterController controller;
    //private State currentState;
    private new Animation animation;

    private bool isControllable = true;

    //private PlayerStatus playerStatus;

    private Vector3 moveDirection = new Vector3(0, 0, 0);
    private float movementSpeed = 2.5f;
    private float gravity = -9.81f;
    private float yVelocity = 0;

    private Transform target;

    private GameObject gun;

    private TDMDefend defendObjective;
    private TDMRoam roamObjective;
    private float roamCooldown = 5.0f;

    // Checking if enemy is within range of attack
    override
    public bool EnemyInRange()
    {
        int targetIndex = FindNearestEnemy();
        Transform target;
        float diff;

        if(targetIndex >= 0)
        {
            target = enemies[targetIndex].transform;
            diff = diffInPosition(target);

            if(diff <= attackDistance)
            {
                return true;
            }
        }

        return false;
    }

    // Checking if enemy is seen
    override
    public bool EnemySeen()
    {
        Vision vis = GetComponent<Vision>();
        Transform target;
        int targetIndex = FindNearestEnemy();

        if(targetIndex >= 0)
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
            Vector3 AIPos = controller.transform.position;

            // Find the difference between positions
            Vector3 u = AIPos - pos;
            diff = u.magnitude;
        }

        return diff;
    }

    override
    public void BeIdle()
    {
        //animation.CrossFade("idle", 0.2f);
        // Debug.Log("Homie in idle");
        moveDirection = new Vector3(0, 0, 0);

    }

    override
    public void BeApproaching()
    {
        /*GameObject player = GameObject.Find("Player");
        Vector3 playerPos = player.transform.position;
        Vector3 u = (playerPos - controller.transform.position).normalized;

        movementSpeed = 5;
        moveDirection = u;

        controller.transform.LookAt(playerPos);*/
        int targetIndex = FindNearestEnemy();
        GameObject target;
        if (targetIndex >= 0)
        {
            target = enemies[targetIndex];
            navMeshAgent.destination = target.transform.position;
        }
    }

    override
    public void BeShooting()
    {
        //animation.CrossFade("shoot", 0.2f);
        Debug.Log("Homie is shooting");
        movementSpeed = 0;
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

    override
    public void BeObjective()
    {
        if (myRole == Role.Defend)
        {
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
            }
            // move
        }
        else if(myRole == Role.Roam)
        {
            Vector3 position = roamObjective.getPosition();
            navMeshAgent.destination = position;
            if (Vector3.Distance(position, controller.transform.position) < 1)
            {
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

    override
    public void goToSpawn()
    {
        GameObject spawn = GameObject.Find("Spawn Point");
        Vector3 spawnLocation = spawn.transform.position;
        if (!((controller.transform.position - spawnLocation).magnitude <= 5)) // character is not at spawn
        {
            // TODO: add better pathfinding/collision avoidance with rayasting
            navMeshAgent.destination = spawnLocation;
        }
        else
        {
            isDead = false;
            defendObjective.generateLocation();
            roamObjective.chooseRoute();
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        Debug.Log("Hit!");
        if(collision.gameObject.tag == "paintballRed") //collision is enemy paintball
        {
            Debug.Log("Dead!");
            // set dead
            isDead = true;
        }
    }

    // Use this for initialization
    void Start()
    {

        controller = GetComponent<CharacterController>();
        navMeshAgent = GetComponent<NavMeshAgent>();
        defendObjective = GetComponent<TDMDefend>();
        roamObjective = GetComponent<TDMRoam>();

        enemies = GameObject.FindGameObjectsWithTag("RedTeam");
        Debug.Log("found " + enemies.Length + " enemies");
        //animation = GetComponent<Animation>();
        //GameObject tmp = GameObject.FindWithTag("Player");
        //if (tmp != null)
        //{
        //target = tmp.transform;
        //playerStatus = tmp.GetComponent<PlayerStatus>();
        //}

        ChangeState(new StateIdle());
    }

    // Update is called once per frame
    void Update()
    {
        if (!isControllable)
        {
            return;
        }

        if(currentState != null)
        {
            currentState.Execute(this);
        }
        //transform.LookAt(new Vector3(moveDirection.x, 0, moveDirection.z));

        /* yVelocity += gravity * Time.deltaTime;

         moveDirection.y = yVelocity;

         *//*if (!isDead)
         {
             controller.Move(moveDirection * Time.deltaTime * movementSpeed);
         }*//*
         controller.Move(moveDirection * Time.deltaTime * movementSpeed);*/
    }
}
