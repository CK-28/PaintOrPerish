using UnityEngine;
using System.Collections;

public class AIController : MonoBehaviour {

	public float			attackDistance = 2.0f;

	private static float	attackSpeed = 1.5f;
	private float			attackWait = 0.0f;
	private float 			gravity = 50.0f;
	private float			attackValue = 5.0f;
	
	private CharacterController controller;
	private PlayerStatus	playerStatus;
	private Transform		target;
	private Vector3			moveDirection = new Vector3(0,0,0);
	private float			movementSpeed = 2.5f;
	private State			currentState;
	private new Animation	animation;

	private bool			isControllable = true;
	private bool			isDead = false;
	private bool			isCritical = false;

	//This is a hack for legacy animation - we will do this properly later
	private bool			deathStarted = false;

	public float			fieldOfView = 180.0f;
	public float			sightDistance = 20.0f;


	public bool 	IsControllable {
		get {return isControllable;}
		set {isControllable = value;}
	}

	public bool IsDead
	{  
		get { return isDead; }
		set { isDead = value; }
	}

	public bool IsCritical
	{
		get { return isCritical; }
		set { isCritical = value; }
	}

	// Use this for initialization
	void Start ()
	{
		controller = GetComponent< CharacterController>();
		animation = GetComponent<Animation>();
		GameObject tmp = GameObject.FindWithTag("Player");
		if (tmp != null){
			target=tmp.transform;
			playerStatus = tmp.GetComponent< PlayerStatus>();
		}

		ChangeState(new StateIdle());
	}
	
	public void ChangeState(State newState)
	{
		currentState = newState;
	}
	
	public void BeDead()
	{
		//This is a hack for legacy animation - we will do this properly later
		if (!deathStarted)
		{
			animation.CrossFade("die", 0.1f);
			deathStarted = true;
			CharacterController controller = GetComponent<CharacterController>();
			controller.enabled = false;

			// Give player +10 heath (Power up)
			GameObject player = GameObject.Find("Player");
			if (player)
            {
				PlayerStatus playerStatus = player.GetComponent(typeof(PlayerStatus)) as PlayerStatus; ;
				playerStatus.AddHealth(10);
            }

		}
		
		moveDirection = new Vector3(0,0,0);

		if (!animation.isPlaying)
        {
			gameObject.SetActive(false);
			this.IsControllable = false;
		}
	}
	
	public void BeIdle()
	{
		animation.CrossFade("idle", 0.2f);	
		moveDirection = new Vector3(0,0,0);
	}

	public void Approach()
    {
		float diff = diffInPosition();

		GameObject player = GameObject.Find("Player");
		if(player)
        {
			// Walk vs run depending on distance between NPC and player
			if (diff <= 8.0f)
			{
				movementSpeed = 1.0f;
				animation["run"].speed = 0.5f;
			}
			else
			{
				movementSpeed = 5.0f;
				animation["run"].speed = 1.0f;
			}
			
			transform.LookAt(player.transform.position);
			animation.CrossFade("run", 0.2f);
			moveDirection = (player.transform.position - transform.position);
			moveDirection.Normalize();
        }
    }

	public void Attack()
    {
		movementSpeed = 0.0f;

		GameObject player = GameObject.Find("Player");
		if (player)
		{
			// Remain looking at enemy
			transform.LookAt(player.transform.position);

			// Applying cooldown for attack animation and damage
			if (attackWait <= 0)
            {
				animation.CrossFade("attack", 0.2f);
				playerStatus.ApplyDamage(attackValue);
				attackWait = attackSpeed;
			}
			else
            {
				attackWait = attackWait - Time.deltaTime;
            }
		}
	}
	public void RunAway()
	{
		movementSpeed = 1.0f;
		animation["run"].speed = 1.0f;

		// Finding the player's position and running away in the opposite direction
		GameObject player = GameObject.Find("Player");
		if (player)
		{
			transform.LookAt(player.transform.position);
			transform.Rotate(new Vector3(0, 180, 0));
			animation.CrossFade("run", 0.2f);
			moveDirection = (transform.position - player.transform.position);
			moveDirection.Normalize();
		}
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
			if (angleToTurn <= (fieldOfView / 2) && angleToTurn >= (fieldOfView / -2) && diff <= sightDistance && (!playerStatus.dead))
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

	// Checking if enemy is within range of attack
	public bool EnemyInRange()
    {
		GameObject player = GameObject.Find("Player");

		if (player)
        {
			PlayerStatus playerStatus = player.GetComponent(typeof(PlayerStatus)) as PlayerStatus;

			float diff = diffInPosition();

			// Checking distance
			if (diff <= attackDistance && (!playerStatus.dead))
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
	
	void Update ()
	{
		
		if (!isControllable)
			return;
		
		currentState.Execute(this);
		moveDirection.y -= gravity*Time.deltaTime;
		
		if(!isDead)
        {
			controller.Move(moveDirection * Time.deltaTime * movementSpeed);
		}
	}

	void OnDisable()
	{
		/*
		 * If you uncomment this, you need to somehow tell the PlayerController to update
		 * the enemies array by calling GameObject.FindGameObjectsWithTag("Enemy").
		 * Otherwise the reference to a desgtroyed GameObejct will still be in the enemies 
		 * array and you will get null pointer exceptions if you try to access it
		 */

		//Destroy(gameObject);

	}
}