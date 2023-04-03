using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

// Handles retreating, collisions, start, and update for Enemy AIs (blue team)
public class EnemyAIController : AIController
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

    // Called when dead, sends character back to spawn
    override
    public void goToSpawn()
    {
        // CharacterController and CapsuleCollider change size when crouching to account for smaller hitbox
        controller.height = 2.2f;
        collider.height = 2.2f;

        // CharacterController and CapsuleCollider move when crouching to account for shorter character height
        controller.center = new Vector3(0, 1.15f, 0);
        collider.center = new Vector3(0, 1.15f, 0);

        GameObject spawn = GameObject.Find("SpawnBNav");
        Vector3 spawnLocation = spawn.transform.position;
        mAnimator.SetTrigger("TriDead");
        if (!((controller.transform.position - spawnLocation).magnitude <= 1)) // character is not at spawn
        {
            navMeshAgent.speed = 5;
            navMeshAgent.destination = spawnLocation;

            mAnimator.SetTrigger("TriDead");
            mAnimator.SetTrigger("TriArmRaise");
            mAnimator.SetTrigger("TriWalkArmRaise");
        }
        else
        {
            mAnimator.SetTrigger("TriIdle");
            mAnimator.ResetTrigger("TriDead");

            isDead = false;
            defendObjective.generateLocation();
            roamObjective.chooseRoute();
        }
    }

    // Called when collision detected
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "paintball" && !isDead) //collision is enemy paintball
        {
            Debug.Log("Dead!");
            isDead = true;

            gameManager.updateRedScore(1);
        }
    }

    // Use this for initialization
    void Start()
    {
        collider = GetComponent<CapsuleCollider>();
        controller = GetComponent<CharacterController>();
        navMeshAgent = GetComponent<NavMeshAgent>();
        defendObjective = GetComponent<TDMDefend>();
        roamObjective = GetComponent<TDMRoam>();

        enemies = GameObject.FindGameObjectsWithTag("RedTeam");
        Debug.Log("found " + enemies.Length + " enemies");

        TheGame = GameObject.Find("TheGame");
        gameManager = TheGame.GetComponent<TheGame>();

        mAnimator = GetComponent<Animator>();

        ChangeState(new StateIdle());
    }

    // Update is called once per frame
    void Update()
    {
        mAnimator.ResetTrigger("TriCrouch");

        if (!isControllable)
        {
            return;
        }

        if(currentState != null)
        {
            currentState.Execute(this);
        }
    }
}
