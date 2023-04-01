using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationController : MonoBehaviour
{
    private Animator mAnimator;
    private PlayerController playerController;
    private CharacterController characterController;
    private CapsuleCollider capsuleCollider;

    // Start is called before the first frame update
    void Start()
    {
        mAnimator = GetComponent<Animator>();
        playerController = GetComponent<PlayerController>();
        characterController = GetComponent<CharacterController>();
        capsuleCollider = GetComponent<CapsuleCollider>();
    }

    // Update is called once per frame
    void Update()
    {
        if (mAnimator != null)
        {
            mAnimator.ResetTrigger("TriWalk");
            mAnimator.ResetTrigger("TriCrouchWalk");
            mAnimator.ResetTrigger("TriRun");
            mAnimator.ResetTrigger("TriIdle");
            //mAnimator.ResetTrigger("Tri");

            // Deals with animation related to standing upright
            if (playerController.moveSpeed == 10f && (Input.GetAxis("Horizontal") != 0 || Input.GetAxis("Vertical") != 0))
            {
                mAnimator.SetTrigger("TriWalk");
            }
            else if (playerController.moveSpeed == 5f && (Input.GetAxis("Horizontal") != 0 || Input.GetAxis("Vertical") != 0))
            {
                mAnimator.SetTrigger("TriCrouchWalk");
            }
            else if (playerController.moveSpeed == 20f && (Input.GetAxis("Horizontal") != 0 || Input.GetAxis("Vertical") != 0))
            {
                mAnimator.SetTrigger("TriRun");
            }
            else
            {
                mAnimator.SetTrigger("TriIdle");
            }

            // Deals with animation related to crouching
            if (Input.GetButtonDown("Crouch"))
            {
                mAnimator.SetTrigger("TriCrouch");
                
                // CharacterController and CapsuleCollider move when crouching to account for shorter character height
                characterController.center = new Vector3(0, 1, 0);
                capsuleCollider.center = new Vector3(0, 1, 0);
            }
            else if (Input.GetButton("Crouch"))
            {
                characterController.center = new Vector3(0, 1, 0);
                capsuleCollider.center = new Vector3(0, 1, 0);
            }
            else if (Input.GetButtonUp("Crouch"))
            {
                mAnimator.SetTrigger("TriCrouchUp");
                characterController.center = new Vector3(0, 1.15f, 0);
                capsuleCollider.center = new Vector3(0, 1.15f, 0);
            }

            /*if (playerController.IsDead)
            {
                mAnimator.SetTrigger("TriDead");
            }*/

        }
    }
}
