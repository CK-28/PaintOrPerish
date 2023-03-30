using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationController : MonoBehaviour
{
    private Animator mAnimator;
    // Start is called before the first frame update
    void Start()
    {
        mAnimator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        Debug.Log("");
        if (mAnimator != null)
        {
            if (Input.GetButtonDown("Crouch"))
            {
                mAnimator.SetTrigger("TriCrouch");
            }
        }
    }
}
