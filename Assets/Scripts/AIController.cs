using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIController : MonoBehaviour
{
    private CharacterController controller;
    private State currentState;
    private new Animation animation;

    private bool isControllable = true;
    private bool isDead = false;

    //private PlayerStatus playerStatus;

    private Vector3 moveDirection = new Vector3(0, 0, 0);
    private float movementSpeed = 2.5f;
    private float gravity = -9.81f;
    private float yVelocity = 0;

    private Transform target;

    private GameObject gun;

    public float fieldOfView = 130.0f;
    public float sightDistance = 20.0f;
    public float attackDistance = 5.0f;
    public float attackCooldown = 1.0f;

    // Checking if character is dead
    public bool IsDead
    {
        get { return isDead; }
        set { isDead = value; }
    }

    // Checking if enemy is within range of attack
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
    public bool EnemySeen()
    {
        GameObject player = GameObject.Find("Player");

        float angleToTurn = 0;
        Vector3 playerPos = new Vector3(0, 0, 0);

        if (player)
        {
            PlayerStatus playerStatus = player.GetComponent(typeof(PlayerStatus)) as PlayerStatus;
            playerPos = player.transform.position;

            // A1 Goalie Logic to find angle between NPC facing direction and player
            Vector3 relativePos = transform.InverseTransformPoint(playerPos);
            angleToTurn = Mathf.Atan2(relativePos.x, relativePos.z) * Mathf.Rad2Deg;

            // Finding distance between NPC and player
            float diff = diffInPosition();

            // Checking angle in both directions, distance, and if player is alive
            if (angleToTurn <= (fieldOfView / 2) && angleToTurn >= (fieldOfView / -2) && diff <= sightDistance && (playerStatus.isAlive()))
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
            Vector3 AIPos = transform.position;

            // Find the difference between positions
            Vector3 u = AIPos - playerPos;
            diff = u.magnitude;
        }

        return diff;
    }

    public void BeIdle()
    {
        //animation.CrossFade("idle", 0.2f);
        // Debug.Log("Homie in idle");
        moveDirection = new Vector3(0, 0, 0);

    }

    public void BeApproaching()
    {
        GameObject player = GameObject.Find("Player");
        Vector3 playerPos = player.transform.position;
        Vector3 u = (playerPos - transform.position).normalized;

        movementSpeed = 5;
        moveDirection = u;

        transform.LookAt(playerPos);
    }

    public void BeShooting()
    {
        //animation.CrossFade("shoot", 0.2f);
        Debug.Log("Homie is shooting");
        movementSpeed = 0;
        gun = transform.GetChild(0).gameObject;
        

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

    public void goToSpawn()
    {
        GameObject spawn = GameObject.Find("Spawn Point");
        Vector3 spawnLocation = spawn.transform.position;
        Vector3 u = (spawnLocation - transform.position).normalized;
        if(!((transform.position - spawnLocation).magnitude <= 5)) // character is not at spawn
        {
            // TODO: add better pathfinding/collision avoidance with rayasting
            movementSpeed = 5;
            moveDirection = u;
        } else
        {
            isDead = false;
        }
    }

    public void ChangeState(State newState)
    {
        currentState = newState;
    }

    private void OnCollisionEnter(Collision collision)
    {
        Debug.Log("Hit!");
        if(collision.gameObject.tag == "paintball") //collision is paintball
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

        currentState.Execute(this);
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
