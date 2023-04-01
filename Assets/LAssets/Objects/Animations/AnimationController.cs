using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationController : MonoBehaviour
{
    private Animator mAnimator;
    private PlayerController playerController;

    // Start is called before the first frame update
    void Start()
    {
        mAnimator = GetComponent<Animator>();
        playerController = GetComponent<PlayerController>();
    }

    // Update is called once per frame
    void Update()
    {
        //Debug.Log("");
        if (mAnimator != null)
        {
            mAnimator.ResetTrigger("TriWalk");
            mAnimator.ResetTrigger("TriCrouchWalk");
            mAnimator.ResetTrigger("TriRun");
            mAnimator.ResetTrigger("TriIdle");
            //mAnimator.ResetTrigger("Tri");


            if (playerController.moveSpeed == 10f && (Input.GetAxis("Horizontal") > 0 || Input.GetAxis("Vertical") > 0))
            {
                mAnimator.SetTrigger("TriWalk");
            }
            else if (playerController.moveSpeed == 5f && (Input.GetAxis("Horizontal") > 0 || Input.GetAxis("Vertical") > 0))
            {
                mAnimator.SetTrigger("TriCrouchWalk");
            }
            else if (playerController.moveSpeed == 20f && (Input.GetAxis("Horizontal") > 0 || Input.GetAxis("Vertical") > 0))
            {
                mAnimator.SetTrigger("TriRun");
            }
            else
            {
                mAnimator.SetTrigger("TriIdle");
            }
            
            if (Input.GetButtonDown("Crouch"))
            {
                mAnimator.SetTrigger("TriCrouch");
            }
            else if (Input.GetButtonUp("Crouch"))
            {
                mAnimator.SetTrigger("TriCrouchUp");
            }

            /*if (playerController.IsDead)
            {
                mAnimator.SetTrigger("TriDead");
            }*/

        }
    }
}
