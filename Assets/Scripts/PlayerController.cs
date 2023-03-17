using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public CharacterController controller;
    public float rotateSpeed = 5f;
    public float moveSpeed = 10f;
    Vector3 playerVelocity;



    // Start is called before the first frame update
    void Start()
    {
        controller = GetComponent<CharacterController>();
    }

    // Update is called once per frame
    void Update()
    {
        playerVelocity = new Vector3(0, 0, Input.GetAxis("Vertical"));
        playerVelocity = transform.TransformDirection(playerVelocity);

        transform.Rotate(0, Input.GetAxis("Mouse X") * rotateSpeed, 0);

        //controller.transform.Rotate(rotateDirection, rotateSpeed * Time.deltaTime);
        CollisionFlags flags = controller.Move(playerVelocity * Time.deltaTime * moveSpeed);
    }
}
