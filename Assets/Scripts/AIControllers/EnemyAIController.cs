using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;


public class EnemyAIController : AIController
{
    public GameObject TheGame;
    TheGame gameManager;

    //private State currentState;
    private new Animation animation;

    private bool isControllable = true;

    //private PlayerStatus playerStatus;

    private Vector3 moveDirection = new Vector3(0, 0, 0);
    private float movementSpeed = 2.5f;
    private float gravity = -9.81f;
    private float yVelocity = 0;

    private Transform target;

    override
    public void goToSpawn()
    {
        GameObject spawn = GameObject.Find("Spawn Point");
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

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "paintball" && !isDead) //collision is enemy paintball
        {
            Debug.Log("Dead!");
            isDead = true;

            gameManager.updateRedScore(1);
            // mAnimator.SetTrigger("TriDead");
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
        mAnimator.ResetTrigger("TriArmRaise");
        mAnimator.ResetTrigger("TriWalkArmRaise");*/

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
