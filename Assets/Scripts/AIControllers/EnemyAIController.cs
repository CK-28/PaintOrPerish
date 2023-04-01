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
        GameObject player = GameObject.Find("Player");

        if (player)
        {
            PlayerStatus playerStatus = player.GetComponent(typeof(PlayerStatus)) as PlayerStatus;

            float diff = diffInPosition();

            // Checking distance
            if (diff <= attackDistance && (playerStatus.isAlive()))
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        return false;
    }

    // Checking if enemy is within FOV (180 degrees)
    override
    public bool EnemySeen()
    {
        /*GameObject player = GameObject.FindGameObjectsWithTag("Player")[0];

        float angleToTurn = 0;
        Vector3 playerPos = new Vector3(0, 0, 0);

        if (player)
        {
            PlayerStatus playerStatus = player.GetComponent<PlayerStatus>();//(typeof(PlayerStatus)) as PlayerStatus;
            playerPos = player.transform.position;

            // A1 Goalie Logic to find angle between NPC facing direction and player
            Vector3 relativePos = controller.transform.InverseTransformPoint(playerPos);
            angleToTurn = Mathf.Atan2(relativePos.x, relativePos.z) * Mathf.Rad2Deg;

            // Finding distance between NPC and player
            float diff = diffInPosition();

            // Checking angle in both directions, distance, and if player is alive
            if (angleToTurn <= (fieldOfView / 2) && angleToTurn >= (fieldOfView / -2) && diff <= sightDistance *//*&& (playerStatus.isAlive())*//*)
            {
                return true;
            }
            else
            {
                return false;
            }
        }*/

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

    // Function to get vector magnitude between Enemy and AI
    public float diffInPosition()
    {
        float diff = 0.0f;

        // Grab enemy position
        GameObject player = GameObject.Find("Player");
        Vector3 playerPos = new Vector3(0, 0, 0);

        if (player)
        {
            // Grab Player position
            playerPos = player.transform.position;

            // Grab NPC position
            Vector3 AIPos = controller.transform.position;

            // Find the difference between positions
            Vector3 u = AIPos - playerPos;
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
        GameObject player = GameObject.Find("Player");
        Vector3 playerPos = player.transform.position;
        Vector3 u = (playerPos - controller.transform.position).normalized;

        movementSpeed = 5;
        moveDirection = u;

        controller.transform.LookAt(playerPos);
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
