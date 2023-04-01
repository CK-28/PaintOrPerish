using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public CharacterController controller;
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
    }

    // Update is called once per frame
    void Update()
    {
        // Camera rotation using mouse input
        transform.Rotate(0, Input.GetAxis("Mouse X") * rotateSpeed, 0);

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
            controller.height = controller.height / 2;
        }
        if (Input.GetButtonUp("Crouch"))
        {
            moveSpeed = 10f;
            controller.height = controller.height * 2;
        }

        yVelocity += gravity * Time.deltaTime;

        playerVelocity.y = yVelocity;

        CollisionFlags flags = controller.Move(playerVelocity * Time.deltaTime * moveSpeed);
        
    }

    private void OnCollisionEnter(Collision collision)
    {
        Debug.Log("Hit!");
        if (collision.gameObject.tag == "paintballBlue") //collision is enemy paintball
        {
            Debug.Log("Dead!");
            // set dead
            isDead = true;
        }
    }

    // Checking if character is dead
    public bool IsDead
    {
        get { return isDead; }
        set { isDead = value; }
    }
}
