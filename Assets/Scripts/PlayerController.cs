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

    private float gravity = -8f;
    private float jumpHeight = 0.25f;


    private bool isControllable = true;

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
        transform.Rotate(0, Input.GetAxis("Mouse X") * rotateSpeed, 0);

        playerVelocity = new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical"));
        playerVelocity = transform.TransformDirection(playerVelocity);

        // Jumping functionality
        if (controller.isGrounded && Input.GetButtonDown("Jump"))
        {
            Debug.Log("Jump");
            yVelocity = Mathf.Sqrt(jumpHeight * -2f * (gravity));
        }

        // Crouching functionality
        if (controller.isGrounded && Input.GetButtonDown("Crouch"))
        {
            Debug.Log("Crouch");
        }

        if (Vector3.Distance(playerVelocity, new Vector3 (0, 0, 0)) > 1)
        {
            animation.CrossFade("Running", 0.1f);
        }

        yVelocity += gravity * Time.deltaTime;

        playerVelocity.y = yVelocity;


        CollisionFlags flags = controller.Move(playerVelocity * Time.deltaTime * moveSpeed);
        
    }
}
