using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class running : StateMachineBehaviour
{
    private PlayerController playerController;
    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        playerController = animator.gameObject.GetComponent<PlayerController>();
        
    }

    // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        foreach (AudioClip audio in playerController.playerSound.jumpClip)
        {
            if(playerController.audioSource.clip == audio)
            {
                if (!playerController.audioSource.isPlaying)
                {
                    playerController.audioSource.clip = playerController.playerSound.currentWalkingSound;
                    if (!playerController.audioSource.isPlaying)
                    {
                        
                        playerController.audioSource.Play();
                    }
                }
                return;
            }
        }
        playerController.audioSource.clip = playerController.playerSound.currentWalkingSound;
        if (!playerController.audioSource.isPlaying)
        {
            
            playerController.audioSource.Play();
        }
    }

    // OnStateExit is called when a transition ends and the state machine finishes evaluating this state
    //override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    //{
    //    
    //}

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
