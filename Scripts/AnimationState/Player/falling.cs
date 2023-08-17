using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class falling : StateMachineBehaviour
{
    private bool check;
    private bool isFalling;
    private PlayerController playerController;
    //OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        playerController = animator.gameObject.GetComponent<PlayerController>();
        check = true;
    }

    // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        playerController.audioSource.Stop();
        if(playerController.rb.velocity.y < -3f)
        {
            isFalling = true;
        }
    }

    // OnStateExit is called when a transition ends and the state machine finishes evaluating this state
    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if (!check)
        {
            playerController = animator.gameObject.GetComponent<PlayerController>();
            check = true;
        }
        if (isFalling)
        {
            playerController.audioSource.clip = playerController.playerSound.currentJumpingSound;
            playerController.audioSource.Play();
            isFalling = false;
        }
    }

    // OnStateMove is called right after Animator.OnAnimatorMove()
    //override public void OnStateMove(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    //{
    //    // Implement code that processes and affects root motion
    //}

    // OnStateIK is called right after Animator.OnAnimatorIK()
    //override public void OnStateIK(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    //{
    //    // Implement code that sets up animation IK (inverse kinematics)
    //}
}
