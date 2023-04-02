using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class FriendlyAIController : AIController
{
    public GameObject TheGame;
    TheGame gameManager;

    private new Animation animation;

    private bool isControllable = true;

    private Vector3 moveDirection = new Vector3(0, 0, 0);
    private float movementSpeed = 2.5f;
    private float gravity = -9.81f;
    private float yVelocity = 0;

    private Transform target;

    private GameObject gun;

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
    public void goToSpawn()
    {
        GameObject spawn = GameObject.Find("SpawnANav");
        Vector3 spawnLocation = spawn.transform.position;
        if (!((controller.transform.position - spawnLocation).magnitude <= 5)) // character is not at spawn
        {
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
        if (collision.gameObject.tag == "paintball" && !isDead) //collision is enemy paintball
        {
            Debug.Log("Dead!");
            isDead = true;

            gameManager.updateBlueScore(1);
        }
    }

    // Use this for initialization
    void Start()
    {

        controller = GetComponent<CharacterController>();
        navMeshAgent = GetComponent<NavMeshAgent>();
        defendObjective = GetComponent<TDMDefend>();
        roamObjective = GetComponent<TDMRoam>();

        enemies = GameObject.FindGameObjectsWithTag("BlueTeam");
        TheGame = GameObject.Find("TheGame");
        gameManager = TheGame.GetComponent<TheGame>();

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
    }
}
