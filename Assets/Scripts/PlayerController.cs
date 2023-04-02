using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;


public class PlayerController : MonoBehaviour
{
    public CharacterController controller;
    public CapsuleCollider collider;
    public float rotateSpeed = 5f;
    public float moveSpeed = 10f;

    Vector3 playerVelocity;
    float yVelocity = 0;

    private float gravity = -2f;
    private float jumpHeight = 0.25f;


    private bool isControllable = true;
    public bool isDead = false;

    private new Animation animation;


    // Start is called before the first frame update
    void Start()
    {
        controller = GetComponent<CharacterController>();
        animation = GetComponent<Animation>();
        collider = GetComponent<CapsuleCollider>();
    }

    // Update is called once per frame
    void Update()
    {
        NavMeshAgent navMeshAgent = GetComponent<NavMeshAgent>();
        Animator mAnimator = GetComponent<Animator>();


        if (isControllable)
        {
            // Camera rotation using mouse input
            //transform.Rotate(0, Input.GetAxis("Mouse X") * rotateSpeed, 0);

            // Movement using keyboard input
            playerVelocity = new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical"));
            playerVelocity = transform.TransformDirection(playerVelocity);

            // Jumping when space is clicked
            if (controller.isGrounded && Input.GetButtonDown("Jump"))
            {
                Debug.Log("Jump");
                yVelocity = Mathf.Sqrt(jumpHeight * -1f * (gravity));
            }

            // Set running speed when shift is clicked
            if (Input.GetButtonDown("Run"))
            {
                moveSpeed = 20f;
            }
            if (Input.GetButtonUp("Run"))
            {
                moveSpeed = 10f;
            }

            // Set crouch speed and lower camera movement when control is clicked
            if (Input.GetButtonDown("Crouch"))
            {
                moveSpeed = 5f;
                // CharacterController and CapsuleCollider change size when crouching to account for smaller hitbox
                controller.height = 1.6f;
                collider.height = 1.6f;
            }
            if (Input.GetButtonUp("Crouch"))
            {
                moveSpeed = 10f;
                controller.height = 2.2f;
                collider.height = 2.2f;
            }

            yVelocity += gravity * Time.deltaTime;

            playerVelocity.y = yVelocity;

            CollisionFlags flags = controller.Move(playerVelocity * Time.deltaTime * moveSpeed);
        } else
        {
            GameObject spawn = GameObject.Find("SpawnANav");
            Vector3 spawnLocation = spawn.transform.position;
            if (!((controller.transform.position - spawnLocation).magnitude <= 5)) // character is not at spawn
            {
                navMeshAgent.enabled = true;
                navMeshAgent.destination = spawnLocation;
                mAnimator.SetTrigger("TriWalkArmRaise");
                mAnimator.ResetTrigger("TriDead");
            }
            else
            {
                isDead = false;
                isControllable = true;
                navMeshAgent.enabled = false;
                mAnimator.SetTrigger("TriIdle");
                mAnimator.ResetTrigger("TriDead");
            }
        }
        
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "paintball") //collision is enemy paintball
        {
            Debug.Log("Player died!");
            // set dead
            isDead = true;
            isControllable = false;
        }
    }

    // Checking if character is dead
    public bool IsDead
    {
        get { return isDead; }
        set { isDead = value; }
    }
}
