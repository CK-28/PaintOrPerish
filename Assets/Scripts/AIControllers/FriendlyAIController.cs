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

    override
    public void goToSpawn()
    {
        // CharacterController and CapsuleCollider change size when crouching to account for smaller hitbox
        controller.height = 2.2f;
        collider.height = 2.2f;

        // CharacterController and CapsuleCollider move when crouching to account for shorter character height
        controller.center = new Vector3(0, 1.15f, 0);
        collider.center = new Vector3(0, 1.15f, 0);

        GameObject spawn = GameObject.Find("SpawnANav");
        Vector3 spawnLocation = spawn.transform.position;
        if (!((controller.transform.position - spawnLocation).magnitude <= 1)) // character is not at spawn
        {
            navMeshAgent.speed = 5;
            navMeshAgent.destination = spawnLocation;

            mAnimator.SetTrigger("TriDead");
            mAnimator.SetTrigger("TriArmRaise");
            mAnimator.SetTrigger("TriWalkArmRaise");
            //mAnimator.ResetTrigger("TriDead");
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

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "paintball" && !isDead) //collision is enemy paintball
        {
            Debug.Log("Dead!");
            isDead = true;

            gameManager.updateBlueScore(1);

            // mAnimator.SetTrigger("TriDead");
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

        enemies = GameObject.FindGameObjectsWithTag("BlueTeam");
        TheGame = GameObject.Find("TheGame");
        gameManager = TheGame.GetComponent<TheGame>();

        mAnimator = GetComponent<Animator>();

        ChangeState(new StateIdle());
    }

    // Update is called once per frame
    void Update()
    {
        /*mAnimator.ResetTrigger("TriWalk");*/
        mAnimator.ResetTrigger("TriCrouch");
        /*mAnimator.ResetTrigger("TriRun");
        mAnimator.ResetTrigger("TriIdle");
        mAnimator.ResetTrigger("TriArmRaise");*/

        /*if (isDead)
        {
            mAnimator.SetTrigger("TriDead");
        }*/

        if (!isControllable)
        {
            return;
        }

        currentState.Execute(this);
    }
}
